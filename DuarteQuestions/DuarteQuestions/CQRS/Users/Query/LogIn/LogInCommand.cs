using MediatR;

namespace DuarteQuestions.CQRS.Users.Query.LogIn
{
    public class LogInCommand : IRequest<bool>
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
