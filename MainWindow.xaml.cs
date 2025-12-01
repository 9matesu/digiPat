using patrimonio_digital.MVVM.Model;
using patrimonio_digital.MVVM.View;
using patrimonio_digital.Utils;
using patrimonio_digital.MVVM.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;


namespace patrimonio_digital
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Loaded += MainWindow_Loaded;
            this.Closing += MainWindow_Closing;
        }


        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ItemStorage.CarregarInstituicao()))
            {
                AbrirCadastroInstituicao();
            }
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (DataContext is MainViewModel vm)
                vm.OnClose();
        }

        private bool isDark = true;

        private void ToggleTheme_Click(object sender, RoutedEventArgs e)
        {
            isDark = !isDark;
            var theme = isDark ? "Theme/Dark.xaml" : "Theme/Light.xaml";
            ThemeManager.ApplyTheme(theme);
        }

        private void AbrirCadastroInstituicao()
        {
            var cadastroWindow = new CadastrarInstituicao();
            cadastroWindow.Owner = this;
            cadastroWindow.DataContext = this.DataContext;
            cadastroWindow.ShowDialog();
        }


    }
}
