namespace CountriesProject.DAL.Models
{
    public class Quiz
    {
        public int QuizId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TimeLimitSeconds { get; set; }
        public int QuestionCount { get; set; }
    }


    

}