using MediatR;

namespace DuarteQuestions.CQRS.Users.Command.ChangePassword
{
    public class ChangePasswordCommand : IRequest<bool>
    {
        public string? Username { get; set; }
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmedPassword { get; set; }
    }
}
