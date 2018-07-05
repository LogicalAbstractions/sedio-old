using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Sedio.Core.Runtime.Execution.Responses.Predefined
{
    public sealed class CreatedExecutionResponse : AbstractExecutionResponse<CreatedExecutionResponse>
    {
        public object Id { get; }
        
        public string IdName { get; set; }
        
        public string ActionName { get; }
        
        public object Model { get; set; }
        
        public CreatedExecutionResponse(object id,string actionName) 
            : base(null)
        {
            Id = id;
            ActionName = actionName;

            RegisterTransform<Controller,IActionResult>((context, response) =>
            {
                if (Id != null && ActionName != null)
                {
                    var routeValues = new RouteValueDictionary()
                    {
                        {IdName ?? "id", Id}
                    };
                    
                    return context.Context.CreatedAtAction(ActionName,routeValues,Model);
                }

                return context.Context.StatusCode((int) HttpStatusCode.Created);
            } );
        }
    }
}