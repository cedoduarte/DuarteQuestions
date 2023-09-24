using DuarteQuestions.CQRS.Questions.ViewModel;
using MediatR;

namespace DuarteQuestions.CQRS.Questions.Query.GetQuestionList
{
    public class GetQuestionListQuery : IRequest<IEnumerable<QuestionViewModel>>
    {
        public string? Keyword { get; set; }
        public bool GetAll { get; set; }
    }
}
