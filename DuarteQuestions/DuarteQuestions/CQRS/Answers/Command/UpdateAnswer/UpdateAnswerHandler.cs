using DuarteQuestions.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DuarteQuestions.CQRS.Answers.Command.UpdateAnswer
{
    public class UpdateAnswerHandler : IRequestHandler<UpdateAnswerCommand, bool>
    {
        private readonly AppDbContext _dbContext;

        public UpdateAnswerHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(UpdateAnswerCommand command, CancellationToken cancel)
        {
            try
            {
                if (string.IsNullOrEmpty(command.Text))
                {
                    throw new Exception($"The answer cannot be empty!");
                }
                Answer? foundAnswer = await _dbContext.Answers
                    .Where(a => a.Id == command.Id && !a.IsDeleted)
                    .FirstOrDefaultAsync(cancel);
                if (foundAnswer != null)
                {
                    foundAnswer.Text = command.Text;
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
