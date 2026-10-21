using Lab1_Vardzar_2_4.Views;

namespace Lab1_Vardzar_2_4
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("studentdetail", typeof(StudentDetailPage));
        }
    }
}