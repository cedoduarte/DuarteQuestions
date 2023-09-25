using DuarteQuestions.DTOs;
using DuarteQuestions.Model;
using MediatR;

namespace DuarteQuestions.CQRS.Answers.Command.CreateAnswer
{
    public class CreateAnswerHandler : IRequestHandler<CreateAnswerCommand, AnswerCreatedDTO>
    {
        private readonly AppDbContext _dbContext;

        public CreateAnswerHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AnswerCreatedDTO> Handle(CreateAnswerCommand command, CancellationToken cancel)
        {
            try
            {
                if (string.IsNullOrEmpty(command.Text))
                {
                    throw new Exception($"The answer cannot be empty!");
                }
                var answer = new Answer();
                answer.Text = command.Text;
                await _dbContext.Answers.AddAsync(answer, cancel);
                await _dbContext.SaveChangesAsync(cancel);
                return new AnswerCreatedDTO()
                {
                    Id = answer.Id,
                    Text = answer.Text
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
