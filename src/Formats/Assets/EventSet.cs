﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SerdesNet;
using UAlbion.Api;
using UAlbion.Config;
using UAlbion.Formats.MapEvents;

namespace UAlbion.Formats.Assets;

public class EventSet : IEventSet
{
    ushort[] _chainMapping;
    AssetId IEventSet.Id => Id;
    [JsonIgnore] public TextId TextId => Id.ToEventText();
    [JsonInclude] public EventSetId Id { get; private init; }
    [JsonInclude] public List<ushort> Chains { get; private set; }
    [JsonIgnore] public List<EventNode> Events { get; private set; }

    public EventSet() { }

    public EventSet(EventSetId id, IEnumerable<EventNode> nodes, IEnumerable<ushort> chains)
    {
        if (nodes == null) throw new ArgumentNullException(nameof(nodes));
        if (chains == null) throw new ArgumentNullException(nameof(chains));
        Id = id;
        Events = nodes.ToList();
        Chains = chains.ToList();
    }

    public ushort GetChainForEvent(ushort index) => index >= Events.Count ? EventNode.UnusedEventId : _chainMapping[index];
    public string[] EventStrings // Used for JSON
    {
        get => Events?.Select(x => x.ToString()).ToArray();
        set
        {
            Events = value?.Select(EventNode.Parse).ToList() ?? new List<EventNode>();
            foreach (var e in Events)
                e.Unswizzle(Events);
        }
    }

    public static EventSet Serdes(EventSetId id, EventSet set, AssetMapping mapping, ISerializer s)
    {
        if (s == null) throw new ArgumentNullException(nameof(s));
        set ??= new EventSet { Id = id };
        ushort chainCount = s.UInt16("ChainCount", (ushort)(set.Chains?.Count ?? 0));
        ushort eventCount = s.UInt16("TotalEventCount", (ushort)(set.Events?.Count ?? 0));

        set.Chains ??= new List<ushort>(chainCount);
        set.Events ??= new List<EventNode>(eventCount);

        while(set.Chains.Count < chainCount) set.Chains.Add(EventNode.UnusedEventId);
        while(set.Events.Count < eventCount) set.Events.Add(null);

        for (int i = 0; i < chainCount; i++)
            set.Chains[i] = s.UInt16(null, set.Chains[i]);

        for (ushort i = 0; i < set.Events.Count; i++)
            set.Events[i] = MapEvent.SerdesNode(i, set.Events[i], s, id, id.ToEventText(), mapping);

        foreach (var e in set.Events)
            e.Unswizzle(set.Events);

        // TODO: Unify this with the code in BaseMapData
        var sortedChains = set.Chains
            .Select((eventId, chainId) => (eventId, chainId))
            .OrderBy(x => x)
            .ToArray();

        set._chainMapping = new ushort[set.Events.Count];
        for (int i = 0, j = 0; i < set.Events.Count; i++)
        {
            while (sortedChains.Length > j + 1 && sortedChains[j].eventId < i) j++;
            set._chainMapping[i] = (ushort)sortedChains[j].chainId;
        }
        return set;
    }
}