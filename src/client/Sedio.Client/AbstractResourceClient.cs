using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace Sedio.Client
{
    public abstract class AbstractResourceClient<TRequestBody,TResponseBody>
    {
        private readonly HttpClient httpClient;
        private readonly JsonSerializer serializer;

        protected AbstractResourceClient(HttpClient httpClient, JsonSerializer serializer)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }
    }
}