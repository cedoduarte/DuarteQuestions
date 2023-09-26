using DuarteQuestions.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DuarteQuestions.CQRS.Answers.Command.RestoreAllAnswer
{
    public class RestoreAllAnswerHandler : IRequestHandler<RestoreAllAnswerCommand, bool>
    {
        private readonly AppDbContext _dbContext;

        public RestoreAllAnswerHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(RestoreAllAnswerCommand command, CancellationToken cancel)
        {
            try
            {
                IEnumerable<Answer> foundAnswers = await _dbContext.Answers
                    .Where(a => a.IsDeleted)
                    .ToListAsync(cancel);
                foreach (Answer answer in foundAnswers)
                {
                    answer.IsDeleted = false;
                }
                _dbContext.Answers.UpdateRange(foundAnswers);
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
