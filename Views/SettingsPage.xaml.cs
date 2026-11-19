using Lab1_Vardzar_2_4.ViewModels;

namespace Lab1_Vardzar_2_4.Views
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage(SettingsViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}