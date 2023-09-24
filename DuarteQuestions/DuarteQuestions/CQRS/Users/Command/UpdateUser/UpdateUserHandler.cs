using DuarteQuestions.Model;
using DuarteQuestions.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DuarteQuestions.CQRS.Users.Command.UpdateUser
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly AppDbContext _dbContext;

        public UpdateUserHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(UpdateUserCommand command, CancellationToken cancel)
        {
            try
            {
                if (string.IsNullOrEmpty(command.Name))
                {
                    throw new Exception($"The username cannot be empty!");
                }
                if (string.IsNullOrEmpty(command.Email))
                {
                    throw new Exception($"The user's email cannot be empty!");
                }
                if (string.IsNullOrEmpty(command.ConfirmedEmail))
                {
                    throw new Exception($"The confirmed email cannot be empty!");
                }
                if (command.Email != command.ConfirmedEmail)
                {
                    throw new Exception($"The email is not confirmed!");
                }
                if (string.IsNullOrEmpty(command.Password))
                {
                    throw new Exception($"The user's password cannot be empty!");
                }
                if (string.IsNullOrEmpty(command.ConfirmedPassword))
                {
                    throw new Exception($"the confirmed password cannot be empty!");
                }
                if (command.Password != command.ConfirmedPassword)
                {
                    throw new Exception($"The password is not confirmed!");
                }
                User? foundUser = await _dbContext.Users
                    .Where(u => u.Id == command.Id && !u.IsDeleted)
                    .FirstOrDefaultAsync(cancel);
                if (foundUser != null)
                {
                    foundUser.Name = command.Name;
                    foundUser.Email = command.Email;
                    foundUser.Password = Util.ToSHA256(command.Password);
                    _dbContext.Users.Update(foundUser);
                    await _dbContext.SaveChangesAsync(cancel);
                    return true;
                }
                throw new Exception($"{nameof(User)} with ID {command.Id} not found!");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
