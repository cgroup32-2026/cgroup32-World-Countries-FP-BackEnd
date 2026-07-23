using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CountriesProject.BL;

namespace CountriesProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizzesController : ControllerBase
    {
        private readonly QuizBL _quizBL;

        public QuizzesController(QuizBL quizBL)
        {
            _quizBL = quizBL;
        }

        public class SubmitQuizRequest
        {
            public List<AnswerInput> Answers { get; set; }
            public int TimeTakenSeconds { get; set; }
        }

        private int GetCurrentUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        [HttpGet]
        public IActionResult GetAll() => Ok(_quizBL.GetAllQuizzes());

        [Authorize]
        [HttpGet("{quizId}/questions")]
        public IActionResult GetQuestions(int quizId)
        {
            try { return Ok(_quizBL.GetQuizForPlayer(quizId)); }
            catch (Exception ex) { return NotFound(new { message = ex.Message }); }
        }

        [Authorize]
        [HttpPost("{quizId}/submit")]
        public IActionResult Submit(int quizId, SubmitQuizRequest request)
        {
            try
            {
                var result = _quizBL.SubmitAttempt(GetCurrentUserId(), quizId, request.Answers, request.TimeTakenSeconds);
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [Authorize]
        [HttpGet("me/attempts")]
        public IActionResult GetMyAttempts() => Ok(_quizBL.GetMyAttempts(GetCurrentUserId()));

        [HttpGet("{quizId}/leaderboard")]
        public IActionResult GetLeaderboard(int quizId) => Ok(_quizBL.GetLeaderboard(quizId));
    }
}