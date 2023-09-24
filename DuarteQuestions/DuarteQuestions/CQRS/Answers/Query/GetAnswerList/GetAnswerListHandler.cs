using AutoMapper;
using DuarteQuestions.CQRS.Answers.ViewModel;
using DuarteQuestions.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DuarteQuestions.CQRS.Answers.Query.GetAnswerList
{
    public class GetAnswerListHandler : IRequestHandler<GetAnswerListQuery, IEnumerable<AnswerViewModel>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _dbContext;

        public GetAnswerListHandler(IMapper mapper, AppDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<AnswerViewModel>> Handle(GetAnswerListQuery query, CancellationToken cancel)
        {
            try
            {
                if (query.GetAll)
                {
                    return _mapper.Map<IEnumerable<AnswerViewModel>>(await _dbContext.Answers
                        .Where(a => !a.IsDeleted)
                        .ToListAsync(cancel));
                }
                if (!string.IsNullOrEmpty(query.Keyword))
                {
                    return _mapper.Map<IEnumerable<AnswerViewModel>>(await _dbContext.Answers
                        .Where(a =>
                            !a.IsDeleted
                                && (
                                // the text is filled?
                                !string.IsNullOrEmpty(a.Text) ?
                                    // the text contains the keyword?
                                    a.Text.ToLower().Contains(query.Keyword.ToLower())
                                // the text is null or empty
                                : false)
                        ).ToListAsync(cancel));
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
