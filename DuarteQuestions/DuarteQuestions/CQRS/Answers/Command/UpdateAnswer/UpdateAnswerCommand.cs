using MediatR;

namespace DuarteQuestions.CQRS.Answers.Command.UpdateAnswer
{
    public class UpdateAnswerCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string? Text { get; set; }
    }
}
