using Sat.Recruitment.Api.Interfaces;

namespace Sat.Recruitment.Api.Validators
{
    public class UserValidator : IUserValidator
    {
        public UserValidator() { }

        public string ValidateErrors(string name, string email, string address, string phone)
        {
            string errors = "";
            if (string.IsNullOrEmpty(name))
                //Validate if Name is null
                errors = "The name is required";
            if (string.IsNullOrEmpty(email))
                //Validate if Email is null
                errors += " The email is required";
            if (string.IsNullOrEmpty(address))
                //Validate if Address is null
                errors += " The address is required";
            if (string.IsNullOrEmpty(phone))
                //Validate if Phone is null
                errors += " The phone is required";

            return errors;
        }
    }
}
