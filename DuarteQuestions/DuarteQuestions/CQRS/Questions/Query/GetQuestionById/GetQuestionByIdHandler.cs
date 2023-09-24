using AutoMapper;
using DuarteQuestions.CQRS.Questions.ViewModel;
using DuarteQuestions.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DuarteQuestions.CQRS.Questions.Query.GetQuestionById
{
    public class GetQuestionByIdHandler : IRequestHandler<GetQuestionByIdQuery, QuestionViewModel>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _dbContext;

        public GetQuestionByIdHandler(IMapper mapper, AppDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<QuestionViewModel> Handle(GetQuestionByIdQuery query, CancellationToken cancel)
        {
            try
            {
                Question? foundQuestion = await _dbContext.Questions
                    .Where(q => q.Id == query.Id && !q.IsDeleted)
                    .Include(q => q.Answers)
                    .FirstOrDefaultAsync(cancel);
                if (foundQuestion != null)
                {
                    return _mapper.Map<QuestionViewModel>(foundQuestion);
                }
                throw new Exception($"{nameof(Question)} with ID {query.Id} not found!");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
