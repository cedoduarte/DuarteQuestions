using DuarteQuestions.CQRS.Users.Command.ChangePassword;
using DuarteQuestions.CQRS.Users.Command.CreateUser;
using DuarteQuestions.CQRS.Users.Command.RestoreUser;
using DuarteQuestions.CQRS.Users.Command.UpdateUser;
using DuarteQuestions.CQRS.Users.Query.GetUserList;
using DuarteQuestions.CQRS.Users.Query.LogIn;
using DuarteQuestions.CQRS.Users.ViewModel;

namespace DuarteQuestions.Service.Interface
{
    public interface IUserService
    {
        Task<bool> CreateUser(CreateUserCommand command);
        Task<bool> UpdateUser(UpdateUserCommand command);
        Task<bool> DeleteUser(int id);
        Task<IEnumerable<UserViewModel>> GetUserList(GetUserListQuery query);
        Task<UserViewModel> GetUserById(int id);
        Task<bool> LogIn(LogInCommand command);
        Task<bool> RestoreUser(RestoreUserCommand command);
        Task<bool> ChangePassword(ChangePasswordCommand command);
    }
}
