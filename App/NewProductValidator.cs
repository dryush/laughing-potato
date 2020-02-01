using MailBank.Filters.Validation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MailBank.App
{
    public class NewProductValidator : BaseValidator<NewProduct>
    {
        private readonly IEnumerable<IDescriptionValidator> descriptionValidators;
        private readonly IEnumerable<INameValidator> nameValidators;

        public NewProductValidator(IEnumerable<IDescriptionValidator> descriptionValidators, IEnumerable<INameValidator> nameValidators)
        {
            this.descriptionValidators = descriptionValidators;
            this.nameValidators = nameValidators;
        }

        protected override async Task<IEnumerable<ValidationResult>> ValidateAsync(NewProduct entity)
        {
            var errors = new List<ValidationResult>();

            foreach (var validator in descriptionValidators)
                if (!await validator.ValidateDescriptionAsync(entity.Description, out var error))
                    errors.Add(new ValidationResult(nameof(NewProduct.Description), error));

            foreach (var validator in nameValidators)
                if (!await validator.ValidateNameAsync(entity.Name, out var error))
                    errors.Add(new ValidationResult(nameof(NewProduct.Name), error));

            return errors;
        }
    }
}
