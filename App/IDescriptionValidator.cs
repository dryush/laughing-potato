using System.Threading.Tasks;

namespace MailBank.App
{
    public interface IDescriptionValidator
    {
        Task<bool> ValidateDescriptionAsync(string description, out string error);
    }

    public class DescriptionLenghtValidtor : IDescriptionValidator
    {
        public Task<bool> ValidateDescriptionAsync(string description, out string error)
        {
            if( string.IsNullOrEmpty(description) || description.Length > 500)
            {
                error = "Продукт должен иметь описание. Описание должно быть не длинее 500 символов";
                return Task.FromResult(false);
            }
            error = null;
            return Task.FromResult(true);
        }
    }
}
