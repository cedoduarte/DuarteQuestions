using AutoMapper;
using DuarteQuestions.CQRS.Users.ViewModel;
using DuarteQuestions.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DuarteQuestions.CQRS.Users.Query.GetUserList
{
    public class GetUserListHandler : IRequestHandler<GetUserListQuery, IEnumerable<UserViewModel>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _dbContext;

        public GetUserListHandler(IMapper mapper, AppDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<UserViewModel>> Handle(GetUserListQuery query, CancellationToken cancel)
        {
            try
            {
                if (query.GetAll)
                {
                    return _mapper.Map<IEnumerable<UserViewModel>>(await _dbContext.Users
                        .Where(u => !u.IsDeleted)
                        .ToListAsync(cancel));
                }
                if (!string.IsNullOrEmpty(query.Keyword))
                {
                    return _mapper.Map<IEnumerable<UserViewModel>>(await _dbContext.Users
                        .Where(u =>
                            !u.IsDeleted
                            && (
                                (// the name is filled?
                                 !string.IsNullOrEmpty(u.Name) ?
                                    // the name contains the keyword?
                                    u.Name.ToLower().Contains(query.Keyword.ToLower())
                                 // the text is null or empty
                                 : false)
                            ||
                                (// the email is filled?
                                 !string.IsNullOrEmpty(u.Email) ?
                                    // the email contains the keyword?
                                    u.Email.ToLower().Contains(query.Keyword.ToLower())
                                 // the text is null or empty
                                 : false)
                        )).ToListAsync(cancel));
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
