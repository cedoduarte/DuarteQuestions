namespace DuarteQuestions.Model
{
    public class Answer
    {
        public int Id { get; set; }
        public string? Text { get; set; } = "no text";
        public bool IsDeleted { get; set; } = false;
    }
}
