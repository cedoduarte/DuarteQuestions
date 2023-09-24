using DuarteQuestions.Model;

namespace DuarteQuestions.CQRS.Questions.ViewModel
{
    public class QuestionViewModel
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public virtual IEnumerable<Answer>? Answers { get; set; }
        public virtual Answer? RightAnswer { get; set; }
        public bool IsDeleted { get; set; }
    }
}
