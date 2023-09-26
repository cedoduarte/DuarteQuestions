using DuarteQuestions.CQRS.Questions.Command.CreateQuestion;
using DuarteQuestions.CQRS.Questions.Command.UpdateQuestion;
using DuarteQuestions.CQRS.Questions.Query.GetQuestionList;
using DuarteQuestions.CQRS.Questions.ViewModel;

namespace DuarteQuestions.Service.Interface
{
    public interface IQuestionService
    {
        Task<bool> CreateQuestion(CreateQuestionCommand command);
        Task<bool> UpdateQuestion(UpdateQuestionCommand command);
        Task<bool> DeleteQuestion(int id);
        Task<IEnumerable<QuestionViewModel>> GetQuestionList(GetQuestionListQuery query);
        Task<QuestionViewModel> GetQuestionById(int id);
        Task<bool> RestoreAll();
    }
}