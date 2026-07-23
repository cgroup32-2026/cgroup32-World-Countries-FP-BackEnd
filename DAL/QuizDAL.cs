using System.Data;
using Microsoft.Data.SqlClient;
using CountriesProject.DAL.Models;

namespace CountriesProject.DAL
{
    public class QuizDAL : DBServices
    {
        public QuizDAL(IConfiguration config) : base(config) { }

        public List<Quiz> GetAllQuizzes()
        {
            List<Quiz> list = new List<Quiz>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_Quizzes_FP_RM_GetAll", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Quiz
                    {
                        QuizId = (int)reader["QuizId"],
                        Title = reader["Title"].ToString(),
                        Description = reader["Description"] as string,
                        TimeLimitSeconds = (int)reader["TimeLimitSeconds"],
                        QuestionCount = (int)reader["QuestionCount"]
                    });
                }
            }
            return list;
        }

        public Quiz GetQuizById(int quizId)
        {
            Quiz quiz = null;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_Quizzes_FP_RM_GetById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@QuizId", quizId);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    quiz = new Quiz
                    {
                        QuizId = (int)reader["QuizId"],
                        Title = reader["Title"].ToString(),
                        Description = reader["Description"] as string,
                        TimeLimitSeconds = (int)reader["TimeLimitSeconds"],
                        QuestionCount = (int)reader["QuestionCount"]
                    };
                }
            }
            return quiz;
        }

        public List<QuizQuestion> GetQuestionsForQuiz(int quizId)
        {
            List<QuizQuestion> list = new List<QuizQuestion>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_QuizQuestions_FP_RM_GetForQuiz", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@QuizId", quizId);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new QuizQuestion
                    {
                        QuestionId = (int)reader["QuestionId"],
                        QuizId = (int)reader["QuizId"],
                        QuestionText = reader["QuestionText"].ToString(),
                        OptionA = reader["OptionA"].ToString(),
                        OptionB = reader["OptionB"].ToString(),
                        OptionC = reader["OptionC"].ToString(),
                        OptionD = reader["OptionD"].ToString(),
                        CorrectOption = reader["CorrectOption"].ToString()
                    });
                }
            }
            return list;
        }

        public void InsertAttempt(int userId, int quizId, int score, int timeTakenSeconds)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_QuizAttempts_FP_RM_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@QuizId", quizId);
                cmd.Parameters.AddWithValue("@Score", score);
                cmd.Parameters.AddWithValue("@TimeTakenSeconds", timeTakenSeconds);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<QuizAttempt> GetAttemptsForUser(int userId)
        {
            List<QuizAttempt> list = new List<QuizAttempt>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_QuizAttempts_FP_RM_GetForUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new QuizAttempt
                    {
                        AttemptId = (int)reader["AttemptId"],
                        QuizId = (int)reader["QuizId"],
                        QuizTitle = reader["QuizTitle"].ToString(),
                        Score = (int)reader["Score"],
                        TimeTakenSeconds = (int)reader["TimeTakenSeconds"],
                        CompletedAt = (DateTime)reader["CompletedAt"]
                    });
                }
            }
            return list;
        }

        public List<LeaderboardEntry> GetLeaderboard(int quizId, int top = 10)
        {
            List<LeaderboardEntry> list = new List<LeaderboardEntry>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_QuizAttempts_FP_RM_GetLeaderboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@QuizId", quizId);
                cmd.Parameters.AddWithValue("@Top", top);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new LeaderboardEntry
                    {
                        Username = reader["Username"].ToString(),
                        Score = (int)reader["Score"],
                        TimeTakenSeconds = (int)reader["TimeTakenSeconds"],
                        CompletedAt = (DateTime)reader["CompletedAt"]
                    });
                }
            }
            return list;
        }
    }
}