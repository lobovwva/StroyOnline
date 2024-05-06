using System.Globalization;
using System.Windows.Controls;

namespace StroyOnlineWPF
{
    public class NumericValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return ValidationResult.ValidResult;

            int result;

            if (!int.TryParse(value.ToString(), out result))
            {
                return new ValidationResult(false, "Пожалуйста, введите только цифры.");
            }
            return ValidationResult.ValidResult;
        }
    }
}
