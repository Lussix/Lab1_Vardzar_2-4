using SQLite;

namespace Lab1_Vardzar_2_4.Models
{
    [Table("Students")]
    public class Student
    {
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string FullName { get; set; } = string.Empty;
        public string Group { get; set; } = string.Empty;
        public double AverageScore { get; set; }
    }
}