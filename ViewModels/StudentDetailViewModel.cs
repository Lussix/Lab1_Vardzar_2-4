using System.ComponentModel;
using Lab1_Vardzar_2_4.Models;

namespace Lab1_Vardzar_2_4.ViewModels
{
    public class StudentDetailViewModel : IQueryAttributable, INotifyPropertyChanged
    {
        public string FullName { get; set; } = string.Empty;
        public string Group { get; set; } = string.Empty;
        public double AverageScore { get; set; }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("SelectedStudent", out var value)
                && value is Student student)
            {
                FullName = student.FullName;
                Group = student.Group;
                AverageScore = student.AverageScore;

                OnPropertyChanged(nameof(FullName));
                OnPropertyChanged(nameof(Group));
                OnPropertyChanged(nameof(AverageScore));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}