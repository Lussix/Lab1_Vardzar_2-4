using System.ComponentModel;
using System.Windows.Input;
using Lab1_Vardzar_2_4.Models;

namespace Lab1_Vardzar_2_4.ViewModels
{
    public class StudentDetailViewModel : IQueryAttributable, INotifyPropertyChanged
    {
        private Student? _original;
        private double _averageScore;

        public string FullName { get; private set; } = string.Empty;
        public string Group { get; private set; } = string.Empty;

        public double AverageScore
        {
            get => _averageScore;
            set
            {
                if (_averageScore != value)
                {
                    _averageScore = value;
                    OnPropertyChanged(nameof(AverageScore));
                }
            }
        }

        public ICommand GoBackCommand { get; }
        public ICommand SaveCommand { get; }

        public StudentDetailViewModel()
        {
            GoBackCommand = new Command(async () =>
                await Shell.Current.GoToAsync(".."));

            SaveCommand = new Command(async () => await SaveAsync());
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("SelectedStudent", out var value)
                && value is Student student)
            {
                _original = student;
                FullName = student.FullName;
                Group = student.Group;
                AverageScore = student.AverageScore;

                OnPropertyChanged(nameof(FullName));
                OnPropertyChanged(nameof(Group));
            }
        }

        private async Task SaveAsync()
        {
            if (_original is null) return;

            var updated = new Student
            {
                FullName = FullName,
                Group = Group,
                AverageScore = AverageScore
            };

            var parameters = new Dictionary<string, object>
            {
                { "OriginalStudent", _original },
                { "UpdatedStudent", updated }
            };

            await Shell.Current.GoToAsync("..", parameters);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}