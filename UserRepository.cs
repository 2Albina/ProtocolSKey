using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Security.Cryptography;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ProtocolSKey
{
    public class UserRepository
    {
        private readonly string _pathToFile = "users.txt";
        public UserRepository()
        {
            if (!File.Exists(_pathToFile)) File.Create(_pathToFile);
        }
        public string Hash(string password, int number)
        {
            SHA256 AlgorithmHash = SHA256.Create();

            var passwordBytes = Encoding.UTF32.GetBytes(password);
            var numberBytes = Encoding.UTF32.GetBytes(number.ToString());
            var bytes = passwordBytes.Concat(numberBytes).ToArray();
            var hash = AlgorithmHash.ComputeHash(bytes);

            string output = Convert.ToBase64String(hash);
            return output;
        }

        public string HashCount(string password, int number, int count)
        {
            for (int i = 0; i < count; i++)
            {
                password = Hash(password, number);
            }
            return password;
        }
        public void AppendUser(User user)
        {
            using (var sw = GetStreamWriter(_pathToFile))
            {
                var json = user.ToJson();
                sw.WriteLine(json);
            }
        }

        public bool HasUser(string userName)
        {
            using (var sr = GetStreamReader(_pathToFile))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    var user = User.FromJson(line);
                    if (user.Name == userName) return true;
                }
            }

            return false;
        }

        public void Create(User user)
        {
            if (HasUser(user.Name))
            {
                MessageBox.Show("Пользователь с именем \"" + user.Name + "\"  уже существует.", "Ошибка входа", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else AppendUser(user);
        }

        public User GetUser(string userName)
        {
            using (var sr = GetStreamReader(_pathToFile))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    var user = User.FromJson(line);
                    if (user.Name == userName) return user;
                }
            }
            MessageBox.Show("Пользователя с именем \"" + userName + "\"  не существует.", "Ошибка входа", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }

        public List<User> GetAllUsers()
        {
            var users = new List<User>();

            using (var sr = new StreamReader(_pathToFile))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    var user = User.FromJson(line);
                    users.Add(user);
                }
            }

            return users;
        }

        public void Update(User newUser)
        {
            var tempFile = Path.GetTempFileName();
            using (var sr = GetStreamReader(_pathToFile))
            using (var sw = GetStreamWriter(tempFile))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    var user = User.FromJson(line);
                    if (user.Name != newUser.Name)
                        sw.WriteLine(line);
                    else
                        sw.WriteLine(newUser.ToJson());
                }
            }

            File.Delete(_pathToFile);
            File.Move(tempFile, _pathToFile);
        }

        public void Delete(string userName)
        {
            var tempFile = Path.GetTempFileName();
            using (var sr = GetStreamReader(_pathToFile))
            using (var sw = GetStreamWriter(tempFile))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    var user = User.FromJson(line);
                    if (user.Name != userName) sw.WriteLine(line);
                }
            }

            File.Delete(_pathToFile);
            File.Move(tempFile, _pathToFile);
        }

        private static StreamWriter GetStreamWriter(string path)
        {
            return new StreamWriter(path, File.Exists(path), Encoding.UTF8);
        }

        private static StreamReader GetStreamReader(string path)
        {
            return new StreamReader(path, Encoding.UTF8);
        }
    }
}
