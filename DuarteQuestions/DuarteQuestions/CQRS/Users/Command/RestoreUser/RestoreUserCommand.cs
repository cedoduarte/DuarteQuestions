using MediatR;

namespace DuarteQuestions.CQRS.Users.Command.RestoreUser
{
    public class RestoreUserCommand : IRequest<bool>
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? ConfirmedPassword { get; set; }
    }
}
