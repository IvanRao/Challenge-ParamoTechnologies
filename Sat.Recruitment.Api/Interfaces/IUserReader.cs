using System.IO;

namespace Sat.Recruitment.Api.Interfaces
{
    public interface IUserReader
    {
        StreamReader ReadUsersFromFile();
    }
}
