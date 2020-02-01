using System.Threading.Tasks;

namespace mail_bank.App
{
    public interface INameValidator
    {
        Task<bool> ValidateNameAsync(string name, out string error);
    }

    public class NameLengthValidator : INameValidator
    {
        public Task<bool> ValidateNameAsync(string name, out string error)
        {
            if (string.IsNullOrEmpty(name) || name.Length > 100)
            {
                error = "Продукт должен иметь Имя. Описание должно быть не длинее 100 символов";
                return Task.FromResult(false);
            }
            error = null;
            return Task.FromResult(true);
        }
    }
}
