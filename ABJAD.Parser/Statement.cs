using ABJAD.Models;
using System.Collections.Generic;

namespace ABJAD.Parser
{
    public abstract class Statement
    {
        public interface Visitor<T>
        {
            T VisitExprStmt(ExprStmt stmt);

            T VisitIfStmt(IfStmt stmt);

            T VisitWhileStmt(WhileStmt stmt);

            T VisitForStmt(ForStmt stmt);

            T VisitBlockStmt(BlockStmt stmt);

            T VisitReturnStmt(ReturnStmt stmt);

            T VisitAssignmentStmt(AssignmentStmt stmt);

            T VisitPrintStmt(PrintStmt stmt);
        }

        public abstract T Accept<T>(Visitor<T> visitor);

        public class ExprStmt : Statement
        {
            public ExprStmt(Expression expr)
            {
                Expr = expr;
            }

            public Expression Expr { get; set; }

            public override T Accept<T>(Visitor<T> visitor)
            {
                return visitor.VisitExprStmt(this);
            }
        }

        public class IfStmt : Statement
        {
            public IfStmt(Expression condition, BlockStmt ifBlock, BlockStmt elseBlock)
            {
                Condition = condition;
                IfBlock = ifBlock;
                ElseBlock = elseBlock;
            }

            public Expression Condition { get; set; }

            public BlockStmt IfBlock { get; set; }

            public BlockStmt ElseBlock { get; set; }

            public override T Accept<T>(Visitor<T> visitor)
            {
                return visitor.VisitIfStmt(this);
            }
        }

        public class WhileStmt : Statement
        {
            public WhileStmt(Expression condition, BlockStmt block)
            {
                Condition = condition;
                Block = block;
            }

            public Expression Condition { get; set; }

            public BlockStmt Block { get; set; }

            public override T Accept<T>(Visitor<T> visitor)
            {
                return visitor.VisitWhileStmt(this);
            }
        }

        public class ForStmt : Statement
        {
            public ForStmt(BlockStmt block, AssignmentStmt assignment, Expression condition, Declaration declaration)
            {
                Block = block;
                Assignment = assignment;
                Condition = condition;
                Declaration = declaration;
            }

            public Declaration Declaration { get; set; }

            public Expression Condition { get; set; }

            public AssignmentStmt Assignment { get; set; }

            public BlockStmt Block { get; set; }

            public override T Accept<T>(Visitor<T> visitor)
            {
                return visitor.VisitForStmt(this);
            }
        }

        public class BlockStmt : Statement
        {
            public BlockStmt(List<Binding> bindingList)
            {
                BindingList = bindingList;
            }

            public List<Binding> BindingList { get; set; }

            public override T Accept<T>(Visitor<T> visitor)
            {
                return visitor.VisitBlockStmt(this);
            }
        }

        public class ReturnStmt : Statement
        {
            public ReturnStmt(Expression expr)
            {
                Expr = expr;
            }

            public Expression Expr { get; set; }

            public override T Accept<T>(Visitor<T> visitor)
            {
                return visitor.VisitReturnStmt(this);
            }
        }

        public class AssignmentStmt : Statement
        {
            public AssignmentStmt(Token variable, Expression value)
            {
                Variable = variable;
                Value = value;
            }

            public Token Variable { get; set; }

            public Expression Value { get; set; }

            public override T Accept<T>(Visitor<T> visitor)
            {
                return visitor.VisitAssignmentStmt(this);
            }
        }

        public class PrintStmt : Statement
        {
            public PrintStmt(Expression expr)
            {
                Expr = expr;
            }

            public Expression Expr { get; set; }

            public override T Accept<T>(Visitor<T> visitor)
            {
                return visitor.VisitPrintStmt(this);
            }
        }
    }
}
