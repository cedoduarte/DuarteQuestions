using Microsoft.EntityFrameworkCore;

namespace DuarteQuestions.Model.Interface
{
    public interface IAppDbContext
    {
        DbSet<Answer> Answers { get; set; }
        DbSet<Question> Questions { get; set; }
        DbSet<User> Users { get; set; }
    }
}
