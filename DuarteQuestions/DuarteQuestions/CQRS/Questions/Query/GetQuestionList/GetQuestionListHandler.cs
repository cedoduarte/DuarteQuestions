using AutoMapper;
using DuarteQuestions.CQRS.Questions.ViewModel;
using DuarteQuestions.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DuarteQuestions.CQRS.Questions.Query.GetQuestionList
{
    public class GetQuestionListHandler : IRequestHandler<GetQuestionListQuery, IEnumerable<QuestionViewModel>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _dbContext;

        public GetQuestionListHandler(IMapper mapper, AppDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<QuestionViewModel>> Handle(GetQuestionListQuery query, CancellationToken cancel)
        {
            try
            {
                if (query.GetAll)
                {
                    return _mapper.Map<IEnumerable<QuestionViewModel>>(await _dbContext.Questions
                        .Where(q => !q.IsDeleted)
                        .Include(q => q.Answers)
                        .ToListAsync(cancel));
                }
                if (!string.IsNullOrEmpty(query.Keyword))
                {
                    return _mapper.Map<IEnumerable<QuestionViewModel>>(await _dbContext.Questions
                        .Where(q =>
                            !q.IsDeleted
                                && (
                                // the text is filled?
                                !string.IsNullOrEmpty(q.Text) ?
                                    // the text contains the keyword?
                                    q.Text.ToLower().Contains(query.Keyword!.ToLower())
                                // the text is null or empty
                                : false)
                        ).Include(q => q.Answers)
                            .ToListAsync(cancel));
                }
                throw new Exception($"Unknown exception!");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
