namespace ABJAD.ParseEngine
{
    public abstract class Primitive
    {
        public interface IVisitor<T>
        {
            T VisitNumberConst(NumberConst numberConst);

            T VisitStringConst(StringConst stringConst);

            T VisitTrue(True @true);

            T VisitFalse(False @false);

            T VisitNull(Null @null);

            T VisitIdentifier(Identifier identifier);
        }

        public abstract T Accept<T>(IVisitor<T> visitor);

        public class NumberConst : Primitive
        {
            public NumberConst(string value)
            {
                this.value = double.Parse(value);
            }

            public double value { get; set; }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitNumberConst(this);
            }
        }

        public class StringConst : Primitive
        {
            public StringConst(string value)
            {
                this.value = value;
            }

            public string value { get; set; }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitStringConst(this);
            }
        }

        public class True : Primitive
        {
            public True()
            {
                value = true;
            }

            public bool value { get; set; }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitTrue(this);
            }
        }

        public class False : Primitive
        {
            public False()
            {
                value = false;
            }

            public bool value { get; set; }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitFalse(this);
            }
        }

        public class Null : Primitive
        {
            public Null()
            {
                value = null;
            }

            public dynamic value { get; set; }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitNull(this);
            }
        }

        public class Identifier : Primitive
        {
            public Identifier(string value)
            {
                this.value = value;
            }

            public string value { get; set; }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitIdentifier(this);
            }
        }
    }
}
