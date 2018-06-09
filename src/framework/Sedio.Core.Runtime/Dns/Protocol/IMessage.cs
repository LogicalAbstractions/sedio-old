using System.Collections.Generic;

namespace Sedio.Core.Runtime.Dns.Protocol
{
    public interface IMessage
    {
        IList<Question> Questions { get; }

        int Size { get; }

        byte[] ToArray();
    }
}