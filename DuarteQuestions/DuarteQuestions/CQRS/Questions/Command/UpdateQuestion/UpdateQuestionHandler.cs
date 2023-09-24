using DuarteQuestions.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DuarteQuestions.CQRS.Questions.Command.UpdateQuestion
{
    public class UpdateQuestionHandler : IRequestHandler<UpdateQuestionCommand, bool>
    {
        private readonly AppDbContext _dbContext;

        public UpdateQuestionHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(UpdateQuestionCommand command, CancellationToken cancel)
        {
            try
            {
                if (string.IsNullOrEmpty(command.Text))
                {
                    throw new Exception($"The question cannot be empty!");
                }
                Question? foundQuestion = await _dbContext.Questions
                    .Where(a => a.Id == command.Id && !a.IsDeleted)
                    .FirstOrDefaultAsync(cancel);
                if (foundQuestion != null)
                {
                    foundQuestion.Text = command.Text;
                    foundQuestion.Answers!.Clear();
                    foreach (int answerId in command.Answers!)
                    {
                        Answer? foundAnswer = await _dbContext.Answers
                            .Where(a => a.Id == answerId)
                            .FirstOrDefaultAsync(cancel);
                        if (foundAnswer != null)
                        {
                            foundQuestion.Answers!.Add(foundAnswer);
                        }
                    }
                    foundQuestion.RightAnswer = await _dbContext.Answers
                        .Where(a => a.Id == command.RightAnswer)
                        .FirstOrDefaultAsync(cancel);
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