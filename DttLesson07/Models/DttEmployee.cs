namespace DttLesson07.Models
{
    public class DttEmployee
    {
        public int DttId { get; set; }
        public required string DttName { get; set; }
        public DateTime DttBirthDay { get; set; }
        public string DttEmail { get; set; }
        public string DttPhone { get; set; }
        public double DttSalary { get; set; }
        public string DttStatus { get; set; }
    }
}
