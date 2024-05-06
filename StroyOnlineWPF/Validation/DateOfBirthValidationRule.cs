using System;
using System.Globalization;
using System.Windows.Controls;

namespace StroyOnlineWPF
{
    public class DateOfBirthValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is DateTime selectedDate)
            {
                if (selectedDate.Date > DateTime.Now.Date)
                {
                    return new ValidationResult(false, "Дата рождения не может быть в будущем.");
                }
            }
            return ValidationResult.ValidResult;
        }
    }
}
