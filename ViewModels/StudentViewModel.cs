using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Lab1_Vardzar_2_4.Models;

namespace Lab1_Vardzar_2_4.ViewModels
{
    public class StudentViewModel : INotifyPropertyChanged, IQueryAttributable 
    {
        private Student _student = new();

        public string FullName
        {
            get => _student.FullName;
            set
            {
                if (_student.FullName != value)
                {
                    _student.FullName = value;
                    OnPropertyChanged(nameof(FullName));
                    ((Command)AddStudentCommand).ChangeCanExecute();
                }
            }
        }

        public string Group
        {
            get => _student.Group;
            set
            {
                if (_student.Group != value)
                {
                    _student.Group = value;
                    OnPropertyChanged(nameof(Group));
                }
            }
        }

        public double AverageScore
        {
            get => _student.AverageScore;
            set
            {
                if (_student.AverageScore != value)
                {
                    _student.AverageScore = value;
                    OnPropertyChanged(nameof(AverageScore));
                }
            }
        }

        public ObservableCollection<Student> Students { get; } = new();

        public ICommand AddStudentCommand { get; }
        public ICommand OpenDetailsCommand { get; }   

        public StudentViewModel()
        {
            AddStudentCommand = new Command(AddStudent, CanAddStudent);
            OpenDetailsCommand = new Command<Student>(async (student) =>
                await OpenDetailsAsync(student));      
        }

        private void AddStudent()
        {
            Students.Add(new Student
            {
                FullName = FullName,
                Group = Group,
                AverageScore = AverageScore
            });

            FullName = string.Empty;
            Group = string.Empty;
        }

        private bool CanAddStudent() => !string.IsNullOrWhiteSpace(FullName);
        private async Task OpenDetailsAsync(Student student)
        {
            if (student is null) return;

            var parameters = new Dictionary<string, object>
            {
                { "SelectedStudent", student }
            };

            await Shell.Current.GoToAsync("studentdetail", parameters);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("OriginalStudent", out var o) && o is Student original
                && query.TryGetValue("UpdatedStudent", out var u) && u is Student updated)
            {
                var index = Students.IndexOf(original);
                if (index >= 0)
                {
                    Students[index] = updated;
                }
            }
        }

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}