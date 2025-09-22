using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using UIApplication.Models;

namespace UIApplication.DataManagers
{
    public static class UserManager
    {
        private static string usersFileName = "users.json";
        private static List<User> _users;

        static UserManager()
        {
            LoadUsers();
        }

        private static void LoadUsers()
        {
            if (File.Exists(usersFileName))
            {
                string jsonString = File.ReadAllText(usersFileName);
                _users = JsonSerializer.Deserialize<List<User>>(jsonString);
                if (_users == null)
                {
                    _users = new List<User>();
                }
            }
            else
            {
                _users = new List<User>();
                //Добавляем модератора по умолчанию
                _users.Add(new User { Login = "Ekaterine", Password = "studentokei", IsModerator = true });
                SaveUsers();
            }
        }

        private static void SaveUsers()
        {
            string jsonString = JsonSerializer.Serialize(_users);
            File.WriteAllText(usersFileName, jsonString);
        }

        public static bool RegisterUser(string login, string password)
        {
            bool userExists = false;
            foreach (var user in _users)
            {
                if (user.Login == login)
                {
                    userExists = true;
                    break;
                }
            }

            if (userExists)
            {
                return false; //Пользователь уже существует
            }

            _users.Add(new User { Login = login, Password = password, IsModerator = false });
            SaveUsers();
            return true;
        }

        public static User AuthenticateUser(string login, string password)
        {
            foreach (var user in _users)
            {
                if (user.Login == login && user.Password == password)
                {
                    return user;
                }
            }
            return null;
        }
    }
}
