using DuarteQuestions.DTOs;
using MediatR;

namespace DuarteQuestions.CQRS.Answers.Command.CreateAnswer
{
    public class CreateAnswerCommand : IRequest<AnswerCreatedDTO>
    {
        public string? Text { get; set; }
    }
}
