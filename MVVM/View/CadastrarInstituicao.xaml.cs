using patrimonio_digital.MVVM.ViewModel;
using patrimonio_digital.MVVM.Model;
using patrimonio_digital.MVVM.View;
using patrimonio_digital.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace patrimonio_digital.MVVM.View
{
    /// <summary>
    /// Lógica interna para CadastrarInstituicao.xaml
    /// </summary>
    public partial class CadastrarInstituicao : Window
    {
        public CadastrarInstituicao()
        {
            InitializeComponent();
            DataContext = new CadastrarInstituicaoViewModel();
        }

        private void BtnConcluir_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Digite um nome válido.");
                return;
            }


            if (this.DataContext is MainViewModel vm)
            {
                vm.Instituicao = txtNome.Text;
                ItemStorage.SalvarInstituicao(vm.Instituicao);
            }
            this.Close();
        }

            
        }
    }

