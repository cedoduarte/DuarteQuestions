using DuarteQuestions.CQRS.Questions.Command.CreateQuestion;
using DuarteQuestions.CQRS.Questions.Command.DeleteQuestion;
using DuarteQuestions.CQRS.Questions.Command.UpdateQuestion;
using DuarteQuestions.CQRS.Questions.Query.GetQuestionById;
using DuarteQuestions.CQRS.Questions.Query.GetQuestionList;
using DuarteQuestions.CQRS.Questions.ViewModel;
using DuarteQuestions.Service.Interface;
using MediatR;

namespace DuarteQuestions.Service
{
    public class QuestionService : IQuestionService
    {
        private readonly IMediator _mediator;

        public QuestionService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> CreateQuestion(CreateQuestionCommand command)
        {
            try
            {
                return await _mediator.Send(command);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateQuestion(UpdateQuestionCommand command)
        {
            try
            {
                return await _mediator.Send(command);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteQuestion(int id)
        {
            try
            {
                return await _mediator.Send(new DeleteQuestionCommand()
                {
                    Id = id
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<QuestionViewModel>> GetQuestionList(GetQuestionListQuery query)
        {
            try
            {
                return await _mediator.Send(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<QuestionViewModel> GetQuestionById(int id)
        {
            try
            {
                return await _mediator.Send(new GetQuestionByIdQuery()
                {
                    Id = id
                });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
