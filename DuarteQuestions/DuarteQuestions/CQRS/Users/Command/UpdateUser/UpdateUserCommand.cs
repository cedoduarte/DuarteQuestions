using MediatR;

namespace DuarteQuestions.CQRS.Users.Command.UpdateUser
{
    public class UpdateUserCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? ConfirmedEmail { get; set; }
        public string? Password { get; set; }
        public string? ConfirmedPassword { get; set; }
    }
}
