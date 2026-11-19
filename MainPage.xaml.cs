using System.Diagnostics;
using Lab1_Vardzar_2_4.Services;
using Lab1_Vardzar_2_4.ViewModels;

namespace Lab1_Vardzar_2_4
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            var vm = new StudentViewModel();
            BindingContext = vm;
            DeepLinkHandler.Main = vm;   
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is StudentViewModel vm)
                vm.LoadCommand.Execute(null);

            Debug.WriteLine("[PAGE] MainPage з'явилась");

           
            MainThread.BeginInvokeOnMainThread(async () => await DeepLinkHandler.TryProcessAsync());
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Debug.WriteLine("[PAGE] MainPage зникла");
        }
    }
}