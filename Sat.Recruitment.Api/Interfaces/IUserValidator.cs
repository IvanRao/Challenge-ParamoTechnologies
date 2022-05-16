namespace Sat.Recruitment.Api.Interfaces
{
    public interface IUserValidator
    {
        string ValidateErrors(string name, string email, string address, string phone);
    }
}
