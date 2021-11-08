﻿namespace UAlbion.Scripting.Ast
{
    public interface IAstVisitor
    {
        void Visit(AlbionEvent e);
        void Visit(BinaryOp binaryOp);
        void Visit(BreakStatement breakStatement);
        void Visit(ContinueStatement continueStatement);
        void Visit(DoLoop doLoop);
        void Visit(EmptyNode empty);
        void Visit(IfThen ifThen);
        void Visit(IfThenElse ifElse);
        void Visit(Label label);
        void Visit(Name name);
        void Visit(Negation negation);
        void Visit(Numeric numeric);
        void Visit(Sequence sequence);
        void Visit(SeseRegion sese);
        void Visit(Statement statement);
        void Visit(WhileLoop whileLoop);
    }
}