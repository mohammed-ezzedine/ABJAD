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

        public virtual AbjadObject OperatorPlus(AbjadObject other, int line, int index)
        {
            throw new AbjadUnallowedOperatorException("+", Type.ToString(), line, index);
        }

        public virtual AbjadObject OperatorMinus(AbjadObject other, int line, int index)
        {
            throw new AbjadUnallowedOperatorException("-", Type.ToString(), line, index);
        }

        public virtual AbjadObject OperatorTimes(AbjadObject other, int line, int index)
        {
            throw new AbjadUnallowedOperatorException("*", Type.ToString(), line, index);
        }

        public virtual AbjadObject OperatorModulo(AbjadObject other, int line, int index)
        {
            throw new AbjadUnallowedOperatorException("%", Type.ToString(), line, index);
        }

        public virtual AbjadObject OperatorDiviededBy(AbjadObject other, int line, int index)
        {
            throw new AbjadUnallowedOperatorException("/", Type.ToString(), line, index);
        }

        public virtual AbjadNumber OperatorUMinus(int line, int index)
        {
            throw new AbjadUnallowedOperatorException("-", Type.ToString(), line, index);
        }

        public virtual AbjadBool OperatorBang(int line, int index)
        {
            throw new AbjadUnallowedOperatorException("!", Type.ToString(), line, index);
        }

        public virtual AbjadBool OperatorLessThan(AbjadObject other, int line, int index)
        {
            throw new AbjadUnallowedOperatorException("<", Type.ToString(), line, index);
        }

        public virtual AbjadBool OperatorLessEqual(AbjadObject other, int line, int index)
        {
            throw new AbjadUnallowedOperatorException("<=", Type.ToString(), line, index);
        }

        public virtual AbjadBool OperatorGreaterThan(AbjadObject other, int line, int index)
        {
            throw new AbjadUnallowedOperatorException(">", Type.ToString(), line, index);
        }

        public virtual AbjadBool OperatorGreaterEqual(AbjadObject other, int line, int index)
        {
            throw new AbjadUnallowedOperatorException(">=", Type.ToString(), line, index);
        }

        public virtual AbjadBool OperatorEqual(AbjadObject other, int line, int index)
        {
            return new AbjadBool(Value.Equals(other.Value));
        }

        public virtual AbjadBool OperatorAnd(AbjadObject other, int line, int index)
        {
            throw new AbjadUnallowedOperatorException("&&", Type.ToString(), line, index);
        }

        public virtual AbjadBool OperatorOr(AbjadObject other, int line, int index)
        {
            throw new AbjadUnallowedOperatorException("||", Type.ToString(), line, index);
        }

        public virtual AbjadString ToStr(int line, int index)
        {
            return new AbjadString(Value.ToString());
        }

        public virtual AbjadNumber ToNumber(int line, int index)
        {
            return new AbjadNumber(Value);
        }

        public virtual AbjadBool ToBool(int line, int index)
        {
            return new AbjadBool(bool.Parse(Value.ToString()));
        }

        public virtual AbjadString GetType(int line, int index)
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

        public override AbjadObject OperatorPlus(AbjadObject other, int line, int index)
        {
            return new AbjadString(Value as string + other.Value.ToString());
        }

        public override AbjadBool OperatorGreaterEqual(AbjadObject other, int line, int index)
        {
            return new AbjadBool(Value.ToString().CompareTo(other.Value.ToString()) >= 0);
        }

        public override AbjadBool OperatorGreaterThan(AbjadObject other, int line, int index)
        {
            return new AbjadBool(Value.ToString().CompareTo(other.Value.ToString()) > 0);
        }

        public override AbjadBool OperatorLessEqual(AbjadObject other, int line, int index)
        {
            return new AbjadBool(Value.ToString().CompareTo(other.Value.ToString()) <= 0);
        }

        public override AbjadBool OperatorLessThan(AbjadObject other, int line, int index)
        {
            return new AbjadBool(Value.ToString().CompareTo(other.Value.ToString()) < 0);
        }

        public override AbjadBool ToBool(int line, int index)
        {
            if (Value as string == Constants.Boolean.True)
                return new AbjadBool(true);
            else if (Value as string == Constants.Boolean.False)
                return new AbjadBool(false);
            else
                throw new AbjadCastingException(Constants.Types.String, Constants.Types.Bool, line, index);
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

        public override AbjadObject OperatorDiviededBy(AbjadObject other, int line, int index)
        {
            if ((double)other.Value == 0)
            {
                throw new AbjadDivisionByZeroException();
            }

            return new AbjadNumber((double)Value / (double)other.Value);
        }

        public override AbjadObject OperatorMinus(AbjadObject other, int line, int index)
        {
            return new AbjadNumber((double)Value - (double)other.Value);
        }

        public override AbjadObject OperatorPlus(AbjadObject other, int line, int index)
        {
            return new AbjadNumber((double)Value + (double)other.Value);
        }

        public override AbjadObject OperatorTimes(AbjadObject other, int line, int index)
        {
            return new AbjadNumber((double)Value * (double)other.Value);
        }

        public override AbjadObject OperatorModulo(AbjadObject other, int line, int index)
        {
            return new AbjadNumber((double)Value % (double)other.Value);
        }

        public override AbjadNumber OperatorUMinus(int line, int index)
        {
            return new AbjadNumber(-1 * (double)Value);
        }

        public override AbjadBool OperatorGreaterEqual(AbjadObject other, int line, int index)
        {
            return new AbjadBool((double)Value >= (double)other.Value);
        }

        public override AbjadBool OperatorGreaterThan(AbjadObject other, int line, int index)
        {
            return new AbjadBool((double)Value > (double)other.Value);
        }

        public override AbjadBool OperatorLessEqual(AbjadObject other, int line, int index)
        {
            return new AbjadBool((double)Value <= (double)other.Value);
        }

        public override AbjadBool OperatorLessThan(AbjadObject other, int line, int index)
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

        public override AbjadBool OperatorAnd(AbjadObject other, int line, int index)
        {
            return new AbjadBool((bool)Value && (bool)other.Value);
        }

        public override AbjadBool OperatorOr(AbjadObject other, int line, int index)
        {
            return new AbjadBool((bool)Value || (bool)other.Value);
        }

        public override AbjadBool OperatorBang(int line, int index)
        {
            return new AbjadBool(!(bool)Value);
        }

        public override AbjadNumber ToNumber(int line, int index)
        {
            return new AbjadNumber((bool)Value ? 1 : 0);
        }

        public override AbjadString ToStr(int line, int index)
        {
            return new AbjadString((bool)Value ? Constants.Boolean.True : Constants.Boolean.False);
        }
    }
}