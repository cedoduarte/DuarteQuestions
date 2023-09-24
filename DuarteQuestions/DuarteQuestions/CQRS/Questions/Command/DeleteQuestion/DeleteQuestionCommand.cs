using MediatR;

namespace DuarteQuestions.CQRS.Questions.Command.DeleteQuestion
{
    public class DeleteQuestionCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
