using DuarteQuestions.CQRS.Questions.Command.CreateQuestion;
using DuarteQuestions.CQRS.Questions.Command.UpdateQuestion;
using DuarteQuestions.CQRS.Questions.Query.GetQuestionList;
using DuarteQuestions.CQRS.Questions.ViewModel;
using DuarteQuestions.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace DuarteQuestions.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : Controller
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpPost("create-question")]
        public async Task<ActionResult<bool>> CreateQuestion([FromBody] CreateQuestionCommand command)
        {
            try
            {
                return await _questionService.CreateQuestion(command);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("update-question")]
        public async Task<ActionResult<bool>> UpdateQuestion([FromBody] UpdateQuestionCommand command)
        {
            try
            {
                return await _questionService.UpdateQuestion(command);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete("delete-question/{id}")]
        public async Task<ActionResult<bool>> DeleteQuestion([FromRoute] int id)
        {
            try
            {
                return await _questionService.DeleteQuestion(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("get-question-list")]
        public async Task<IEnumerable<QuestionViewModel>> GetQuestionList([FromQuery] GetQuestionListQuery query)
        {
            try
            {
                return await _questionService.GetQuestionList(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("get-question/{id}")]
        public async Task<ActionResult<QuestionViewModel>> GetQuestionById([FromRoute] int id)
        {
            try
            {
                return await _questionService.GetQuestionById(id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
