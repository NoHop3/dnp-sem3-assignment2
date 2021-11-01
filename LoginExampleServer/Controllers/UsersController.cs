using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoginExampleServer.Data;
using LoginExampleServer.Data.Impl;
using LoginExampleServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace LoginExampleServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService peopleData)
        {
            this._userService = peopleData;
        }

        [HttpGet]
        public async Task<ActionResult<IList<User>>> GetUsers([FromQuery] string? username, [FromQuery] string? role)
        {
            try
            {
                IList<User> allUsers = await _userService.GetUsersAsync();
                IList<User> filteredUsers = new List<User>();
                if (username == null && role == null)
                    filteredUsers = allUsers;
                else
                {
                    if (role != null)
                        foreach (var user in allUsers)
                        {
                            if (user.Role.Equals(role)) filteredUsers.Add(user);
                        }

                    if (username != null)
                        foreach (var user in allUsers)
                        {
                            if (user.UserName.Equals(username)) filteredUsers.Add(user);
                        }
                }

                return Ok(filteredUsers);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<User>> RegisterUser([FromBody] User user)
        {
            try
            {
                await _userService.AddNewUserAsync(user);
                return Created($"/{user.Id}", user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpPatch]
        public async Task<ActionResult<User>> ValidateUser([FromQuery] string userName, [FromQuery] string password)
        {
            try
            {
                await _userService.ValidateUserAsync(userName, password);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
    }
}