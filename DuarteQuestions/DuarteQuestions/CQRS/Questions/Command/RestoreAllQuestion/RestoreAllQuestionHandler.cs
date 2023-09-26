using DuarteQuestions.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DuarteQuestions.CQRS.Questions.Command.RestoreAllQuestion
{
    public class RestoreAllQuestionHandler : IRequestHandler<RestoreAllQuestionCommand, bool>
    {
        private readonly AppDbContext _dbContext;

        public RestoreAllQuestionHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(RestoreAllQuestionCommand command, CancellationToken cancel)
        {
            try
            {
                IEnumerable<Question> foundQuestions = await _dbContext.Questions.Where(q => q.IsDeleted).ToListAsync(cancel);
                foreach (Question question in foundQuestions)
                {
                    question.IsDeleted = false;
                }
                _dbContext.Questions.UpdateRange(foundQuestions);
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
