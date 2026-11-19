using System.Diagnostics;
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

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is StudentViewModel vm)
                vm.LoadCommand.Execute(null);

            Debug.WriteLine("[PAGE] MainPage з'явилась");
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Debug.WriteLine("[PAGE] MainPage зникла");
        }
    }
}