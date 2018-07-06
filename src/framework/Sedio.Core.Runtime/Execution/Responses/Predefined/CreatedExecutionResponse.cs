using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Sedio.Core.Runtime.Execution.Responses.Predefined
{
    public sealed class CreatedExecutionResponse : AbstractExecutionResponse<CreatedExecutionResponse>
    {
        public object RouteValues { get; }
        
        public string ActionName { get; }
        
        public object Model { get; set; }
        
        public CreatedExecutionResponse(object routeValues,string actionName) 
            : base(null)
        {
            RouteValues = routeValues;
            ActionName = actionName;

            RegisterTransform<Controller,IActionResult>((context, response) =>
            {
                if (RouteValues != null && ActionName != null)
                {
                    return context.Context.CreatedAtAction(ActionName,routeValues,Model);
                }

                return context.Context.StatusCode((int) HttpStatusCode.Created);
            } );
        }
    }
}