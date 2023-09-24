using DuarteQuestions.Model;
using DuarteQuestions.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DuarteQuestions.CQRS.Users.Command.ChangePassword
{
    public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, bool>
    {
        private readonly AppDbContext _dbContext;

        public ChangePasswordHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(ChangePasswordCommand command, CancellationToken cancel)
        {
            try
            {
                if (string.IsNullOrEmpty(command.Username))
                {
                    throw new Exception($"The username cannot be empty!");
                }
                if (string.IsNullOrEmpty(command.CurrentPassword))
                {
                    throw new Exception($"The current password cannot be empty!");
                }
                if (string.IsNullOrEmpty(command.NewPassword))
                {
                    throw new Exception($"The new password cannot be empty!");
                }
                if (string.IsNullOrEmpty(command.ConfirmedPassword))
                {
                    throw new Exception($"The confirmed password cannot be empty!");
                }
                if (command.NewPassword != command.ConfirmedPassword)
                {
                    throw new Exception($"The password is not confirmed!");
                }
                User? foundUser = await _dbContext.Users
                    .Where(u => !u.IsDeleted 
                           && (u.Name == command.Username || u.Email == command.Username) 
                           && u.Password == Util.ToSHA256(command.CurrentPassword))
                    .FirstOrDefaultAsync(cancel);
                if (foundUser != null)
                {
                    foundUser.Password = Util.ToSHA256(command.NewPassword!);
                    _dbContext.Users.Update(foundUser);
                    await _dbContext.SaveChangesAsync(cancel);
                    return true;
                }
                throw new Exception($"{nameof(User)} with Username/Email {command.Username} not found, please resend information!");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
