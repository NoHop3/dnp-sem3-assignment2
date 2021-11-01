using System.Collections.Generic;
using System.Threading.Tasks;
using LoginExample.Models;

namespace LoginExample.Data {
public interface IUserService
{
    public Task<IList<User>> GetUsersAsync(); 
    public Task<User> ValidateUserAsync(string userName, string password);
    public Task AddNewUserAsync(User user);
}
}