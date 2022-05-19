using Sat.Recruitment.Api.Interfaces;
using System.Text.RegularExpressions;

namespace Sat.Recruitment.Api.Validators
{
    public class UserValidator : IUserValidator
    {
        public UserValidator() { }

        public string ValidateErrors(string name, string email, string address, string phone)
        {
            string errors = "";
            string phoneRegex = @"^[\+]?\d+[ ]?\d+$";

            if (string.IsNullOrEmpty(name))
            {
                // Validate if Name is null or empty
                errors = "The name is required.";
            }

            if (string.IsNullOrEmpty(email))
            {
                // Validate if Email is null or empty
                errors += " The email is required.";
            }

            if (string.IsNullOrEmpty(address))
            {
                // Validate if Address is null or empty
                errors += " The address is required.";
            }

            if (string.IsNullOrEmpty(phone))
            {
                // Validate if Phone is null or empty
                errors += " The phone number is required.";
            }
            else if (!Regex.Match(phone, phoneRegex).Success)
            {
                // Validate if the phone has the correct formatting using regex
                errors += " The phone number is incorrect.";
            }

            return errors;
        }
    }
}
