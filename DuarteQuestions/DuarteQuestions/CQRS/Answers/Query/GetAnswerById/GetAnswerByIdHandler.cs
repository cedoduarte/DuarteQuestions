using AutoMapper;
using DuarteQuestions.CQRS.Answers.ViewModel;
using DuarteQuestions.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DuarteQuestions.CQRS.Answers.Query.GetAnswerById
{
    public class GetAnswerByIdHandler : IRequestHandler<GetAnswerByIdQuery, AnswerViewModel>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _dbContext;

        public GetAnswerByIdHandler(IMapper mapper, AppDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<AnswerViewModel> Handle(GetAnswerByIdQuery query, CancellationToken cancel)
        {
            try
            {
                Answer? foundAnswer = await _dbContext.Answers
                    .Where(a => a.Id == query.Id && !a.IsDeleted)
                    .FirstOrDefaultAsync(cancel);
                if (foundAnswer != null)
                {
                    return _mapper.Map<AnswerViewModel>(foundAnswer);
                }
                throw new Exception($"{nameof(Answer)} with ID {query.Id} not found!");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
