using Sat.Recruitment.Api.Models;
using System.Collections.Generic;

namespace Sat.Recruitment.Api.Interfaces
{
    public interface IUserService
    {
        Result CreateUser(string name, string email, string address, string phone, string userType, string money);
    }
}
