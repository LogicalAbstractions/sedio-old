using System;
using System.Runtime.Serialization;

namespace Sedio.Core.Runtime.Application.Dependencies
{
    [Serializable]
    public class DependencyException : Exception
    {
        public DependencyException()
        {
        }

        public DependencyException(string message) : base(message)
        {
        }

        public DependencyException(string message, Exception inner) : base(message, inner)
        {
        }

        protected DependencyException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}