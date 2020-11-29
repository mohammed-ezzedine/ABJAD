namespace ABJAD.Models
{
    public static class Constants
    {
        public static readonly string AbjadExtension = "abjad"; 

        public class Boolean
        {
            public static readonly string True = "صحيح"; 
            public static readonly string False = "خطا"; 
        }

        public class Types
        {
            public static readonly string String = "مقطع";
            public static readonly string Number = "رقم";
            public static readonly string Bool = "منطقي";
        }

        public class ErrorMessages
        {
            public class Arabic
            {
                public static string ConstantModification = "لا يمكن تغيير قيمة الثوابت";
                public static readonly string Extension = $"ملفات أبجد يجب أن تنتهي أسماؤها بـ .{AbjadExtension}";
                public static readonly string ForLoopVar = "فقط المتغيرات يمكن استعمالها بعد 'كرر'";
                public static readonly string InvalidSyntax = "تركيب جملة غير صالح";
                public static readonly string UnassignedVar = "يجب منح المتغير قيمة عند تعريفه";
                public static readonly string UnassignedConst = "يجب منح الثابت قيمة عند تعريفه";

                public static string Casting(string fromType, string toType)
                    => $"لا يمكن التحويل من نوع ’{fromType}‘ إلى نوع ’{toType}‘";

                public static string Casting(string toType)
                    => $"لا يمكن التحويل إلى نوع ’{toType}‘";

                public static string ExpectedToken(int line, string token) 
                    => $"رمز مفقود: الرمز ’{token}‘ لم يوجد في السطر ’{line}‘";

                public static string ExpectedToken(string token) 
                    => $"رمز مفقود: الرمز ’{token}‘ لم يوجد";

                public static string NameTaken(string name)
                    => $"الإسم {name} مأخوذ";

                public static string OperatorNotAllowed(string oper, string type)
                    => $"علامة الـ {oper} غير مسموحة للنوع {type}";

                public static string UndefinedToken(int line, int index, string token)
                    => $"رمز غير مُعرّف ’{token}‘ في السطر {index} : {line}";

                public static string UnexpectedToken(int line, int index, string token)
                    => $"رمز غير متوقع ’{token}‘ في السطر {index} : {line}";

                public static string UnfoundConstructor(string className)
                    => $"الصنف {className} ليس لديه مُنشئ";

                public static string UnknownClass(string className)
                    => $"الصنف {className} غير معرّف";

                public static string UnknownFunction(string func, int paramCount)
                    => $"دالة باسم {func} و{paramCount} عوامل غير معرّفة";

                public static string UnknownKey(string key)
                    => $"المفتاح {key} غير معرّف";

                public static string UnknownVariable(string name)
                    => $"المتغير {name} غير معرّف";
            }

            public class English
            {
                public static string ConstantModification = "Cannot change the value of a constant";
                public static readonly string Extension = $"ABJAD files should have .{AbjadExtension} extension";
                public static readonly string ForLoopVar = "Variables only can be used in for loops";
                public static readonly string InvalidSyntax = "Invalid syntax";
                public static readonly string UnassignedVar = "A variable shall not be declared without a value";
                public static readonly string UnassignedConst = "A constant shall not be declared without a value";

                public static string Casting(string fromType, string toType)
                    => $"Cannot cast from type {nameof(fromType).ToLower()} to type {nameof(toType).ToLower()}";

                public static string Casting(string toType)
                    => $"Cannot cast to type {nameof(toType).ToLower()}";

                public static string ExpectedToken(int line, string token)
                    => $"Expected token: '{token}' was not found at line: {line}";

                public static string ExpectedToken(string token)
                    => $"Expected token: '{token}' was not found";

                public static string NameTaken(string name)
                    => $"Name {name} already exists in the stack";

                public static string OperatorNotAllowed(string oper, string type)
                    => $"The {oper} operator is not allowed on data of type {type}";

                public static string UndefinedToken(int line, int index, string token)
                    => $"Undefined token '{token}' at line {line}:{index}";

                public static string UnexpectedToken(int line, int index, string token)
                    => $"Unexpected token '{token}' line {line}:{index}";

                public static string UnfoundConstructor(string className)
                    => $"Class {className} has no constructor";

                public static string UnknownClass(string className)
                    => $"Class with name {className} doesn't exits in the scope";

                public static string UnknownFunction(string func, int paramCount)
                    => $"Function with name {func} and {paramCount} parameters doesn't exits in the scope";

                public static string UnknownKey(string key)
                    => $"Key {key} doesn't exits in the scope";

                public static string UnknownVariable(string name)
                    => $"Variable {name} doesn't exits in the scope";
            }
        }
    }
}
