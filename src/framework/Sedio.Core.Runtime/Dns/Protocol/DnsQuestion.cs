using System.Collections.Generic;
using System.Runtime.InteropServices;
using Sedio.Core.Runtime.Dns.Protocol.Utils;

namespace Sedio.Core.Runtime.Dns.Protocol
{
    public class DnsQuestion : IDnsMessageEntry
    {
        public static IList<DnsQuestion> GetAllFromArray(byte[] message, int offset, int questionCount)
        {
            return GetAllFromArray(message, offset, questionCount, out offset);
        }

        public static IList<DnsQuestion> GetAllFromArray(byte[] message, int offset, int questionCount, out int endOffset)
        {
            IList<DnsQuestion> questions = new List<DnsQuestion>(questionCount);

            for (int i = 0; i < questionCount; i++)
            {
                questions.Add(FromArray(message, offset, out offset));
            }

            endOffset = offset;
            return questions;
        }

        public static DnsQuestion FromArray(byte[] message, int offset)
        {
            return FromArray(message, offset, out offset);
        }

        public static DnsQuestion FromArray(byte[] message, int offset, out int endOffset)
        {
            Domain domain = Domain.FromArray(message, offset, out offset);
            Tail   tail   = Marshalling.Struct.GetStruct<Tail>(message, offset, Tail.SIZE);

            endOffset = offset + Tail.SIZE;

            return new DnsQuestion(domain, tail.Type, tail.Class);
        }

        private Domain      domain;
        private DnsRecordType  type;
        private DnsRecordClass klass;

        public DnsQuestion(Domain domain, DnsRecordType type = DnsRecordType.A, DnsRecordClass klass = DnsRecordClass.IN)
        {
            this.domain = domain;
            this.type = type;
            this.klass = klass;
        }

        public Domain Name
        {
            get { return domain; }
        }

        public DnsRecordType Type
        {
            get { return type; }
        }

        public DnsRecordClass Class
        {
            get { return klass; }
        }

        public int Size
        {
            get { return domain.Size + Tail.SIZE; }
        }

        public byte[] ToArray()
        {
            ByteStream result = new ByteStream(Size);

            result
                .Append(domain.ToArray())
                .Append(Marshalling.Struct.GetBytes(new Tail {Type = Type, Class = Class}));

            return result.ToArray();
        }

        public override string ToString()
        {
            return ObjectStringifier.New(this)
                .Add("Name", "Type", "Class")
                .ToString();
        }

        [Marshalling.Endian(Marshalling.Endianness.Big)]
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        private struct Tail
        {
            public const int SIZE = 4;

            private ushort type;
            private ushort klass;

            public DnsRecordType Type
            {
                get { return (DnsRecordType) type; }
                set { type = (ushort) value; }
            }

            public DnsRecordClass Class
            {
                get { return (DnsRecordClass) klass; }
                set { klass = (ushort) value; }
            }
        }
    }
}