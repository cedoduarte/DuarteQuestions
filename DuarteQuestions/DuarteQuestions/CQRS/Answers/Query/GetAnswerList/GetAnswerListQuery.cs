using DuarteQuestions.CQRS.Answers.ViewModel;
using MediatR;

namespace DuarteQuestions.CQRS.Answers.Query.GetAnswerList
{
    public class GetAnswerListQuery : IRequest<IEnumerable<AnswerViewModel>>
    {
        public string? Keyword { get; set; }
        public bool GetAll { get; set; }
    }
}
