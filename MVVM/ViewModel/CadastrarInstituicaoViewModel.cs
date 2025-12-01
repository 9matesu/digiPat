using patrimonio_digital.Core;
using patrimonio_digital.MVVM.Model;
using patrimonio_digital.MVVM.View;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace patrimonio_digital.MVVM.ViewModel
{
    public class CadastrarInstituicaoViewModel : ObservableObject
    {
        private MainViewModel _mainVM;

        public ICommand CadastrarInstituicaoCommand { get; set; }

        private string nomeInstituicao;
        public string NomeInstituicao
        {
            get => nomeInstituicao;
            set { nomeInstituicao = value; OnPropertyChanged(); }
        }

        public CadastrarInstituicaoViewModel(MainViewModel mainVM)
        {
            _mainVM = mainVM;
            CadastrarInstituicaoCommand = new RelayCommand(_ => CadastrarInstituicao());
        }

        private void CadastrarInstituicao()
        {
            if (string.IsNullOrWhiteSpace(NomeInstituicao)) return;

            _mainVM.Instituicao = NomeInstituicao;

            Application.Current.Windows
                .OfType<CadastrarInstituicao>()
                .FirstOrDefault()
                ?.Close();
        }
    }
}
