using Lab1_Vardzar_2_4.ViewModels;

namespace Lab1_Vardzar_2_4
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new StudentViewModel();
        }
    }
}
