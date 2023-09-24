using DuarteQuestions.CQRS.Answers.ViewModel;
using MediatR;

namespace DuarteQuestions.CQRS.Answers.Query.GetAnswerById
{
    public class GetAnswerByIdQuery : IRequest<AnswerViewModel>
    {
        public int Id { get; set; }
    }
}
