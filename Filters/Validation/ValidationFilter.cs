using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailBank.Filters.Validation
{
    public class ValidationFilter : IAsyncActionFilter
    {

        private IEnumerable<IValidator> Validators;

        public ValidationFilter(IEnumerable<IValidator> validators) => Validators = validators;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var errors = new List<ValidationResult>();
            if (this.Validators != null && Validators.Any())
                foreach (var val in context.ActionArguments.Values)
                    foreach (var validator in Validators)
                        errors.AddRange(await validator.ValidateAsync(val));

            foreach (var error in errors)
                context.ModelState.AddModelError(error.Field, error.Description);

            await next();
        }
    }
}
