using HomeProject.Models.Response;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;

namespace HomeProject.ServiceExtensions
{
    public class ValidationFilterAttribute : ActionFilterAttribute
    {
        public ValidationFilterAttribute()
        {

        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = new Dictionary<string, string>();

                foreach (var modelStateKey in context.ModelState.Keys)
                {
                    var modelStateVal = context.ModelState[modelStateKey];
                    if (modelStateVal != null)
                    {
                        foreach (var error in modelStateVal.Errors)
                        {
                            errors.Add(char.ToLowerInvariant(modelStateKey[0]) + modelStateKey.Substring(1), error.ErrorMessage);
                        }
                    }
                }
            }
            await next();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            // No implementation needed, but the override keyword resolves CS0114.
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = new Dictionary<string, string>();

                IEnumerable<ModelError> allErrors = context.ModelState.Values.SelectMany(v => v.Errors);

                foreach (var modelStateKey in context.ModelState.Keys)
                {
                    var modelStateVal = context.ModelState[modelStateKey];
                    if (modelStateVal != null)
                    {
                        foreach (var error in modelStateVal.Errors)
                        {
                            errors.Add(char.ToLowerInvariant(modelStateKey[0]) + modelStateKey.Substring(1), error.ErrorMessage);
                        }
                    }
                }

                context.Result = new BadRequestObjectResult(new ResponseModel<object>
                {
                    Data = errors,
                    Message = "One or more model validation error found.",
                    Status = false
                });
            }
        }
    }
}
