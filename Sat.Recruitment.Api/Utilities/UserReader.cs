using Sat.Recruitment.Api.Interfaces;
using System.IO;

namespace Sat.Recruitment.Api.Utilities
{
    public class UserReader : IUserReader
    {
        public UserReader() { }

        public StreamReader ReadUsersFromFile()
        {
            var path = Directory.GetCurrentDirectory() + "/Files/Users.txt";

            FileStream fileStream = new FileStream(path, FileMode.Open);

            StreamReader reader = new StreamReader(fileStream);
            return reader;
        }
    }
}
