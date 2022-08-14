﻿using System;
using System.Globalization;
using System.IO;
using System.Linq;
using UAlbion.Api;
using UAlbion.Api.Eventing;
using UAlbion.Config;
using UAlbion.Core;
using UAlbion.Core.Veldrid;
using UAlbion.Core.Veldrid.Etm;
using UAlbion.Core.Veldrid.Skybox;
using UAlbion.Core.Veldrid.Sprites;
using UAlbion.Formats;
using UAlbion.Game.Assets;
using UAlbion.Game.Events;
using UAlbion.Game.Text;

// args for testing isometric map export: -b Base Unpacked -t "Labyrinth Map" -id "Labyrinth.Jirinaar Map.Jirinaar"
// args for full asset export: -b Base Unpacked
// args for re-pack of exported assets: -b Unpacked Repacked
// args for combining mods for original: -b "Base SomeMod SomeOtherMod" Repacked

#pragma warning disable CA2000 // Dispose objects before losing scopes
namespace UAlbion;

static class Program
{
    static void Main(string[] args)
    {
        PerfTracker.IsTracing = true;
        PerfTracker.StartupEvent("Entered main");
        AssetSystem.LoadEvents();
        PerfTracker.StartupEvent("Built event parsers");

        var commandLine = new CommandLineOptions(args);
        if (commandLine.Mode == ExecutionMode.Exit)
            return;

        CultureInfo.CurrentCulture
            = CultureInfo.CurrentUICulture
            = CultureInfo.DefaultThreadCurrentCulture
            = CultureInfo.DefaultThreadCurrentUICulture
            = CultureInfo.InvariantCulture;

        PerfTracker.StartupEvent($"Running as {commandLine.Mode}");
        var disk = new FileSystem(Directory.GetCurrentDirectory());
        var jsonUtil = new FormatJsonUtil();

        var baseDir = ConfigUtil.FindBasePath(disk);
        if (baseDir == null)
            throw new InvalidOperationException("No base directory could be found.");

        PerfTracker.StartupEvent($"Found base directory {baseDir}");

        if (commandLine.Mode == ExecutionMode.ConvertAssets)
        {
            using var converter = new AssetConverter(
                AssetMapping.Global,
                disk,
                jsonUtil,
                commandLine.ConvertFrom,
                commandLine.ConvertTo);

            converter.Convert(
                commandLine.DumpIds,
                commandLine.DumpAssetTypes,
                commandLine.ConvertFilePattern);

            return;
        }

        var exchange = AssetSystem.Setup(
            baseDir,
            AssetMapping.Global,
            disk,
            jsonUtil,
            commandLine.Mods);

        IRenderPass mainPass = null;
        if (commandLine.NeedsEngine)
            mainPass = BuildEngine(commandLine, exchange);

        exchange.Attach(new StdioConsoleReader()); // TODO: Only add this if running with a console window

        var assets = exchange.Resolve<IAssetManager>();
        AutodetectLanguage(exchange, assets);

        switch (commandLine.Mode) // ConvertAssets handled above as it requires a specialised asset system setup
        {
            case ExecutionMode.Game: Albion.RunGame(exchange, mainPass, commandLine); break;
            case ExecutionMode.BakeIsometric: IsometricTest.Run(exchange, commandLine); break;

            case ExecutionMode.DumpData:
                PerfTracker.BeginFrame(); // Don't need to show verbose startup logging while dumping
                var tf = new TextFormatter();
                exchange.Attach(tf);
                var parsedIds = commandLine.DumpIds?.Select(AssetId.Parse).ToArray();

                if ((commandLine.DumpFormats & DumpFormats.Json) != 0)
                {
                    var dumper = new DumpJson();
                    exchange.Attach(dumper);
                    dumper.Dump(baseDir, commandLine.DumpAssetTypes, parsedIds);
                    dumper.Remove();
                }

                if ((commandLine.DumpFormats & DumpFormats.Text) != 0)
                {
                    var dumper = new DumpText();
                    exchange.Attach(dumper);
                    dumper.Dump(baseDir, commandLine.DumpAssetTypes, parsedIds);
                    dumper.Remove();
                }

                if ((commandLine.DumpFormats & DumpFormats.Png) != 0)
                {
                    var dumper = new DumpGraphics(commandLine.DumpFormats);
                    exchange.Attach(dumper);
                    dumper.Dump(baseDir, commandLine.DumpAssetTypes, parsedIds);
                    dumper.Remove();
                }

                if ((commandLine.DumpFormats & DumpFormats.Annotated) != 0)
                {
                    var dumper = new DumpAnnotated();
                    exchange.Attach(dumper);
                    dumper.Dump(baseDir, commandLine.DumpAssetTypes, parsedIds);
                    dumper.Remove();
                }

                //if ((commandLine.DumpFormats & DumpFormats.Tiled) != 0)
                //    DumpTiled.Dump(baseDir, assets, commandLine.DumpAssetTypes, parsedIds);
                break;

            case ExecutionMode.Exit: break;
        }

        Console.WriteLine("Exiting");
        exchange.Dispose();
    }

    static void AutodetectLanguage(EventExchange exchange, IAssetManager assets)
    {
        // Check the language saved in settings.json first
        if (assets.IsStringDefined(Base.SystemText.MainMenu_MainMenu, null)) 
            return;

        // Otherwise just use the first one we can find
        var modApplier = exchange.Resolve<IModApplier>();
        foreach (var language in modApplier.Languages.Keys)
        {
            if (assets.IsStringDefined(Base.SystemText.MainMenu_MainMenu, language))
            {
                exchange.Raise(new SetLanguageEvent(language), null);
                return;
            }
        }
    }

    static IRenderPass BuildEngine(CommandLineOptions commandLine, EventExchange exchange)
    {
        PerfTracker.StartupEvent("Creating engine");
        var framebuffer = new MainFramebuffer();
        var renderPass = new RenderPass("Main Pass", framebuffer);
        renderPass // TODO: Populate from json so mods can add new render methods
            .Add(new SpriteRenderer(framebuffer))
            .Add(new BlendedSpriteRenderer(framebuffer))
            .Add(new TileRenderer(framebuffer))
            .Add(new EtmRenderer(framebuffer))
            .Add(new SkyboxRenderer(framebuffer))
            .Add(new DebugGuiRenderer(framebuffer))
            ;

        var engine = new Engine(commandLine.Backend, commandLine.UseRenderDoc, commandLine.StartupOnly, true);
        engine.AddRenderPass(renderPass);

#pragma warning disable CA2000 // Dispose objects before losing scopes
        var pathResolver = exchange.Resolve<IPathResolver>();
        var shaderCache = new ShaderCache(pathResolver.ResolvePath("$(CACHE)/ShaderCache"));
        var shaderLoader = new ShaderLoader();

        foreach (var shaderPath in exchange.Resolve<IModApplier>().ShaderPaths)
            shaderLoader.AddShaderDirectory(shaderPath);
#pragma warning restore CA2000 // Dispose objects before losing scopes

        var engineServices = new Container("Engine", 
            shaderCache,
            shaderLoader,
            framebuffer,
            renderPass,
            engine,
            new ResourceLayoutSource());

        exchange.Attach(engineServices);
        return renderPass;
    }
}
#pragma warning restore CA2000 // Dispose objects before losing scopes
