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
            // Get the existing users
            List<User> _users = GetUsers();
            
            // Check for errors
            var errors = _userValidator.ValidateErrors(name, email, address, phone);

            if (errors != null && errors != "")
                return new Result()
                {
                    IsSuccess = false,
                    Errors = errors.Trim()
                };

            // Create the new user
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
                // Check is the data of the new user match with the data of an existing user
                var userExists = _users.Find(x => x.Email == newUser.Email || x.Phone == newUser.Phone || (x.Name == newUser.Name && x.Address == newUser.Address));

                if (userExists == null)
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
                Debug.WriteLine("Unknown error");

                return new Result()
                {
                    IsSuccess = false,
                    Errors = "Unknown error"
                };
            }
        }

        /// <summary>
        /// Normalize the email of the new user
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private string NormalizeEmail(string email)
        {
            var aux = email.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);

            var atIndex = aux[0].IndexOf("+", StringComparison.Ordinal);

            aux[0] = atIndex < 0 ? aux[0].Replace(".", "") : aux[0].Replace(".", "").Remove(atIndex);

            return string.Join("@", new string[] { aux[0], aux[1] });
        }

        /// <summary>
        /// Calculate the percentage of interest depending the user type and adds it to the original amount of the user
        /// </summary>
        /// <param name="type"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        private decimal CalculateMoney(string type, decimal money)
        {
            decimal percentage = 0;

            if (type == UserTypes.NORMAL_USER)
            {
                if (money > 100)
                {
                    // If new user is normal and has more than USD100
                    percentage = Convert.ToDecimal(0.12);
                }
                else if (money <= 100 && money > 10)
                {
                    // If new user is normal and has less than USD10
                    percentage = Convert.ToDecimal(0.8);
                }
            }
            else if (type == UserTypes.SUPER_USER)
            {
                if (money > 100)
                {
                    // If new user is super and has more than USD100
                    percentage = Convert.ToDecimal(0.20);
                }
            }
            else if (type == UserTypes.PREMIUM_USER)
            {
                if (money > 100)
                {
                    // If new user is premium and has more than USD100
                    percentage = 2;
                }
            }

            // Returns the original amount plus the extra assigned to each user type
            return money + (money * percentage);
        }

        /// <summary>
        /// Reads the file Users.txt and returns a list of users
        /// </summary>
        /// <returns></returns>
        private List<User> GetUsers()
        {
            List<User> users = new List<User>();

            var reader = _userReader.ReadUsersFromFile();

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
