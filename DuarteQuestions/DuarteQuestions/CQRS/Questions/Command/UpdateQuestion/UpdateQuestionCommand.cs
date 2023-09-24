using DuarteQuestions.Model;
using MediatR;

namespace DuarteQuestions.CQRS.Questions.Command.UpdateQuestion
{
    public class UpdateQuestionCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public List<int>? Answers { get; set; }
        public int RightAnswer { get; set; }
    }
}
