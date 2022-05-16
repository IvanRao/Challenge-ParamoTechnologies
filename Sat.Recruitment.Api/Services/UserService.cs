using Sat.Recruitment.Api.Interfaces;
using Sat.Recruitment.Api.Models;
using Sat.Recruitment.Api.Utilities;
using Sat.Recruitment.Api.Validators;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sat.Recruitment.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserValidator _userValidator;
        private readonly IUserReader _userReader;

        public UserService(IUserValidator userValidator, IUserReader userReader)
        {
            _userValidator = userValidator;
            _userReader = userReader;
        }


        public Result CreateUser(string name, string email, string address, string phone, string userType, string money)
        {
            List<User> _users = GetUsers();

            var errors = _userValidator.ValidateErrors(name, email, address, phone);

            if (errors != null && errors != "")
                return new Result()
                {
                    IsSuccess = false,
                    Errors = errors
                };

            var newUser = new User
            {
                Name = name,
                Email = NormalizeEmail(email),
                Address = address,
                Phone = phone,
                UserType = userType,
                Money = CalculateMoney(userType, decimal.Parse(money))
            };

            try
            {
                var isDuplicated = false;

                foreach (var user in _users)
                {
                    if (user.Email == newUser.Email || user.Phone == newUser.Phone)
                    {
                        isDuplicated = true;
                        break;
                    }
                    else if (user.Name == newUser.Name)
                    {
                        if (user.Address == newUser.Address)
                        {
                            isDuplicated = true;
                            break;
                        }
                    }
                }

                if (!isDuplicated)
                {
                    Debug.WriteLine("User Created");

                    return new Result()
                    {
                        IsSuccess = true,
                        Errors = "User Created"
                    };
                }
                else
                {
                    Debug.WriteLine("The user is duplicated");

                    return new Result()
                    {
                        IsSuccess = false,
                        Errors = "The user is duplicated"
                    };
                }
            }
            catch
            {
                Debug.WriteLine("The user is duplicated");

                return new Result()
                {
                    IsSuccess = false,
                    Errors = "The user is duplicated"
                };
            }
        }

        private string NormalizeEmail(string email)
        {
            var aux = email.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);

            var atIndex = aux[0].IndexOf("+", StringComparison.Ordinal);

            aux[0] = atIndex < 0 ? aux[0].Replace(".", "") : aux[0].Replace(".", "").Remove(atIndex);

            return string.Join("@", new string[] { aux[0], aux[1] });
        }

        private decimal CalculateMoney(string type, decimal money)
        {
            decimal gif = 0;

            if (type == "Normal")
            {
                if (money > 100)
                {
                    var percentage = Convert.ToDecimal(0.12);
                    //If new user is normal and has more than USD100
                    gif = money * percentage;
                }

                if (money <= 100)
                {
                    if (money > 10)
                    {
                        var percentage = Convert.ToDecimal(0.8);
                        gif = money * percentage;
                    }
                }
            }

            if (type == "SuperUser")
            {
                if (money > 100)
                {
                    var percentage = Convert.ToDecimal(0.20);
                    gif = money * percentage;
                }
            }

            if (type == "Premium")
            {
                if (money > 100)
                {
                    gif = money * 2;
                }
            }

            return money + gif;
        }

        private List<User> GetUsers()
        {
            var reader = _userReader.ReadUsersFromFile();

            List<User> users = new List<User>();

            while (reader.Peek() >= 0)
            {
                var line = reader.ReadLineAsync().Result;
                var user = new User
                {
                    Name = line.Split(',')[0].ToString(),
                    Email = line.Split(',')[1].ToString(),
                    Phone = line.Split(',')[2].ToString(),
                    Address = line.Split(',')[3].ToString(),
                    UserType = line.Split(',')[4].ToString(),
                    Money = decimal.Parse(line.Split(',')[5].ToString()),
                };
                users.Add(user);
            }

            reader.Close();

            return users;
        }
    }
}
