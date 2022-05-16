using Sat.Recruitment.Api.Interfaces;

namespace Sat.Recruitment.Api.Validators
{
    public class UserValidator : IUserValidator
    {
        public UserValidator() { }

        public void ValidateErrors(string name, string email, string address, string phone, ref string errors)
        {
            if (name == null)
                //Validate if Name is null
                errors = "The name is required";
            if (email == null)
                //Validate if Email is null
                errors += " The email is required";
            if (address == null)
                //Validate if Address is null
                errors += " The address is required";
            if (phone == null)
                //Validate if Phone is null
                errors += " The phone is required";
        }
    }
}
