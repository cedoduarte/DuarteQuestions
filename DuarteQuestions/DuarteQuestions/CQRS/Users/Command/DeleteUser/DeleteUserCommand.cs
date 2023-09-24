using MediatR;

namespace DuarteQuestions.CQRS.Users.Command.DeleteUser
{
    public class DeleteUserCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
