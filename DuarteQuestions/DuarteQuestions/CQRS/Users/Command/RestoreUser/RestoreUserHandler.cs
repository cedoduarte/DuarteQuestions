using DuarteQuestions.Model;
using DuarteQuestions.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DuarteQuestions.CQRS.Users.Command.RestoreUser
{
    public class RestoreUserHandler : IRequestHandler<RestoreUserCommand, bool>
    {
        private readonly AppDbContext _dbContext;

        public RestoreUserHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(RestoreUserCommand command, CancellationToken cancel)
        {
            try
            {
                if (string.IsNullOrEmpty(command.Username))
                {
                    throw new Exception($"The username cannot be empty!");
                }
                if (string.IsNullOrEmpty(command.Password))
                {
                    throw new Exception($"The user's password cannot be empty!");
                }
                if (string.IsNullOrEmpty(command.ConfirmedPassword))
                {
                    throw new Exception($"The confirmed password cannot be empty!");
                }
                if (command.Password != command.ConfirmedPassword)
                {
                    throw new Exception($"The password is not confirmed!");
                }
                User? foundUser = await _dbContext.Users
                        .Where(u =>
                            u.IsDeleted
                            && (u.Name == command.Username || u.Email == command.Username)
                            && u.Password == Util.ToSHA256(command.Password))
                        .FirstOrDefaultAsync(cancel);
                if (foundUser != null)
                {
                    foundUser.IsDeleted = false;
                    _dbContext.Users.Update(foundUser);
                    await _dbContext.SaveChangesAsync(cancel);
                    return true;
                }
                throw new Exception($"{nameof(User)} with Username {command.Username} not found!");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
