namespace Sedio.Core.Runtime.Dns.Protocol
{
    public enum DnsOperationCode
    {
        Query = 0,
        IQuery,
        Status,

        // Reserved = 3
        Notify = 4,
        Update,
    }
}