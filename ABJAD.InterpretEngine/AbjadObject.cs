using ABJAD.Models;
using ABJAD.Models.Exceptions.InterpretingExceptions;

namespace ABJAD.InterpretEngine
{
    public enum AbjadType
    {
        رقم,        // double
        مقطع,       // string
        منطقي,      // bool
        خاص         // User Defined Type
    }

    public abstract class AbjadObject
    {
        public AbjadType Type { get; set; }

        public object Value { get; set; }

        public virtual AbjadObject OperatorPlus(AbjadObject other)
        {
            throw new AbjadUnallowedOperatorException("+", Type.ToString());
        }

        public virtual AbjadObject OperatorMinus(AbjadObject other)
        {
            throw new AbjadUnallowedOperatorException("-", Type.ToString());
        }

        public virtual AbjadObject OperatorTimes(AbjadObject other)
        {
            throw new AbjadUnallowedOperatorException("*", Type.ToString());
        }

        public virtual AbjadObject OperatorDiviededBy(AbjadObject other)
        {
            throw new AbjadUnallowedOperatorException("/", Type.ToString());
        }

        public virtual AbjadNumber OperatorUMinus()
        {
            throw new AbjadUnallowedOperatorException("-", Type.ToString());
        }

        public virtual AbjadBool OperatorBang()
        {
            throw new AbjadUnallowedOperatorException("!", Type.ToString());
        }

        public virtual AbjadBool OperatorLessThan(AbjadObject other)
        {
            throw new AbjadUnallowedOperatorException("<", Type.ToString());
        }

        public virtual AbjadBool OperatorLessEqual(AbjadObject other)
        {
            throw new AbjadUnallowedOperatorException("<=", Type.ToString());
        }

        public virtual AbjadBool OperatorGreaterThan(AbjadObject other)
        {
            throw new AbjadUnallowedOperatorException(">", Type.ToString());
        }

        public virtual AbjadBool OperatorGreaterEqual(AbjadObject other)
        {
            throw new AbjadUnallowedOperatorException(">=", Type.ToString());
        }

        public virtual AbjadBool OperatorEqual(AbjadObject other)
        {
            return new AbjadBool(Value.Equals(other.Value));
        }

        public virtual AbjadBool OperatorAnd(AbjadObject other)
        {
            throw new AbjadUnallowedOperatorException("&&", Type.ToString());
        }

        public virtual AbjadBool OperatorOr(AbjadObject other)
        {
            throw new AbjadUnallowedOperatorException("||", Type.ToString());
        }

        public virtual AbjadString ToStr()
        {
            return new AbjadString(Value.ToString());
        }

        public virtual AbjadNumber ToNumber()
        {
            return new AbjadNumber(Value);
        }

        public virtual AbjadBool ToBool()
        {
            return new AbjadBool(bool.Parse(Value.ToString()));
        }

        public new virtual AbjadString GetType()
        {
            return new AbjadString(Type.ToString());
        }
    }

    public class AbjadString : AbjadObject
    {
        public AbjadString(string str)
        {
            Value = str;
            Type = AbjadType.مقطع;
        }

        public override AbjadObject OperatorPlus(AbjadObject other)
        {
            return new AbjadString(Value as string + other.Value.ToString());
        }

        public override AbjadBool OperatorGreaterEqual(AbjadObject other)
        {
            return new AbjadBool(Value.ToString().CompareTo(other.Value.ToString()) >= 0);
        }

        public override AbjadBool OperatorGreaterThan(AbjadObject other)
        {
            return new AbjadBool(Value.ToString().CompareTo(other.Value.ToString()) > 0);
        }

        public override AbjadBool OperatorLessEqual(AbjadObject other)
        {
            return new AbjadBool(Value.ToString().CompareTo(other.Value.ToString()) <= 0);
        }

        public override AbjadBool OperatorLessThan(AbjadObject other)
        {
            return new AbjadBool(Value.ToString().CompareTo(other.Value.ToString()) < 0);
        }

        public override AbjadBool ToBool()
        {
            if (Value as string == Constants.Boolean.True)
                return new AbjadBool(true);
            else if (Value as string == Constants.Boolean.False)
                return new AbjadBool(false);
            else
                throw new AbjadCastingException("string", "bool");
        }
    }

    public class AbjadNumber : AbjadObject
    {
        public AbjadNumber(double number)
        {
            Value = number;
            Type = AbjadType.رقم;
        }

        public AbjadNumber(object number)
        {
            Value = double.Parse(number.ToString());
        }

        public override AbjadObject OperatorDiviededBy(AbjadObject other)
        {
            if ((double)other.Value == 0)
            {
                throw new AbjadDivisionByZeroException();
            }

            return new AbjadNumber((double)Value / (double)other.Value);
        }

        public override AbjadObject OperatorMinus(AbjadObject other)
        {
            return new AbjadNumber((double)Value - (double)other.Value);
        }

        public override AbjadObject OperatorPlus(AbjadObject other)
        {
            return new AbjadNumber((double)Value + (double)other.Value);
        }

        public override AbjadObject OperatorTimes(AbjadObject other)
        {
            return new AbjadNumber((double)Value * (double)other.Value);
        }

        public override AbjadNumber OperatorUMinus()
        {
            return new AbjadNumber(-1 * (double)Value);
        }

        public override AbjadBool OperatorGreaterEqual(AbjadObject other)
        {
            return new AbjadBool((double)Value >= (double)other.Value);
        }

        public override AbjadBool OperatorGreaterThan(AbjadObject other)
        {
            return new AbjadBool((double)Value > (double)other.Value);
        }

        public override AbjadBool OperatorLessEqual(AbjadObject other)
        {
            return new AbjadBool((double)Value <= (double)other.Value);
        }

        public override AbjadBool OperatorLessThan(AbjadObject other)
        {
            return new AbjadBool((double)Value < (double)other.Value);
        }
    }

    public class AbjadBool : AbjadObject
    {
        public AbjadBool(bool boolean)
        {
            Value = boolean;
            Type = AbjadType.منطقي;
        }

        public override AbjadBool OperatorAnd(AbjadObject other)
        {
            return new AbjadBool((bool)Value && (bool)other.Value);
        }

        public override AbjadBool OperatorOr(AbjadObject other)
        {
            return new AbjadBool((bool)Value || (bool)other.Value);
        }

        public override AbjadBool OperatorBang()
        {
            return new AbjadBool(!(bool)Value);
        }

        public override AbjadNumber ToNumber()
        {
            return new AbjadNumber((bool)Value ? 1 : 0);
        }

        public override AbjadString ToStr()
        {
            return new AbjadString((bool)Value ? Constants.Boolean.True : Constants.Boolean.False);
        }
    }
}