using DuarteQuestions.CQRS.Answers.Command.CreateAnswer;
using DuarteQuestions.CQRS.Answers.Command.DeleteAnswer;
using DuarteQuestions.CQRS.Answers.Command.UpdateAnswer;
using DuarteQuestions.CQRS.Answers.Query.GetAnswerById;
using DuarteQuestions.CQRS.Answers.Query.GetAnswerList;
using DuarteQuestions.CQRS.Answers.ViewModel;
using DuarteQuestions.DTOs;
using DuarteQuestions.Service.Interface;
using MediatR;

namespace DuarteQuestions.Service
{
    public class AnswerService : IAnswerService
    {
        private readonly IMediator _mediator;

        public AnswerService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<AnswerCreatedDTO> CreateAnswer(CreateAnswerCommand command) 
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

        public async Task<bool> UpdateAnswer(UpdateAnswerCommand command)
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

        public async Task<bool> DeleteAnswer(int id)
        {
            try
            {
                return await _mediator.Send(new DeleteAnswerCommand()
                {
                    Id = id
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<AnswerViewModel>> GetAnswerList(GetAnswerListQuery query)
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

        public async Task<AnswerViewModel> GetAnswerById(int id)
        {
            try
            {
                return await _mediator.Send(new GetAnswerByIdQuery()
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
