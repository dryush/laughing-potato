namespace MailBank.Filters.Validation
{
    public class ValidationResult
    {
        public readonly string Field;
        public readonly string Description;

        public ValidationResult(string description) {
            Description = description;
            Field = "";

        }
        public ValidationResult(string field, string description)
        {
            Field = field;
            Description = description;
        }


    }
}
