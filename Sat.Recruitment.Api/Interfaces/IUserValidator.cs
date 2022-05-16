namespace Sat.Recruitment.Api.Interfaces
{
    public interface IUserValidator
    {
        void ValidateErrors(string name, string email, string address, string phone, ref string errors);
    }
}
