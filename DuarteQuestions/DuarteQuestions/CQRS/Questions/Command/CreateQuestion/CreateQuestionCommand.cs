using MediatR;

namespace DuarteQuestions.CQRS.Questions.Command.CreateQuestion
{
    public class CreateQuestionCommand : IRequest<bool>
    {
        public string? Text { get; set; }
        public IEnumerable<int>? Answers { get; set; }
        public int RightAnswer { get; set; }
    }
}
