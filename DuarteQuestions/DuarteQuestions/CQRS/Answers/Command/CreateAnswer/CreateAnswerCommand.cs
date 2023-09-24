using MediatR;

namespace DuarteQuestions.CQRS.Answers.Command.CreateAnswer
{
    public class CreateAnswerCommand : IRequest<bool>
    {
        public string? Text { get; set; }
    }
}
