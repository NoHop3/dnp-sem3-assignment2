using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using LoginExampleServer.Models;

namespace LoginExampleServer.Data.Impl
{
    public class AdultData : IAdultService, IUserService
    {
        public IList<Adult> Adults { get; private set; }
        public IList<User> Users { get; private set; }

        private readonly string adultsFile = "adults.json";
        private readonly string usersFile = "users.json";

        public AdultData()
        {
            Adults = File.Exists(adultsFile) ? ReadData<Adult>(adultsFile) : new List<Adult>();
            Users = File.Exists(usersFile) ? ReadData<User>(usersFile) : new List<User>();
        }

        private IList<T> ReadData<T>(string s)
        {
            using (var jsonReader = File.OpenText(s))
            {
                return JsonSerializer.Deserialize<List<T>>(jsonReader.ReadToEnd());
            }
        }

        public void SaveChanges()
        {
            // storing persons
            string jsonAdults = JsonSerializer.Serialize(Adults, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            using (StreamWriter outputFile = new StreamWriter(adultsFile, false))
            {
                outputFile.Write(jsonAdults);
            }

            string jsonUsers = JsonSerializer.Serialize(Users, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            using (StreamWriter outputFile = new StreamWriter(usersFile, false))
            {
                outputFile.Write(jsonUsers);
            }
        }

        public async Task<IList<Adult>> GetAdultsAsync()
        {
            List<Adult> temp = new List<Adult>(Adults).OrderBy(adult => adult.Id).ToList();
            return temp;
        }

        public async Task<IList<User>> GetUsersAsync()
        {
            List<User> temp = new List<User>(Users);
            return temp;
        }

        public async Task<User> ValidateUserAsync(string userName, string password)
        {
            User first = Users.FirstOrDefault(user => user.UserName.Equals(userName) && user.Password.Equals(password));
            if (first == null)
            {
                throw new Exception("User not found");
            }

            if (!first.Password.Equals(password))
            {
                throw new Exception("Incorrect password");
            }

            return first;
        }

        public async Task AddNewUserAsync(User user)
        {
            int max;
            try
            {
                foreach (var user1 in Users)
                {
                    if (user.Id == user1.Id)
                    {
                        max = Users.Max(adult => adult.Id);
                        user.Id = (++max);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Users.Add(user);
            SaveChanges();
        }

        public async Task AddPersonAsync(Adult adult)
        {
            int max;
            try
            {
                foreach (var adult1 in Adults)
                {
                    if (adult.Id == adult1.Id)
                    {
                        max = Adults.Max(adult => adult.Id);
                        adult.Id = (++max);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Adults.Add(adult);
            SaveChanges();
        }

        public async Task RemovePersonAsync(int adultId)
        {
            Adult adultToRemove = Adults.First(adult1 => adult1.Id == adultId);
            Adults.Remove(adultToRemove);
            SaveChanges();
        }

        public async Task<Adult> GetAdultAsync(int id)
        {
            return Adults.FirstOrDefault(adult => adult.Id == id);
        }
    }
}