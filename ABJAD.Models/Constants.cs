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
                public static readonly string Extension = $"ملفات أبجد يجب أن تنتهي أسماؤها بـ .{AbjadExtension}";
                
                public static string Casting(string fromType, string toType, int line, int index) 
                    => $"لا يمكن التحويل من نوع ’{fromType}‘ إلى نوع ’{toType}‘ في السطر {index} : {line}";

                public static string Casting(string toType, int line, int index)
                    => $"لا يمكن التحويل إلى نوع ’{toType}‘ في السطر {index} : {line}";

                public static string ConstantModification(int line, int index)
                    => $"لا يمكن تغيير قيمة الثوابت في السطر {index} : {line}";

                public static string ExpectedToken(string token, int line, int index)
                    => $"رمز مفقود: الرمز ’{token}‘ لم يوجد في السطر {index} : {line}";
                
                public static string ForLoopVar(int line, int index)
                    => $"فقط المتغيرات يمكن استعمالها بعد 'كرر' في السطر {index} : {line}";
                
                public static string InvalidSyntax(int line, int index)
                    => $"تركيب جملة غير صالح في السطر {index} : {line}";

                public static string NameTaken(string name, int line, int index)
                    => $"الإسم {name} مأخوذ في السطر {index} : {line}";

                public static string OperatorNotAllowed(string oper, string type, int line, int index)
                    => $"علامة الـ {oper} غير مسموحة للنوع {type}  في السطر {index} : {line}";
                
                public static string UnassignedVar(int line, int index)
                    => $"يجب منح المتغير قيمة عند تعريفه في السطر {index} : {line}";
                
                public static string UnassignedConst(int line, int index)
                    => $"يجب منح الثابت قيمة عند تعريفه في السطر {index} : {line}";

                public static string UndefinedToken(int line, int index, string token)
                    => $"رمز غير مُعرّف ’{token}‘ في السطر {index} : {line}";

                public static string UnexpectedToken(int line, int index, string token)
                    => $"رمز غير متوقع ’{token}‘ في السطر {index} : {line}";

                public static string UnfoundConstructor(string className, int paramsCount)
                    => $"الصنف {className} ليس لديه دالة بانية بحيث تأخد ${paramsCount} مُعطيات";

                public static string UnknownClass(string className, int line, int index)
                    => $"الصنف {className} غير معرّف  في السطر {index} : {line}";

                public static string UnknownFunction(string func, int paramCount, int line, int index)
                    => $"دالة باسم {func} و{paramCount} عوامل غير معرّفة";

                public static string UnknownKey(string key, int line, int index)
                    => $"المفتاح {key} غير معرّف  في السطر {index} : {line}";

                public static string UnknownVariable(string name, int line, int index)
                    => $"المتغير {name} غير معرّف  في السطر {index} : {line}";
            }

            public class English
            {
                public static readonly string Extension = $"ABJAD files should have .{AbjadExtension} extension";

                public static string Casting(string fromType, string toType, int line, int index)
                    => $"Cannot cast from type {nameof(fromType).ToLower()} to type {nameof(toType).ToLower()} at line {line}:{index}";

                public static string Casting(string toType, int line, int index)
                    => $"Cannot cast to type {nameof(toType).ToLower()} at line {line}:{index}";

                public static string ConstantModification(int line, int index) 
                    => $"Cannot change the value of a constant at line {line}:{index}";

                public static string ExpectedToken(string token, int line, int index)
                    => $"Expected token: '{token}' was not found at line: {line} at line {line}:{index}";

                public static string ForLoopVar(int line, int index) 
                    => $"Variables only can be used in for loops at line {line}:{index}";

                public static string InvalidSyntax(int line, int index) 
                    => $"Invalid syntax at line {line}:{index}";

                public static string NameTaken(string name, int line, int index)
                    => $"Name {name} already exists in the stack at line {line}:{index}";

                public static string OperatorNotAllowed(string oper, string type, int line, int index)
                    => $"The {oper} operator is not allowed on data of type {type} at line {line}:{index}";

                public static string UnassignedVar(int line, int index) 
                    => $"A variable shall not be declared without a value at line {line}:{index}";

                public static string UnassignedConst(int line, int index) 
                    => $"A constant shall not be declared without a value at line {line}:{index}";

                public static string UndefinedToken(int line, int index, string token)
                    => $"Undefined token '{token}' at line {line}:{index}";

                public static string UnexpectedToken(int line, int index, string token)
                    => $"Unexpected token '{token}' line {line}:{index}";

                public static string UnfoundConstructor(string className, int paramsCount)
                    => $"Class {className} has no constructor that takes ${paramsCount} parameters.";

                public static string UnknownClass(string className, int line, int index)
                    => $"Class with name {className} doesn't exits in the scope at line {line}:{index}";

                public static string UnknownFunction(string func, int paramCount, int line, int index)
                    => $"Function with name {func} and {paramCount} parameters doesn't exits in the scope at line {line}:{index}";

                public static string UnknownKey(string key, int line, int index)
                    => $"Key {key} doesn't exits in the scope at line {line}:{index}";

                public static string UnknownVariable(string name, int line, int index)
                    => $"Variable {name} doesn't exits in the scope at line {line}:{index}";
            }
        }
    }
}
