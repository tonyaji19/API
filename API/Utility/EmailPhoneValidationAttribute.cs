using API.Contracts;
using System.ComponentModel.DataAnnotations;

namespace API.Utility
{
    public class EmailPhoneValidationAttribute : ValidationAttribute
    {
/*        private readonly string _guidProperty;
*/        private readonly string _propertyName;

        public EmailPhoneValidationAttribute( string propertyName)
        {
/*            _guidProperty = guidProperty;
*/            _propertyName = propertyName;   
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return new ValidationResult($"{_propertyName} is required.");
            var employeeRepository = validationContext.GetService(typeof(IEmployeeRepository))
                                        as IEmployeeRepository;

            bool checkEmailAndPhone = employeeRepository.CheckEmailAndPhoneAndNIK(value.ToString());
            if (checkEmailAndPhone) return new ValidationResult($"{_propertyName} '{value}' already exists.");
            return ValidationResult.Success;
        }
    }
}
