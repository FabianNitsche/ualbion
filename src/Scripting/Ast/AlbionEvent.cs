﻿using UAlbion.Api;

namespace UAlbion.Scripting.Ast
{
    public record AlbionEvent(IEvent Event) : ICfgNode
    {
        public override string ToString() => Event.ToString();
        public void Accept(IAstVisitor visitor) => visitor.Visit(this);
        public int Priority => int.MaxValue;
    }
}