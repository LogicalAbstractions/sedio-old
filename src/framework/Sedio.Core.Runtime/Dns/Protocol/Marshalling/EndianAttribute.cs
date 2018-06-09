using System;

namespace Sedio.Core.Runtime.Dns.Protocol.Marshalling
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Struct)]
    public class EndianAttribute : Attribute
    {
        public EndianAttribute(Endianness endianness)
        {
            this.Endianness = endianness;
        }

        public Endianness Endianness { get; private set; }
    }
}