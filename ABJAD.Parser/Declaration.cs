using ABJAD.Models;
using System.Collections.Generic;

namespace ABJAD.Parser
{
    public abstract class Declaration
    {
        public interface IVisitor<T>
        {
            T VisitFuncDecl(FuncDecl declaration);

            T VisitConstDecl(ConstDecl declaration);

            T VisitVarDecl(VarDecl declaration);

            T VisitClassDecl(ClassDecl declaration);
        }

        public abstract T Accept<T>(IVisitor<T> visitor);

        public class FuncDecl : Declaration
        {
            public FuncDecl(Token name, List<Expression> parameters, Statement.BlockStmt block)
            {
                Name = name;
                Parameters = parameters;
                Block = block;
            }

            public Token Name { get; set; }

            public List<Expression> Parameters { get; set; }

            public Statement.BlockStmt Block { get; set; }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitFuncDecl(this);
            }
        }

        public class ConstDecl : Declaration
        {
            public ConstDecl(Token name, Expression value)
            {
                Name = name;
                Value = value;
            }

            public Token Name { get; set; }

            public Expression Value { get; set; }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitConstDecl(this);
            }
        }

        public class VarDecl : Declaration
        {
            public VarDecl(Token name, Expression value)
            {
                Name = name;
                Value = value;
            }

            public Token Name { get; set; }

            public Expression Value { get; set; }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitVarDecl(this);
            }
        }

        public class ClassDecl : Declaration
        {
            public ClassDecl(Token name, Statement block)
            {
                Name = name;
                Block = block;
            }

            public Token Name { get; set; }

            public Statement Block { get; set; }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitClassDecl(this);
            }
        }
    }
}
