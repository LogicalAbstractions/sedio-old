using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Sedio.Server.Runtime.Execution.Commands;

namespace Sedio.Server.Runtime.Api.Http
{
    public static class StatusCodeExtensions
    {
        private static readonly Dictionary<CreationResultType,HttpStatusCode> statusCodes = new Dictionary<CreationResultType, HttpStatusCode>()
        {
            {CreationResultType.Conflict,HttpStatusCode.Conflict},
            {CreationResultType.Created,HttpStatusCode.Created},
            {CreationResultType.Updated,HttpStatusCode.Accepted},
            {CreationResultType.ValidationFailed,HttpStatusCode.BadRequest}
        };

        public static IActionResult ToHttpStatusResult(this CreationResultType resultType)
        {
            return new StatusCodeResult((int)ToHttpStatusCode(resultType));
        }

        public static HttpStatusCode ToHttpStatusCode(this CreationResultType resultType)
        {
            return statusCodes[resultType];
        }
    }
}