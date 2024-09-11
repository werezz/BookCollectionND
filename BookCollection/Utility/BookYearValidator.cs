using System.ComponentModel.DataAnnotations;

namespace BookCollection.Utility
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class BookYearValidator: ValidationAttribute
    {
        public BookYearValidator()
        {
            ErrorMessage = "The publication year cannot be in the future.";
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime localDate = DateTime.Now;
            if ((int?)value >= localDate.Year) return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
