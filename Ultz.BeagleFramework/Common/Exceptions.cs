using System;

namespace Ultz.BeagleFramework.Common
{
    public class DataException : Exception
    {
        public DataException(string message) : base(message)
        {
        }
    }
    public class InvalidModelException : Exception
    {
        public InvalidModelException() : base("The class provided is not a model.")
        {
        }
        public InvalidModelException(string msg) : base(msg)
        {
        }
    }
}