using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LoginExample.Models;

namespace LoginExample.Data.Impl
{
    public class WebService : IAdultService, IUserService
    {
        public async Task<IList<Adult>> GetAdultsAsync()
        {
            using HttpClient client = new HttpClient();
            HttpResponseMessage responseMessage = await client.GetAsync("https://localhost:5003/Adults");

            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception($@"Error: {responseMessage.StatusCode}, {responseMessage.ReasonPhrase}");
            string result = await responseMessage.Content.ReadAsStringAsync();

            List<Adult> adults = JsonSerializer.Deserialize<List<Adult>>(result, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            return adults;
        }

        public async Task<IList<User>> GetUsersAsync()
        {
            using HttpClient client = new HttpClient();
            HttpResponseMessage responseMessage = await client.GetAsync("https://localhost:5003/Users");

            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception($@"Error: {responseMessage.StatusCode}, {responseMessage.ReasonPhrase}");
            string result = await responseMessage.Content.ReadAsStringAsync();

            List<User> users = JsonSerializer.Deserialize<List<User>>(result, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            return users;
        }

        public async Task AddNewUserAsync(User user)
        {
            try
            {
                using HttpClient client = new HttpClient();
                string userToAdd = JsonSerializer.Serialize(user);
                StringContent content = new StringContent(userToAdd, Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = await client.PostAsync("https://localhost:5003/Users", content);
                if (!responseMessage.IsSuccessStatusCode)
                    throw new Exception($@"Error: {responseMessage.StatusCode}, {responseMessage.ReasonPhrase}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task<User> ValidateUserAsync(string userName, string password)
        {
            User? userToValidate = new User();
            try
            {
                using HttpClient client = new HttpClient();
                IList<User> users = await GetUsersAsync();
                userToValidate = users.FirstOrDefault(user1 => user1.UserName.Equals(userName) && user1.Password.Equals(password));
                if (userToValidate != null && userToValidate.Password.Equals(password))
                {
                    string userToValidateJson = JsonSerializer.Serialize(userToValidate);
                    StringContent content = new StringContent
                    (
                        userToValidateJson,
                        Encoding.UTF8,
                        "application/json"
                    );

                    HttpResponseMessage response = 
                        await client.PatchAsync(($"http://https://localhost:5003/Users?userName=" +
                                                 $"{userToValidate.UserName}&password={userToValidate.Password}"), content);
                    if (!response.IsSuccessStatusCode)
                        throw new Exception($@"Error: {response.StatusCode}, {response.ReasonPhrase}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return userToValidate;
        }
        public async Task AddPersonAsync(Adult adult)
        {
            try
            {
                using HttpClient client = new HttpClient();
                string adultToAdd = JsonSerializer.Serialize(adult);
                Console.WriteLine(adultToAdd);
                StringContent content = new StringContent(adultToAdd, Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = await client.PostAsync("https://localhost:5003/Adults", content);
                if (!responseMessage.IsSuccessStatusCode)
                    throw new Exception($@"Error: {responseMessage.StatusCode}, {responseMessage.ReasonPhrase}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task RemovePersonAsync(int adultId)
        {
            try
            {
                using HttpClient client = new HttpClient();
                HttpResponseMessage responseMessage =
                    await client.DeleteAsync($"https://localhost:5003/Adults/{adultId}");
                if (!responseMessage.IsSuccessStatusCode)
                    throw new Exception($@"Error: {responseMessage.StatusCode}, {responseMessage.ReasonPhrase}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task<Adult> GetAdultAsync(int id)
        {
            Adult adult = new Adult();
            try
            {
                using HttpClient client = new HttpClient();
                HttpResponseMessage responseMessage = await client.GetAsync($"https://localhost:5003/Adults/{id}");
                if (!responseMessage.IsSuccessStatusCode)
                    throw new Exception($@"Error: {responseMessage.StatusCode}, {responseMessage.ReasonPhrase}");
                string result = await responseMessage.Content.ReadAsStringAsync();

                adult = JsonSerializer.Deserialize<Adult>(result, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return adult;
        }
    }
}