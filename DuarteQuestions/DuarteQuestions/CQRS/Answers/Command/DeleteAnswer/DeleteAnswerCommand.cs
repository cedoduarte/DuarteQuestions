using MediatR;

namespace DuarteQuestions.CQRS.Answers.Command.DeleteAnswer
{
    public class DeleteAnswerCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
