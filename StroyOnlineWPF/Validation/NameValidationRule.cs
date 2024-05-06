using System.Globalization;
using System.Windows.Controls;

namespace StroyOnlineWPF
{
    public class NameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string inputValue = value as string;

            if (string.IsNullOrWhiteSpace(inputValue))
            {
                return ValidationResult.ValidResult; 
            }

            foreach (char c in inputValue)
            {
                if (!char.IsLetter(c))
                {
                    return new ValidationResult(false, "Пожалуйста, введите только буквы.");
                }
            }
            return ValidationResult.ValidResult;
        }
    }
}
