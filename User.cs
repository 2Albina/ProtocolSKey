using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProtocolSKey
{
    public class User
    {
        public readonly string Name; 
        public readonly int Number; // N - случайное число (примесь)
        public readonly int Count; // M - количество неиспользованных одноразовых паролей
        public readonly string OneTimePassword; // Y_(m+1) - одноразовый пароль 
        public readonly string Password; // P - пароль 
        public readonly int TotalCount; // Общее количество одноразовых паролей
        public User(string name, int number, int count, string oneTimePassword, string password, int totalCount)
        {
            Name = name;
            Number = number;
            Count = count;
            OneTimePassword = oneTimePassword;
            Password = password;
            TotalCount = totalCount;
        }
        
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static User FromJson(string json)
        {
            var user = JsonConvert.DeserializeObject<User>(json);
            return user;
        }
    }
}
