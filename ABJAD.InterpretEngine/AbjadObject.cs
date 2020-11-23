using ABJAD.Models;
using ABJAD.Models.Exceptions.InterpretingExceptions;

namespace ABJAD.InterpretEngine
{
    public enum AbjadType
    {
        رقم,        // float
        مقطع,       // string
        حرف,        // char
        منطقي,      // bool
        خاص         // User Defined Type
    }

    public abstract class AbjadObject<T>
    {
        public AbjadType Type { get; set; }

        public T Value { get; set; }

        public virtual AbjadObject<T> OperatorPlus(AbjadObject<T> other)
        {
            throw new AbjadUnallowedOperatorException("+", Type.ToString());
        }

        public virtual AbjadObject<T> OperatorMinus(AbjadObject<T> other)
        {
            throw new AbjadUnallowedOperatorException("-", Type.ToString());
        }

        public virtual AbjadObject<T> OperatorTimes(AbjadObject<T> other)
        {
            throw new AbjadUnallowedOperatorException("*", Type.ToString());
        }

        public virtual AbjadObject<T> OperatorDiviededBy(AbjadObject<T> other)
        {
            throw new AbjadUnallowedOperatorException("/", Type.ToString());
        }

        public virtual AbjadObject<bool> OperatorLessThan(AbjadObject<T> other)
        {
            throw new AbjadUnallowedOperatorException("<", Type.ToString());
        }

        public virtual AbjadObject<bool> OperatorLessEqual(AbjadObject<T> other)
        {
            throw new AbjadUnallowedOperatorException("<=", Type.ToString());
        }

        public virtual AbjadObject<bool> OperatorGreaterThan(AbjadObject<T> other)
        {
            throw new AbjadUnallowedOperatorException(">", Type.ToString());
        }

        public virtual AbjadObject<bool> OperatorGreaterEqual(AbjadObject<T> other)
        {
            throw new AbjadUnallowedOperatorException(">=", Type.ToString());
        }

        public virtual AbjadObject<bool> OperatorEqual(AbjadObject<T> other)
        {
            return new AbjadBool(Value.Equals(other.Value));
        }

        public virtual AbjadObject<bool> OperatorAnd(AbjadObject<T> other)
        {
            throw new AbjadUnallowedOperatorException("&&", Type.ToString());
        }

        public virtual AbjadObject<bool> OperatorOr(AbjadObject<T> other)
        {
            throw new AbjadUnallowedOperatorException("||", Type.ToString());
        }

        public virtual AbjadString ToStr()
        {
            return new AbjadString(Value.ToString());
        }

        public virtual AbjadNumber ToNumber()
        {
            return new AbjadNumber(float.Parse(Value.ToString()));
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

    public class AbjadString : AbjadObject<string>
    {
        public AbjadString(string str)
        {
            Value = str;
        }

        public override AbjadObject<string> OperatorPlus(AbjadObject<string> other)
        {
            return new AbjadString(Value + other.Value);
        }

        public override AbjadBool ToBool()
        {
            if (Value == Constants.Boolean.True)
                return new AbjadBool(true);
            else if (Value == Constants.Boolean.False)
                return new AbjadBool(false);
            else
                throw new AbjadCastingException("string", "bool");
        }
    }

    public class AbjadNumber : AbjadObject<float>
    {
        public AbjadNumber(float number)
        {
            Value = number;
        }

        public override AbjadObject<float> OperatorDiviededBy(AbjadObject<float> other)
        {
            if (other.Value == 0)
            {
                throw new AbjadDivisionByZeroException();
            }

            return new AbjadNumber(Value / other.Value);
        }

        public override AbjadObject<float> OperatorMinus(AbjadObject<float> other)
        {
            return new AbjadNumber(Value - other.Value);
        }

        public override AbjadObject<float> OperatorPlus(AbjadObject<float> other)
        {
            return new AbjadNumber(Value + other.Value);
        }

        public override AbjadObject<float> OperatorTimes(AbjadObject<float> other)
        {
            return new AbjadNumber(Value * other.Value);
        }

        public override AbjadObject<bool> OperatorGreaterEqual(AbjadObject<float> other)
        {
            return new AbjadBool(Value >= other.Value);
        }

        public override AbjadObject<bool> OperatorGreaterThan(AbjadObject<float> other)
        {
            return new AbjadBool(Value > other.Value);
        }

        public override AbjadObject<bool> OperatorLessEqual(AbjadObject<float> other)
        {
            return new AbjadBool(Value <= other.Value);
        }

        public override AbjadObject<bool> OperatorLessThan(AbjadObject<float> other)
        {
            return new AbjadBool(Value < other.Value);
        }
    }

    public class AbjadBool : AbjadObject<bool>
    {
        public AbjadBool(bool boolean)
        {
            Value = boolean;
        }

        public override AbjadObject<bool> OperatorAnd(AbjadObject<bool> other)
        {
            return new AbjadBool(Value && other.Value);
        }

        public override AbjadObject<bool> OperatorOr(AbjadObject<bool> other)
        {
            return new AbjadBool(Value || other.Value);
        }

        public override AbjadNumber ToNumber()
        {
            return new AbjadNumber(Value? 1 : 0);
        }

        public override AbjadString ToStr()
        {
            return new AbjadString(Value? Constants.Boolean.True : Constants.Boolean.False);
        }
    }
}