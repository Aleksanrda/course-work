using System;

namespace ClassLibrary1.Infrastrcture
{
    public class ComanderException : Exception
    {
        public ComanderException() : base()
        {
        }

        public ComanderException(string message) : base(message)
        {
        }

        public ComanderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
