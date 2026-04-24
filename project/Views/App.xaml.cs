using System.Windows;

namespace FootballLeague
{
    public partial class App : Application
    {
        [System.STAThread]
        public static void Main()
        {
            App app = new App();
            app.InitializeComponent();
            app.Run();
        }
    }
}
