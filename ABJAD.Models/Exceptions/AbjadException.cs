using System;

namespace ABJAD.Models.Exceptions
{
    public class AbjadException : Exception
    {
        public AbjadException(string msg_en, string msg_ar)
            : base(msg_en)
        {
            EnglishMessage = msg_en;
            ArabicMessage = msg_ar;
        }

        public string EnglishMessage { get; set; }

        public string ArabicMessage { get; set; }
    }
}
