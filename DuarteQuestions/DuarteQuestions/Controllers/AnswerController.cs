using DuarteQuestions.CQRS.Answers.Command.CreateAnswer;
using DuarteQuestions.CQRS.Answers.Command.UpdateAnswer;
using DuarteQuestions.CQRS.Answers.Query.GetAnswerList;
using DuarteQuestions.CQRS.Answers.ViewModel;
using DuarteQuestions.DTOs;
using DuarteQuestions.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace DuarteQuestions.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnswerController : Controller
    {
        private readonly IAnswerService _answerService;
        
        public AnswerController(IAnswerService answerService)
        {
            _answerService = answerService;
        }

        [HttpPost("create-answer")]
        public async Task<ActionResult<AnswerCreatedDTO>> CreateAnswer([FromBody] CreateAnswerCommand command)
        {
            try
            {
                return await _answerService.CreateAnswer(command);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("update-answer")]
        public async Task<ActionResult<bool>> UpdateAnswer([FromBody] UpdateAnswerCommand command)
        {
            try
            {
                return await _answerService.UpdateAnswer(command);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete("delete-answer/{id}")]
        public async Task<ActionResult<bool>> DeleteAnswer([FromRoute] int id)
        {
            try
            {
                return await _answerService.DeleteAnswer(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("get-answer-list")]
        public async Task<IEnumerable<AnswerViewModel>> GetAnswerList([FromQuery] GetAnswerListQuery query)
        {
            try
            {
                return await _answerService.GetAnswerList(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("get-answer/{id}")]
        public async Task<ActionResult<AnswerViewModel>> GetAnswerById([FromRoute] int id)
        {
            try
            {
                return await _answerService.GetAnswerById(id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
