using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailBank.Filters.Validation
{
    public abstract class BaseValidator<TValidateEntity> : IValidator
    {
        protected abstract Task<IEnumerable<ValidationResult>> ValidateAsync(TValidateEntity entity);

        async Task<IEnumerable<ValidationResult>> IValidator.ValidateAsync(object entity)
        {
            if (entity is TValidateEntity e)
                return await ValidateAsync(e);
            return Enumerable.Empty<ValidationResult>();
        }
    }
}
