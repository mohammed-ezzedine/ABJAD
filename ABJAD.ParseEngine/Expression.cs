using ABJAD.Models;
using System.Collections.Generic;

namespace ABJAD.ParseEngine
{
    public abstract class Expression
    {
        public interface IVisitor<T>
        {
            T VisitCallExpr(CallExpr expr);

            T VisitFieldExpr(FieldExpr expr);

            T VisitInstExpr(InstExpr expr);

            T VisitToStrExpr(ToStrExpr expr);

            T VisitToNumberExpr(ToNumberExpr expr);

            T VisitToBoolExpr(ToBoolExpr expr);

            T VisitTypeofExpr(TypeofExpr expr);

            T VisitUnaryExpr(UnaryExpr expr);

            T VisitBinaryExpr(BinaryExpr expr);

            T VisitGroupExpr(GroupExpr expr);

            T VisitPrimitiveExpr(PrimitiveExpr expr);
        }

        public abstract T Accept<T>(IVisitor<T> visitor);

        public class CallExpr : Expression
        {
            public CallExpr(Token @class, Token func, List<Expression> parameters)
            {
                Class = @class;
                Func = func;
                Parameters = parameters;
            }

            public Token Class { get; set; }
            
            public Token Func { get; set; }
            
            public List<Expression> Parameters { get; set; }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitCallExpr(this);
            }
        }

        public class FieldExpr : Expression
        {
            public FieldExpr(Token @class, Token field)
            {
                Class = @class;
                Field = field;
            }

            public Token Class { get; set; }

            public Token Field { get; set; }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitFieldExpr(this);
            }
        }

        public class InstExpr : Expression
        {
            public InstExpr(Token @class, List<Expression> parameters)
            {
                Class = @class;
                Parameters = parameters;
            }

            public Token Class { get; set; }

             public List<Expression> Parameters { get; set; }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitInstExpr(this);
            }
        }

        public class ToStrExpr : Expression
        {
            public ToStrExpr(Expression input)
            {
                Input = input;
            }

            public Expression Input { get; set; }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitToStrExpr(this);
            }
        }

        public class ToNumberExpr : Expression
        {
            public ToNumberExpr(Expression input)
            {
                Input = input;
            }

            public Expression Input { get; set; }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitToNumberExpr(this);
            }
        }

        public class ToBoolExpr : Expression
        {
            public ToBoolExpr(Expression input)
            {
                Input = input;
            }

            public Expression Input { get; set; }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitToBoolExpr(this);
            }
        }

        public class TypeofExpr : Expression
        {
            public TypeofExpr(Expression input)
            {
                Input = input;
            }

            public Expression Input { get; set; }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitTypeofExpr(this);
            }
        }

        public class UnaryExpr : Expression
        {
            public UnaryExpr(Token operation, Expression operand)
            {
                Operation = operation;
                Operand = operand;
            }

            public Expression Operand { get; set; }

            public Token Operation { get; set; }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitUnaryExpr(this);
            }
        }

        public class BinaryExpr : Expression
        {
            public BinaryExpr(Expression operand1, Token operation, Expression operand2)
            {
                Operand1 = operand1;
                Operand2 = operand2;
                Operation = operation;
            }

            public Expression Operand1 { get; set; }

            public Expression Operand2 { get; set; }

            public Token Operation { get; set; }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitBinaryExpr(this);
            }
        }

        public class GroupExpr : Expression
        {
            public GroupExpr(Expression expr)
            {
                Expr = expr;
            }

            public Expression Expr { get; set; }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitGroupExpr(this);
            }
        }

        public class PrimitiveExpr : Expression
        {
            public PrimitiveExpr(Primitive primitive)
            {
                Primitive = primitive;
            }

            public Primitive Primitive { get; set; }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitPrimitiveExpr(this);
            }
        }
    }
}
