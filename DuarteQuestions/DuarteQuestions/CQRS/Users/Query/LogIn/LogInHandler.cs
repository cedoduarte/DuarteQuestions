using DuarteQuestions.Model;
using DuarteQuestions.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DuarteQuestions.CQRS.Users.Query.LogIn
{
    public class LogInHandler : IRequestHandler<LogInCommand, bool>
    {
        private readonly AppDbContext _dbContext;

        public LogInHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(LogInCommand command, CancellationToken cancel)
        {
            try
            {
                if (string.IsNullOrEmpty(command.Username))
                {
                    throw new Exception($"The username cannot be empty!");
                }
                if (string.IsNullOrEmpty(command.Password))
                {
                    throw new Exception($"the user's password cannot be empty!");
                }
                // the user can do login when the username is the user's name or is the user's email
                return await _dbContext.Users
                    .Where(u => !u.IsDeleted
                                && (u.Name == command.Username || u.Email == command.Username) 
                                && u.Password == Util.ToSHA256(command.Password))
                    .FirstOrDefaultAsync(cancel) != null;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
