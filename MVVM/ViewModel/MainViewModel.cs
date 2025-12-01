using patrimonio_digital.Core;
using patrimonio_digital.MVVM.Model;
using patrimonio_digital.MVVM.View;
using patrimonio_digital.Utils;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace patrimonio_digital.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        public AuditoriaViewModel AuditoriaVM { get; }

        private Usuario _usuarioLogado;
        public Usuario UsuarioLogadoBool
        {
            get => _usuarioLogado;
            set
            {
                _usuarioLogado = value ?? throw new ArgumentNullException(nameof(value));
                OnPropertyChanged();
                OnPropertyChanged(nameof(PodeCatalogar));
                OnPropertyChanged(nameof(PodeExcluir));
                OnPropertyChanged(nameof(PodeGerenciarUsuarios));
                OnPropertyChanged(nameof(PodeAuditoria));
                OnPropertyChanged(nameof(PodeEditar));
            }
        }

        public bool PodeCatalogar => UsuarioLogadoBool != null &&
            (UsuarioLogadoBool.Tipo == TipoUsuario.Administrador || UsuarioLogadoBool.Tipo == TipoUsuario.Funcionario);

        public bool PodeExcluir => UsuarioLogadoBool != null &&
            UsuarioLogadoBool.Tipo == TipoUsuario.Administrador;

        public bool PodeGerenciarUsuarios => UsuarioLogadoBool != null &&
            UsuarioLogadoBool.Tipo == TipoUsuario.Administrador;

        public bool PodeAuditoria => UsuarioLogadoBool != null &&
            UsuarioLogadoBool.Tipo == TipoUsuario.Administrador;

        public bool PodeEditar => UsuarioLogadoBool != null &&
            UsuarioLogadoBool.Tipo != TipoUsuario.Visitante;

        public ICommand LogoutCommand { get; }
        public ICommand AbrirJanelaCommand { get; }
        public ICommand FecharJanelaCommand { get; }
        public ICommand AbrirEditorCommand { get; }
        public ICommand EditarItemCommand { get; }
        public ICommand ExcluirItemCommand { get; }


        private string textoPesquisa;
        public string TextoPesquisa
        {
            get => textoPesquisa;
            set
            {
                if (textoPesquisa != value)
                {
                    textoPesquisa = value;
                    OnPropertyChanged();
                    ItensView.Refresh();
                }
            }
        }

        public ObservableCollection<Item> Itens { get; }
        public ICollectionView ItensView { get; }

        private string usuarioLogado;
        public string UsuarioLogado
        {
            get => usuarioLogado;
            set
            {
                usuarioLogado = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel(Usuario usuario, string nomeUsuario)
        {
            UsuarioLogado = nomeUsuario;
            UsuarioLogadoBool = usuario;
            AuditoriaVM = new AuditoriaViewModel();

            AbrirJanelaCommand = new RelayCommand(AbrirJanela);
            AbrirEditorCommand = new RelayCommand(AbrirEditor);
            EditarItemCommand = new RelayCommand(EditarItem);
            FecharJanelaCommand = new RelayCommand(FecharJanela);
            LogoutCommand = new RelayCommand(_ => Logout(null));
            ExcluirItemCommand = new RelayCommand(ExcluirItem);

            Itens = ItemStorage.Carregar();
            ItensView = CollectionViewSource.GetDefaultView(Itens);
            ItensView.Filter = FiltrarItens;
        }

        private bool FiltrarItens(object obj)
        {
            if (obj is not Item item) return false;
            if (string.IsNullOrWhiteSpace(TextoPesquisa)) return true;

            string termo = TextoPesquisa.Trim().ToLower();

            return (item.Nome?.ToLower().Contains(termo) == true) ||
                   (item.Autor?.ToLower().Contains(termo) == true) ||
                   (item.Origem?.ToLower().Contains(termo) == true) ||
                   (item.Tipo?.ToLower().Contains(termo) == true) ||
                   (item.Data?.ToLower().Contains(termo) == true) ||
                   (item.EstadoCons?.ToLower().Contains(termo) == true) ||
                   (item.SetorFisico?.ToLower().Contains(termo) == true);
        }

        private void AbrirJanela(object parameter)
        {
            if (parameter is string tipoJanela)
            {
                if (tipoJanela == "Usuarios" && !PodeGerenciarUsuarios) return;
                if (tipoJanela == "Auditoria" && !PodeAuditoria) return;

                Window janela = tipoJanela switch
                {
                    "Catalogar" when PodeCatalogar => new CatalogarItemWindow
                    {
                        DataContext = new CatalogarItemViewModel(Itens, UsuarioLogado, AuditoriaVM)
                    },
                    "Auditoria" when PodeAuditoria => new Auditoria { DataContext = AuditoriaVM },
                    "Usuarios" when PodeGerenciarUsuarios => new Usuarios(),
                    "Sobre" => new SobreWindow(),
                    "Login" => new Login(),
                    _ => null
                };

                janela?.Show();
            }
        }

        public void FecharJanela(object parameter)
        {
            if (parameter is Window janela)
                janela.Close();
        }


        private void ExcluirItem(object parameter)
        {
            if (!PodeExcluir)
            {
                MessageBox.Show("Acesso negado: você não tem permissão para excluir itens.");
                return;
            }

            if (parameter is Item item && Itens.Contains(item))
            {
                var resultado = MessageBox.Show(
                    $"Tem certeza que deseja excluir o item \"{item.Nome}\"?",
                    "Confirmar exclusão",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (resultado == MessageBoxResult.Yes)
                {
                    Itens.Remove(item);

                    AuditoriaVM.RegistrarAuditoria(new AuditoriaModel
                    {
                        DataHora = DateTime.Now,
                        Usuario = UsuarioLogado,
                        Acao = "Exclusão de Item",
                        Item = item.Nome,
                    });
                }
            }
        }

        private void EditarItem(object parameter)
        {
            if (parameter is Item item)
            {
                var janela = new CatalogarItemWindow
                {
                    DataContext = new CatalogarItemViewModel(Itens, UsuarioLogado, AuditoriaVM, item)
                };

                janela.ShowDialog();
            }
        }

        private void AbrirEditor(object parameter)
        {
            
            if (parameter is Item item)
            {
                if (PodeEditar)
                {
                    var editor = new EditorDocumentoView(item);
                    editor.ShowDialog();

                }
                else
                {
                    var visualizador = new VisualizadorDocumentoView(item);
                    visualizador.DataContext = new VisualizadorDocumentoViewModel(item);
                    visualizador.ShowDialog();
                }
            }
        }

        public void Logout(object? obj)
        {
            foreach (Window w in Application.Current.Windows.Cast<Window>().ToList())
                w.Close();

            ((App)Application.Current).IniciarApp();
        }
        public void OnClose()
        {
            ItemStorage.Salvar(Itens);
            AuditoriaVM.SalvarAuditoria();
        }
    }
}
