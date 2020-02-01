using MailBank.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static MailBank.Filters.StatusToBody.BodyStatusAnswer;


namespace MailBank.Filters.StatusToBody
{
    public class HttpStatusToBodyFormatFilter :
        IAsyncActionFilter,
        IAsyncResourceFilter,
        IAsyncResultFilter,
        IAsyncExceptionFilter,
        IAsyncAuthorizationFilter
    {
        public AuthorizationPolicy Policy;

        public HttpStatusToBodyFormatFilter(AuthorizationPolicy policy) => Policy = policy;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            if( !resultContext.ModelState.IsValid)
            {
                resultContext.Result = new OkObjectResult(
                    Empty(
                        (int?)HttpStatusCode.BadRequest,
                        string.Join("\n", resultContext.ModelState.Values.SelectMany(s => s.Errors))
                    ));
            }

            resultContext.Result =
                resultContext.Result switch
                {
                    JsonResult jsonResult => jsonResult.Value switch
                    {
                        BodyStatusAnswer bodyStatus => resultContext.Result,
                        _ => new JsonResult(
                            new BodyStatusAnswer(jsonResult.StatusCode, jsonResult.Value)
                        )
                    },
                    ObjectResult objectResult => objectResult.Value switch
                    {
                        BodyStatusAnswer bodyStatus => resultContext.Result,
                        _ => new OkObjectResult(
                            new BodyStatusAnswer(objectResult.StatusCode, objectResult.Value)
                        )
                    },
                    StatusCodeResult statusCodeResult => new OkObjectResult(
                        Empty(statusCodeResult.StatusCode)
                    ),
                    // ContentResult 
                    // PartialView 
                    // ViewComponentResult
                    // ViewResult
                    _ => resultContext.Result
                };
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {

            // Allow Anonymous skips all authorization
            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
            {
                return;
            }

            var policyEvaluator = context.HttpContext.RequestServices.GetRequiredService<IPolicyEvaluator>();
            var authenticateResult = await policyEvaluator.AuthenticateAsync(Policy, context.HttpContext);
            var authorizeResult = await policyEvaluator.AuthorizeAsync(Policy, authenticateResult, context.HttpContext, context);

            if (authorizeResult.Challenged)
                context.Result = new OkObjectResult(Empty(401));
            else if (authorizeResult.Forbidden)
                context.Result = new OkObjectResult(Empty(403));
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            if (!context.ExceptionHandled)
            {
                context.Result = new OkObjectResult(
                    new BodyStatusAnswer(
                        500,
                        null,
                        context.Exception.Message
                    ));
            }
        }


        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            var resultContext = await next();

        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new OkObjectResult(Empty(
                    (int?)HttpStatusCode.BadRequest,
                    string.Join("\n\n", context.ModelState.Select(s =>
                        s.Key + ": " + string.Join("\n \t", s.Value.Errors.Select(e => e.ErrorMessage))
                    ))
                ));

                context.ModelState.Clear();
            }
            context.ModelState.Clear();
            var resultContext = await next();
        }
    }
}
