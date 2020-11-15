namespace ABJAD.ParseEngine
{
    public abstract class Binding
    {
        public interface IVisitor<T>
        {
            T VisitStmtBinding(StmtBinding binding);

            T VisitDeclBinding(DeclBinding binding);
        }

        public abstract T Accept<T>(IVisitor<T> visitor);

        public class StmtBinding : Binding
        {
            public StmtBinding(Statement stmt)
            {
                Stmt = stmt;
            }

            public Statement Stmt { get; set; }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitStmtBinding(this);
            }
        }

        public class DeclBinding : Binding
        {
            public DeclBinding(Declaration decl)
            {
                Decl = decl;
            }

            public Declaration Decl { get; set; }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitDeclBinding(this);
            }
        }
    }
}
