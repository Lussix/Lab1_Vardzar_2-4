using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Lab1_Vardzar_2_4.Models;
using Lab1_Vardzar_2_4.Services;

namespace Lab1_Vardzar_2_4.ViewModels
{
    public class StudentViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        private readonly IStudentRepository _repository;
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
        public ICommand LoadCommand { get; }
        public ICommand DeleteStudentCommand { get; }
        public ICommand OpenSettingsCommand { get; }

        public StudentViewModel(IStudentRepository repository)
        {
            _repository = repository;

            AddStudentCommand = new Command(async () => await AddStudentAsync(), CanAddStudent);
            OpenDetailsCommand = new Command<Student>(async (student) =>
                await OpenDetailsAsync(student));
            LoadCommand = new Command(async () => await LoadAsync());
            DeleteStudentCommand = new Command<Student>(async (student) =>
                await DeleteStudentAsync(student));
            OpenSettingsCommand = new Command(async () =>
                await Shell.Current.GoToAsync("settings"));
        }

        public async Task LoadAsync()
        {
            var items = await _repository.GetAllAsync();


            if (!Preferences.Get("db_seeded", false))
            {
                var demo = new Student
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    FullName = "Іваненко Іван",
                    Group = "ІТ-21",
                    AverageScore = 85
                };
                await _repository.SaveAsync(demo);
                Preferences.Set("db_seeded", true);
                items = await _repository.GetAllAsync();
            }

            Students.Clear();
            foreach (var s in items)
                Students.Add(s);

            Debug.WriteLine($"[VM] LoadAsync: з бази прочитано {Students.Count} студентів");
        }

        private async Task AddStudentAsync()
        {
            var student = new Student
            {
                FullName = FullName,
                Group = Group,
                AverageScore = AverageScore
            };

            await _repository.SaveAsync(student);   
            Students.Add(student);                  
            FullName = string.Empty;
            Group = string.Empty;
        }

        private async Task DeleteStudentAsync(Student student)
        {
            if (student is null) return;

            await _repository.DeleteAsync(student);
            Students.Remove(student);
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

        public event PropertyChangedEventHandler? PropertyChanged;

      
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("UpdatedStudent", out var u) && u is Student updated)
            {
                _ = UpdateStudentAsync(updated);
                query.Clear();  
            }
        }

        private async Task UpdateStudentAsync(Student updated)
        {
            await _repository.SaveAsync(updated);  

            var existing = Students.FirstOrDefault(s => s.Id == updated.Id);
            if (existing is null) return;

            var index = Students.IndexOf(existing);
            Students[index] = updated;              
        }

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}