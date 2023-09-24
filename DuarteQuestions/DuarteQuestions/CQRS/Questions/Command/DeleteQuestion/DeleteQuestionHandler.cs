using DuarteQuestions.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DuarteQuestions.CQRS.Questions.Command.DeleteQuestion
{
    public class DeleteQuestionHandler : IRequestHandler<DeleteQuestionCommand, bool>
    {
        private readonly AppDbContext _dbContext;

        public DeleteQuestionHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(DeleteQuestionCommand command, CancellationToken cancel)
        {
            try
            {
                Question? foundQuestion = await _dbContext.Questions
                    .Where(a => a.Id == command.Id && !a.IsDeleted)
                    .FirstOrDefaultAsync(cancel);
                if (foundQuestion != null)
                {
                    foundQuestion.IsDeleted = true;
                    _dbContext.Questions.Update(foundQuestion);
                    await _dbContext.SaveChangesAsync(cancel);
                    return true;
                }
                throw new Exception($"{nameof(Question)} with ID {command.Id} not found!");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
