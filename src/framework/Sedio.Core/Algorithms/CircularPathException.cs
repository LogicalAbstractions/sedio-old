using System;
using System.Runtime.Serialization;

namespace Sedio.Core.Algorithms
{
    [Serializable]
    public class CircularPathException : Exception
    {
        public CircularPathException() : this("Circular path detected")
        {
        }

        public CircularPathException(string message) : base(message)
        {
        }

        public CircularPathException(string message, Exception inner) : base(message, inner)
        {
        }

        protected CircularPathException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}