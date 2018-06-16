namespace Sedio.Core.Runtime.Dns.Protocol
{
    public enum DnsResponseCode
    {
        NoError = 0,
        FormatError,
        ServerFailure,
        NameError,
        NotImplemented,
        Refused,
        YXDomain,
        YXRRSet,
        NXRRSet,
        NotAuth,
        NotZone,
    }
}