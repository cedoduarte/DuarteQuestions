namespace DuarteQuestions.DTOs
{
    public record AnswerCreatedDTO
    {
        public int Id { get; init; }
        public string? Text { get; init; }
    }
}
