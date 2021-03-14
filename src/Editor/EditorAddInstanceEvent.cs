﻿using UAlbion.Api;

namespace UAlbion.Editor
{
    public class EditorAddInstanceEvent : Event, IEditorEvent
    {
        public int Id { get; }
        public string CollectionName { get; }
        public int Index { get; }
    }
}