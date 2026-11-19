using System.ComponentModel;
using Lab1_Vardzar_2_4.Services;

namespace Lab1_Vardzar_2_4.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private bool _isDark = Application.Current?.UserAppTheme == AppTheme.Dark;

        public bool IsDark
        {
            get => _isDark;
            set
            {
                if (_isDark == value) return;

                _isDark = value;
                OnPropertyChanged(nameof(IsDark));
                ThemeService.Apply(value ? AppTheme.Dark : AppTheme.Light);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}