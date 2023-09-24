using DuarteQuestions.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DuarteQuestions.CQRS.Questions.Command.CreateQuestion
{
    public class CreateQuestionHandler : IRequestHandler<CreateQuestionCommand, bool>
    {
        private readonly AppDbContext _dbContext;

        public CreateQuestionHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(CreateQuestionCommand command, CancellationToken cancel)
        {
            try
            { 
                if (string.IsNullOrEmpty(command.Text))
                {
                    throw new Exception($"The question cannot be empty!");
                }
                if (command.Answers == null)
                {
                    throw new Exception("The question needs to have at least one answer!");
                }
                else
                {
                    if (!command.Answers.Any())
                    {
                        throw new Exception("The question needs to have at least one answer!");
                    }
                }
                var question = new Question();
                question.Text = command.Text;
                foreach (int answerId in command.Answers!)
                {
                    Answer? foundAnswer = await _dbContext.Answers
                        .Where(a => a.Id == answerId)
                        .FirstOrDefaultAsync(cancel);
                    if (foundAnswer != null)
                    {
                        question.Answers!.Add(foundAnswer);                        
                    }
                }
                question.RightAnswer = await _dbContext.Answers
                    .Where(a => a.Id == command.RightAnswer)
                    .FirstOrDefaultAsync(cancel);
                await _dbContext.Questions.AddAsync(question, cancel);
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
