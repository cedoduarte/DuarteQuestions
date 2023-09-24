namespace DuarteQuestions.CQRS.Users.ViewModel
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
