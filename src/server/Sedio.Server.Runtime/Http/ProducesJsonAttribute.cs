using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace Sedio.Server.Runtime.Http
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ProducesJsonAttribute : ProducesAttribute
    {
        public ProducesJsonAttribute(Type type) : base(type)
        {
            this.ContentTypes = new MediaTypeCollection() {new MediaTypeHeaderValue("application/json")};
        }

        public ProducesJsonAttribute() : base("application/json") { }
    }
}