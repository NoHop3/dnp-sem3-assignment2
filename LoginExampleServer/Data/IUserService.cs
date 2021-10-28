using System.Collections.Generic;
using System.Threading.Tasks;
using LoginExampleServer.Models;

namespace LoginExampleServer.Data
{
    public interface IUserService
    {
        Task<IList<User>> GetUsersAsync();
        Task<User> ValidateUserAsync(string userName, string password);
        Task AddNewUserAsync(User user);
    }
}