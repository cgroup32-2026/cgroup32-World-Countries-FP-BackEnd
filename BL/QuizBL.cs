using CountriesProject.DAL;
using CountriesProject.DAL.Models;

namespace CountriesProject.BL
{
    public class QuizBL
    {
        private readonly QuizDAL _quizDAL;

        public QuizBL(QuizDAL quizDAL)
        {
            _quizDAL = quizDAL;
        }

        public List<Quiz> GetAllQuizzes() => _quizDAL.GetAllQuizzes();

        public QuizForPlayer GetQuizForPlayer(int quizId)
        {
            Quiz quiz = _quizDAL.GetQuizById(quizId);
            if (quiz == null) throw new Exception("Quiz not found");

            List<QuizQuestion> questions = _quizDAL.GetQuestionsForQuiz(quizId);

            return new QuizForPlayer
            {
                QuizId = quiz.QuizId,
                Title = quiz.Title,
                Description = quiz.Description,
                TimeLimitSeconds = quiz.TimeLimitSeconds,
                Questions = questions.Select(q => new QuizQuestionForPlayer
                {
                    QuestionId = q.QuestionId,
                    QuestionText = q.QuestionText,
                    OptionA = q.OptionA,
                    OptionB = q.OptionB,
                    OptionC = q.OptionC,
                    OptionD = q.OptionD
                }).ToList()
            };
        }

        public QuizSubmissionResult SubmitAttempt(int userId, int quizId, List<AnswerInput> answers, int timeTakenSeconds)
        {
            Quiz quiz = _quizDAL.GetQuizById(quizId);
            if (quiz == null) throw new Exception("Quiz not found");

            if (timeTakenSeconds > quiz.TimeLimitSeconds)
                throw new Exception("Time limit exceeded for this quiz");

            List<QuizQuestion> questions = _quizDAL.GetQuestionsForQuiz(quizId);
            int score = 0;
            List<QuestionResult> results = new List<QuestionResult>();

            foreach (var question in questions)
            {
                var submitted = answers.FirstOrDefault(a => a.QuestionId == question.QuestionId);
                bool wasCorrect = submitted != null &&
                    string.Equals(submitted.SelectedOption, question.CorrectOption, StringComparison.OrdinalIgnoreCase);

                if (wasCorrect) score++;

                results.Add(new QuestionResult
                {
                    QuestionId = question.QuestionId,
                    WasCorrect = wasCorrect,
                    CorrectOption = question.CorrectOption
                });
            }

            _quizDAL.InsertAttempt(userId, quizId, score, timeTakenSeconds);

            return new QuizSubmissionResult
            {
                Score = score,
                TotalQuestions = questions.Count,
                TimeTakenSeconds = timeTakenSeconds,
                Results = results
            };
        }

        public List<QuizAttempt> GetMyAttempts(int userId) => _quizDAL.GetAttemptsForUser(userId);

        public List<LeaderboardEntry> GetLeaderboard(int quizId, int top = 10) => _quizDAL.GetLeaderboard(quizId, top);
    }
}