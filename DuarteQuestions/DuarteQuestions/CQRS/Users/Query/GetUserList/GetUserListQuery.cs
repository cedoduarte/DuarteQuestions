using DuarteQuestions.CQRS.Users.ViewModel;
using MediatR;

namespace DuarteQuestions.CQRS.Users.Query.GetUserList
{
    public class GetUserListQuery : IRequest<IEnumerable<UserViewModel>>
    {
        public string? Keyword { get; set; }
        public bool GetAll { get; set; }
    }
}
