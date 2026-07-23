namespace CountriesProject.DAL.Models
{
    public class QuizAttempt
    {
        public int AttemptId { get; set; }
        public int QuizId { get; set; }
        public string QuizTitle { get; set; }
        public int Score { get; set; }
        public int TimeTakenSeconds { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}