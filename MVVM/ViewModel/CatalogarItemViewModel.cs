using patrimonio_digital.Core;
using patrimonio_digital.MVVM.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace patrimonio_digital.MVVM.ViewModel
{
    public class CatalogarItemViewModel : ObservableObject
    {
        public AuditoriaViewModel AuditoriaVM { get; }
        public Window JanelaAtual { get; set; }
        public string UsuarioLogado { get; set; }

        private string _nomeNovoItem;
        public string NomeNovoItem
        {
            get => _nomeNovoItem;
            set { if (_nomeNovoItem != value) { _nomeNovoItem = value; OnPropertyChanged(); } }
        }

        private string _autorNovoItem;
        public string AutorNovoItem
        {
            get => _autorNovoItem;
            set { if (_autorNovoItem != value) { _autorNovoItem = value; OnPropertyChanged(); } }
        }

        private string _dataNovoItem;
        public string DataNovoItem
        {
            get => _dataNovoItem;
            set { if (_dataNovoItem != value) { _dataNovoItem = value; OnPropertyChanged(); } }
        }

        private string _origemNovoItem;
        public string OrigemNovoItem
        {
            get => _origemNovoItem;
            set { if (_origemNovoItem != value) { _origemNovoItem = value; OnPropertyChanged(); } }
        }

        private string _tipoNovoItem;
        public string TipoNovoItem
        {
            get => _tipoNovoItem;
            set { if (_tipoNovoItem != value) { _tipoNovoItem = value; OnPropertyChanged(); } }
        }

        private string _estadoConsNovoItem;
        public string EstadoConsNovoItem
        {
            get => _estadoConsNovoItem;
            set { if (_estadoConsNovoItem != value) { _estadoConsNovoItem = value; OnPropertyChanged(); } }
        }

        private string _setorFisicoNovoItem;
        public string SetorFisicoNovoItem
        {
            get => _setorFisicoNovoItem;
            set { if (_setorFisicoNovoItem != value) { _setorFisicoNovoItem = value; OnPropertyChanged(); } }
        }

        public ObservableCollection<Item> Itens { get; }

        // interfaces de comando
        public ICommand RegistrarCommand { get; }
        public ICommand FecharCommand { get; }

        private Item ItemEditando { get; }

        public ObservableCollection<string> EstadoCons { get; set; } = new ObservableCollection<string>
        {
            "Novo", "Bom", "Regular", "Ruim"
        };

        public ObservableCollection<string> Tipo { get; set; } = new ObservableCollection<string>
        {
            "Fotografia", "Declaração", "Boletim de Ocorrência", "Escritura"
        };

        public CatalogarItemViewModel(ObservableCollection<Item> itens, string usuarioLogado) :
            this(itens, usuarioLogado, null, null)
        {
        }

        public CatalogarItemViewModel(
                ObservableCollection<Item> itens,
                string usuario,
                AuditoriaViewModel auditoriaVM,
                Item item = null)
        {
            Itens = itens;
            UsuarioLogado = usuario;
            AuditoriaVM = auditoriaVM;
            ItemEditando = item;
            FecharCommand = new RelayCommand(j => (j as Window)?.Close());
            RegistrarCommand = new RelayCommand(_ => RegistrarItem());


            if (item != null)
            {
                NomeNovoItem = item.Nome;
                AutorNovoItem = item.Autor;
                DataNovoItem = item.Data;
                OrigemNovoItem = item.Origem;
                TipoNovoItem = item.Tipo;
                EstadoConsNovoItem = item.EstadoCons;
                SetorFisicoNovoItem = item.SetorFisico;
            }
        }

        private void RegistrarItem()
        {
            if (string.IsNullOrWhiteSpace(NomeNovoItem)) return;

            if (ItemEditando == null)
            {
                var novo = new Item
                {
                    Nome = NomeNovoItem,
                    Autor = AutorNovoItem,
                    Data = DataNovoItem,
                    Origem = OrigemNovoItem,
                    Tipo = TipoNovoItem,
                    EstadoCons = EstadoConsNovoItem,
                    SetorFisico = SetorFisicoNovoItem
                };

                Itens.Add(novo);

                AuditoriaVM?.RegistrarAuditoria(new AuditoriaModel
                {
                    DataHora = DateTime.Now,
                    Usuario = UsuarioLogado,
                    Acao = "Adição de Item",
                    Item = NomeNovoItem
                });
            }
            else
            {
                ItemEditando.Nome = NomeNovoItem;
                ItemEditando.Autor = AutorNovoItem;
                ItemEditando.Data = DataNovoItem;
                ItemEditando.Origem = OrigemNovoItem;
                ItemEditando.Tipo = TipoNovoItem;
                ItemEditando.EstadoCons = EstadoConsNovoItem;
                ItemEditando.SetorFisico = SetorFisicoNovoItem;

                AuditoriaVM?.RegistrarAuditoria(new AuditoriaModel
                {
                    DataHora = DateTime.Now,
                    Usuario = UsuarioLogado,
                    Acao = "Edição de Item",
                    Item = NomeNovoItem
                });
            }

            NomeNovoItem = "";
            AutorNovoItem = "";
            DataNovoItem = "";
            OrigemNovoItem = "";
            TipoNovoItem = "";
            EstadoConsNovoItem = "";
            SetorFisicoNovoItem = "";
        }

    }
}
