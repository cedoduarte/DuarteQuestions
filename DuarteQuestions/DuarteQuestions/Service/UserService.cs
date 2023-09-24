using DuarteQuestions.CQRS.Users.Command.ChangePassword;
using DuarteQuestions.CQRS.Users.Command.CreateUser;
using DuarteQuestions.CQRS.Users.Command.DeleteUser;
using DuarteQuestions.CQRS.Users.Command.RestoreUser;
using DuarteQuestions.CQRS.Users.Command.UpdateUser;
using DuarteQuestions.CQRS.Users.Query.GetUserById;
using DuarteQuestions.CQRS.Users.Query.GetUserList;
using DuarteQuestions.CQRS.Users.Query.LogIn;
using DuarteQuestions.CQRS.Users.ViewModel;
using DuarteQuestions.Service.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DuarteQuestions.Service
{
    public class UserService : IUserService
    {
        private readonly IMediator _mediator;

        public UserService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> CreateUser(CreateUserCommand command)
        {
            try
            {
                return await _mediator.Send(command);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateUser(UpdateUserCommand command)
        {
            try
            {
                return await _mediator.Send(command);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteUser(int id)
        {
            try
            {
                return await _mediator.Send(new DeleteUserCommand()
                {
                    Id = id
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<UserViewModel>> GetUserList(GetUserListQuery query)
        {
            try
            {
                return await _mediator.Send(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserViewModel> GetUserById(int id)
        {
            try
            {
                return await _mediator.Send(new GetUserByIdQuery()
                {
                    Id = id
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> LogIn(LogInCommand command)
        {
            try
            {
                return await _mediator.Send(command);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> RestoreUser(RestoreUserCommand command)
        {
            try
            {
                return await _mediator.Send(command);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> ChangePassword(ChangePasswordCommand command)
        {
            try
            {
                return await _mediator.Send(command);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
