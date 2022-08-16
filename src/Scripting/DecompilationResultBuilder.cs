﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UAlbion.Api.Eventing;
using UAlbion.Scripting.Ast;

namespace UAlbion.Scripting;

public class DecompilationResultBuilder : IScriptBuilder
{
    readonly StringBuilder _sb = new();
    readonly List<ScriptPart> _parts = new();
    readonly List<EventRegion> _eventRegions = new();
    readonly Dictionary<int, int> _eventRegionLookup = new();

    public DecompilationResultBuilder(bool useNumericIds) => UseNumericIds = useNumericIds;

    public bool UseNumericIds { get; }
    public int Length => _sb.Length;

    public void Append(char c) => Add(ScriptPartType.Text, c, (x, sb) => sb.Append(x));
    public void Append(string text) => Add(ScriptPartType.Text, text, (x, sb) => sb.Append(x));
    public void Append(object value) => Add(ScriptPartType.Text, value, (x, sb) => sb.Append(x));
    public void AppendLine() => Add(ScriptPartType.Text, 0, (_, sb) => sb.AppendLine());
    public void AppendLine(string text) => Add(ScriptPartType.Text, text, (x, sb) => sb.AppendLine(x));
    public void Add(ScriptPartType type, char c) => Add(type, c, (x, sb) => sb.Append(x));
    public void Add(ScriptPartType type, string text) => Add(type, text, (x, sb) => sb.Append(x));

    public void EventScope<T>(int eventId, T context, Action<T> func)
    {
        if (func == null)
            throw new ArgumentNullException(nameof(func));

        int start = _sb.Length;
        func(context);
        int end = _sb.Length;

        _eventRegionLookup[eventId] = _eventRegions.Count;
        _eventRegions.Add(new EventRegion(eventId, new Range(start, end)));
    }

    public void Add<T>(ScriptPartType type, T context, Action<T, StringBuilder> func)
    {
        if (func == null)
            throw new ArgumentNullException(nameof(func));

        int start = _sb.Length;
        func(context, _sb);
        int end = _sb.Length;

        // Can we extend the last part?
        if (_parts.Count > 0 && _parts[^1].Type == type)
            _parts[^1] = new ScriptPart(type, new Range(_parts[^1].Range.Start, end));
        else
            _parts.Add(new ScriptPart(type, new Range(start, end)));
    }

    public DecompilationResult Build(IEnumerable<ICfgNode> nodes)
    {
        if (nodes == null) throw new ArgumentNullException(nameof(nodes));
        return new(_sb.ToString(), _parts.ToArray(), _eventRegions.ToArray(), _eventRegionLookup, nodes.ToArray());
    }
}