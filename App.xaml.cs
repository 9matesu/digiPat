using patrimonio_digital.MVVM.Model;
using patrimonio_digital.MVVM.View;
using patrimonio_digital.MVVM.ViewModel;
using System.Windows;

namespace patrimonio_digital
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var auditoriaVM = new AuditoriaViewModel();
            auditoriaVM.CarregarAuditoria();
            IniciarApp();

        }

        public void IniciarApp()
        {
            var loginWindow = new Login();
            MainWindow = loginWindow; // DEFINA ANTES
            bool? result = loginWindow.ShowDialog();

            if (result == true)
            {
                try
                {
                    var mainWindow = new MainWindow();
                    MainWindow = mainWindow;
                    mainWindow.Show();
                }
                catch
                {
                    Shutdown();
                }
            }
            else
            {
                Shutdown();
            }
        }
    }
}