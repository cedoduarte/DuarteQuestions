using DuarteQuestions.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DuarteQuestions.CQRS.Answers.Command.DeleteAnswer
{
    public class DeleteAnswerHandler : IRequestHandler<DeleteAnswerCommand, bool>
    {
        private readonly AppDbContext _dbContext;

        public DeleteAnswerHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(DeleteAnswerCommand command, CancellationToken cancel)
        {
            try
            {
                Answer? foundAnswer = await _dbContext.Answers
                    .Where(a => a.Id == command.Id && !a.IsDeleted)
                    .FirstOrDefaultAsync(cancel);
                if (foundAnswer != null)
                {
                    foundAnswer.IsDeleted = true;
                    _dbContext.Answers.Update(foundAnswer);
                    await _dbContext.SaveChangesAsync(cancel);
                    return true;
                }
                throw new Exception($"{nameof(Answer)} with ID {command.Id} not found!");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
