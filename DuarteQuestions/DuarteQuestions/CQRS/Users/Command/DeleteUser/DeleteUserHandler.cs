using DuarteQuestions.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DuarteQuestions.CQRS.Users.Command.DeleteUser
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly AppDbContext _dbContext;

        public DeleteUserHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(DeleteUserCommand command, CancellationToken cancel)
        {
            try
            {
                User? foundUser = await _dbContext.Users
                    .Where(u => u.Id == command.Id && !u.IsDeleted)
                    .FirstOrDefaultAsync(cancel);
                if (foundUser != null)
                {
                    foundUser.IsDeleted = true;
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
