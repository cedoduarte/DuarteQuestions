using AutoMapper;
using DuarteQuestions.CQRS.Users.ViewModel;
using DuarteQuestions.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DuarteQuestions.CQRS.Users.Query.GetUserById
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserViewModel>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _dbContext;

        public GetUserByIdHandler(IMapper mapper, AppDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<UserViewModel> Handle(GetUserByIdQuery query, CancellationToken cancel)
        {
            try
            {
                User? foundUser = await _dbContext.Users
                    .Where(u => u.Id == query.Id && !u.IsDeleted)
                    .FirstOrDefaultAsync(cancel);
                if (foundUser != null)
                {
                    return _mapper.Map<UserViewModel>(foundUser);
                }
                throw new Exception($"{nameof(User)} with ID {query.Id} not found!");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
