using System.Collections.Generic;
using System.Threading.Tasks;

namespace MailBank.Filters.Validation
{
    public interface IValidator
    {
        Task<IEnumerable<ValidationResult>> ValidateAsync(object entity);
    }
}
