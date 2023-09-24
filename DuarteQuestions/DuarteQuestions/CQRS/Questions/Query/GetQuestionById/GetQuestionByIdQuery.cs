using DuarteQuestions.CQRS.Questions.ViewModel;
using MediatR;

namespace DuarteQuestions.CQRS.Questions.Query.GetQuestionById
{
    public class GetQuestionByIdQuery : IRequest<QuestionViewModel>
    {
        public int Id { get; set; }
    }
}
