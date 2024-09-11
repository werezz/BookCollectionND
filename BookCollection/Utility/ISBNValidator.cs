using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BookCollection.Utility
{
    public class ISBNValidator: ValidationAttribute
    {
        public ISBNValidator()
        {
            ErrorMessage = "The ISBN is in bad format.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(!CheckISBNFormat((string)value)) return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }

        private bool CheckISBNFormat(string? input)
        {
            if (CheckISBNLenght(input) && CheckForLetters(input)) return true;
            return false;
        }

        private bool CheckISBNLenght(string? input)
        {
            if (string.IsNullOrEmpty(input)) return false;

            var ISBNStripped = Regex.Replace(input, "[^0-9]", "");

            if (ISBNStripped.Length == 13) return true;

            return false;
        }

        private bool CheckForLetters(string? input)
        {
            if (string.IsNullOrEmpty(input)) return false;

            if (input.ToCharArray().All(c => char.IsLetter(c)) == false) return true;
            return false;
        }
    }
}
