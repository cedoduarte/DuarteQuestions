namespace DuarteQuestions.Model
{
    public class Question
    {
        public int Id { get; set; }
        public string? Text { get; set; } = "no text";
        public virtual List<Answer>? Answers { get; set; } = new List<Answer>();
        public virtual Answer? RightAnswer { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
