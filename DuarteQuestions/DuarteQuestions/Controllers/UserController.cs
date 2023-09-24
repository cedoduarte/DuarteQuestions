using DuarteQuestions.CQRS.Users.Command.ChangePassword;
using DuarteQuestions.CQRS.Users.Command.CreateUser;
using DuarteQuestions.CQRS.Users.Command.RestoreUser;
using DuarteQuestions.CQRS.Users.Command.UpdateUser;
using DuarteQuestions.CQRS.Users.Query.GetUserList;
using DuarteQuestions.CQRS.Users.Query.LogIn;
using DuarteQuestions.CQRS.Users.ViewModel;
using DuarteQuestions.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace DuarteQuestions.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("create-user")]
        public async Task<ActionResult<bool>> CreateUser([FromBody] CreateUserCommand command)
        {
            try
            {
                return await _userService.CreateUser(command);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("update-user")]
        public async Task<ActionResult<bool>> UpdateUser([FromBody] UpdateUserCommand command)
        {
            try
            {
                return await _userService.UpdateUser(command);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete("delete-user/{id}")]
        public async Task<ActionResult<bool>> DeleteUser([FromRoute] int id)
        {
            try
            {
                return await _userService.DeleteUser(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("get-user-list")]
        public async Task<IEnumerable<UserViewModel>> GetUserList([FromQuery] GetUserListQuery query)
        {
            try
            {
                return await _userService.GetUserList(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("get-user/{id}")]
        public async Task<ActionResult<UserViewModel>> GetUserById([FromRoute] int id)
        {
            try
            {
                return await _userService.GetUserById(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<bool>> LogIn([FromBody] LogInCommand command)
        {
            try
            {
                return await _userService.LogIn(command);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("restore-user")]
        public async Task<ActionResult<bool>> RestoreUser([FromBody] RestoreUserCommand command)
        {
            try
            {
                return await _userService.RestoreUser(command);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("change-user-password")]
        public async Task<ActionResult<bool>> ChangePassword([FromBody] ChangePasswordCommand command)
        {
            try
            {
                return await _userService.ChangePassword(command);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
