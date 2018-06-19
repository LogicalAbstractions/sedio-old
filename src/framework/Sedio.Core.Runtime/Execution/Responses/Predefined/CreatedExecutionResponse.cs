using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Sedio.Core.Runtime.Execution.Responses.Predefined
{
    public sealed class CreatedExecutionResponse : AbstractExecutionResponse<CreatedExecutionResponse>
    {
        public object Id { get; }
        
        public string ActionName { get; }
        
        public object Model { get; }
        
        public CreatedExecutionResponse(object id = null,string actionName = null,object model = null) 
            : base(null)
        {
            Id = id;
            ActionName = actionName;
            Model = model;
            
            RegisterTransform<Controller,IActionResult>((context, response) =>
            {
                if (Id != null && ActionName != null)
                {
                    return context.Context.CreatedAtAction(ActionName, new {id = Id},Model);
                }

                return context.Context.StatusCode((int) HttpStatusCode.Created);
            } );
        }
    }
}