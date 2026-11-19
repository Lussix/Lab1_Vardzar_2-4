using Lab1_Vardzar_2_4.ViewModels;

namespace Lab1_Vardzar_2_4.Views
{
    public partial class StudentDetailPage : ContentPage
    {
        public StudentDetailPage(StudentDetailViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}