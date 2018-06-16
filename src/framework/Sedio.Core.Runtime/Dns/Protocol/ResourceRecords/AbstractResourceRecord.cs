using System;
using Sedio.Core.Runtime.Dns.Protocol.Utils;

namespace Sedio.Core.Runtime.Dns.Protocol.ResourceRecords
{
    public abstract class AbstractResourceRecord : IResourceRecord
    {
        private IResourceRecord record;

        public AbstractResourceRecord(IResourceRecord record)
        {
            this.record = record;
        }

        public Domain Name
        {
            get { return record.Name; }
        }

        public DnsRecordType Type
        {
            get { return record.Type; }
        }

        public DnsRecordClass Class
        {
            get { return record.Class; }
        }

        public TimeSpan TimeToLive
        {
            get { return record.TimeToLive; }
        }

        public int DataLength
        {
            get { return record.DataLength; }
        }

        public byte[] Data
        {
            get { return record.Data; }
        }

        public int Size
        {
            get { return record.Size; }
        }

        public byte[] ToArray()
        {
            return record.ToArray();
        }

        internal ObjectStringifier Stringify()
        {
            return ObjectStringifier.New(this)
                .Add("Name", "Type", "Class", "TimeToLive", "DataLength");
        }
    }
}