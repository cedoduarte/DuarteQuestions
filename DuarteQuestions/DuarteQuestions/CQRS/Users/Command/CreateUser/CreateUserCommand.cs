using MediatR;

namespace DuarteQuestions.CQRS.Users.Command.CreateUser
{
    public class CreateUserCommand : IRequest<bool>
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? ConfirmedEmail { get; set; }
        public string? Password { get; set; }
        public string? ConfirmedPassword { get; set; }
    }
}
