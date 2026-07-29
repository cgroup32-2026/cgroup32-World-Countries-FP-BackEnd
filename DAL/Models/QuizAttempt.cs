namespace CountriesProject.DAL.Models
{
    //this is simply for recording history of quizzes, hence its called attempt. its not the normal quiz class. i cant add
    // data completed and timetaken in the normal quize class because being completed is not related to its state - raslan
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