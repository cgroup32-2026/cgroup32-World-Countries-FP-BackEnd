namespace CountriesProject.BL
{
    public class QuizQuestionForPlayer
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
    }

    public class QuizForPlayer
    {
        public int QuizId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TimeLimitSeconds { get; set; }
        public List<QuizQuestionForPlayer> Questions { get; set; }
    }

    public class AnswerInput
    {
        public int QuestionId { get; set; }
        public string SelectedOption { get; set; }
    }

    public class QuestionResult
    {
        public int QuestionId { get; set; }
        public bool WasCorrect { get; set; }
        public string CorrectOption { get; set; }
    }

    public class QuizSubmissionResult
    {
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public int TimeTakenSeconds { get; set; }
        public List<QuestionResult> Results { get; set; }
    }
}