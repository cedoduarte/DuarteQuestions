using DuarteQuestions.Model;
using DuarteQuestions.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DuarteQuestions.CQRS.Users.Command.CreateUser
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, bool>
    {
        private readonly AppDbContext _dbContext;

        public CreateUserHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(CreateUserCommand command, CancellationToken cancel)
        {
            try
            {
                if (string.IsNullOrEmpty(command.Name))
                {
                    throw new Exception($"The new user's name cannot be empty!");
                }
                if (string.IsNullOrEmpty(command.Email))
                {
                    throw new Exception($"The new user's email cannot be empty!");
                }
                if (!Util.IsEmail(command.Email))
                {
                    throw new Exception($"The new user's email '{command.Email}' is not valid!");
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
                    throw new Exception($"The new user's password cannot be empty!");
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
                    .Where(u => u.Name == command.Name || u.Email == command.Email)
                    .FirstOrDefaultAsync(cancel);
                if (foundUser != null)
                {
                    throw new Exception($"{nameof(User)} with Name {command.Name} and Email {command.Email} already exists!");
                }
                var user = new User();
                user.Name = command.Name;
                user.Email = command.Email;
                user.Password = Util.ToSHA256(command.Password!);
                await _dbContext.Users.AddAsync(user, cancel);
                await _dbContext.SaveChangesAsync(cancel);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
