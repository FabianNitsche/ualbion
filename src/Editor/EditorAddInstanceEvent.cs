﻿namespace UAlbion.Editor
{
    public class EditorAddInstanceEvent : IEditorEvent
    {
        public int Id { get; }
        public string CollectionName { get; }
        public int Index { get; }
    }
}