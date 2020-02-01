using System.Collections.Generic;
using System.Threading.Tasks;

namespace mail_bank.Filters.Validation
{
    public interface IValidator
    {
        Task<IEnumerable<ValidationResult>> ValidateAsync(object entity);
    }
}
