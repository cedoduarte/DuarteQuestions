using DuarteQuestions.CQRS.Users.ViewModel;
using MediatR;

namespace DuarteQuestions.CQRS.Users.Query.GetUserById
{
    public class GetUserByIdQuery : IRequest<UserViewModel>
    {
        public int Id { get; set; }
    }
}
