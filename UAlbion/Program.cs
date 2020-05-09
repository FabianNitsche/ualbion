﻿using System;
using System.Text;
using UAlbion.Api;
using UAlbion.Core;
using UAlbion.Core.Veldrid;
using UAlbion.Core.Visual;
using UAlbion.Formats;
using UAlbion.Game;
using UAlbion.Game.Assets;
using UAlbion.Game.Entities;
using UAlbion.Game.Gui.Text;
using UAlbion.Game.Settings;
using UAlbion.Game.Veldrid.Assets;
using UAlbion.Game.Veldrid.Audio;
using UAlbion.Game.Veldrid.Debugging;

namespace UAlbion
{
    static class Program
    {
        static void Main(string[] args)
        {
            /*
            Console.WriteLine("Entry point reached. Press enter to continue");
            Console.ReadLine(); //*/

            PerfTracker.StartupEvent("Entered main");
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // Required for code page 850 support in .NET Core
            PerfTracker.StartupEvent("Registered encodings");

            var commandLine = new CommandLineOptions(args);

            var baseDir = FormatUtil.FindBasePath();
            if (baseDir == null)
                throw new InvalidOperationException("No base directory could be found.");

            PerfTracker.StartupEvent($"Found base directory {baseDir}");
            PerfTracker.StartupEvent("Registering asset manager");
            var factory = new VeldridCoreFactory();
            using var assets = new AssetManager()
                .AddAssetLocator(new StandardAssetLocator())
                .AddAssetLocator(new AssetConfigLocator())
                .AddAssetLocator(new CoreSpriteLocator())
                .AddAssetLocator(new MetaFontLocator(factory))
                .AddAssetLocator(new NewStringLocator())
                .AddAssetLocator(new SoundBankLocator())
                .AddAssetPostProcessor(new AlbionSpritePostProcessor())
                .AddAssetPostProcessor(new ImageSharpPostProcessor())
                .AddAssetPostProcessor(new InterlacedBitmapPostProcessor())
                ;

            var logExchange = new LogExchange();
            using var exchange = new EventExchange(logExchange)
                .Register<ICoreFactory>(factory)
                .Attach(new StdioConsoleLogger())
                .Attach(new ImGuiConsoleLogger())
                .Attach(Settings.Load(baseDir))
                .Attach(assets) // Need to register settings first, as the AssetConfigLocator relies on it.
                ;

            Engine.GlobalExchange = exchange;

            PerfTracker.StartupEvent("Registered asset manager");
            PerfTracker.StartupEvent($"Running as {commandLine.Mode}");

            switch(commandLine.Mode)
            {
                case ExecutionMode.Game:
                case ExecutionMode.GameWithSlavedAudio:
                    Albion.RunGame(exchange, baseDir, commandLine);
                    break;

                case ExecutionMode.AudioSlave: 
                    exchange.Attach(new AudioManager(true));
                    break;

                case ExecutionMode.Editor: break; // TODO
                case ExecutionMode.SavedGameTests: SavedGameTests.RoundTripTest(baseDir); break;

                case ExecutionMode.DumpData:
                    var textManager = new TextManager();
                    exchange.Attach(textManager);

                    Dump.CoreSprites(assets, baseDir);
                    Dump.CharacterSheets(assets, textManager, baseDir);
                    Dump.Chests(assets, baseDir);
                    Dump.ItemData(assets, baseDir);
                    Dump.MapEvents(assets, baseDir);
                    Dump.EventSets(assets, baseDir);
                    Dump.MapData(assets, textManager, baseDir);
                    Dump.ThreeDMapAndLabInfo(assets, baseDir);
                    break;

                case ExecutionMode.Exit: break;
            }
        }
    }
}
