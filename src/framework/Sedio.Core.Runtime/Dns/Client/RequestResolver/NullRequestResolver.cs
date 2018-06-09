﻿using System.Threading.Tasks;
using Sedio.Core.Runtime.Dns.Protocol;

namespace Sedio.Core.Runtime.Dns.Client.RequestResolver
{
    public class NullRequestResolver : IRequestResolver
    {
        public Task<IResponse> Resolve(IRequest request)
        {
            throw new ResponseException("Request failed");
        }
    }
}