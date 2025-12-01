using patrimonio_digital.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;

namespace patrimonio_digital.MVVM.ViewModel
{
    public class AuditoriaViewModel
    {
        public ObservableCollection<AuditoriaModel> Registros { get; } = new();

        private readonly string pastaDesktop = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            "digiPat");

        private readonly string caminho;

        public AuditoriaViewModel()
        {
            caminho = Path.Combine(pastaDesktop, "auditoria.json");
            CarregarAuditoria(); 
        }

        public void RegistrarAuditoria(AuditoriaModel registro)
        {
            Registros.Add(registro);
        }

        public void SalvarAuditoria()
        {
            try
            {
                Directory.CreateDirectory(pastaDesktop);

                var lista = Registros.ToList();
                var json = JsonSerializer.Serialize(lista, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(caminho, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Falha ao salvar auditoria: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void CarregarAuditoria()
        {
            if (!File.Exists(caminho))
                return;

            try
            {
                var json = File.ReadAllText(caminho);
                var lista = JsonSerializer.Deserialize<List<AuditoriaModel>>(json) ?? new List<AuditoriaModel>();

                Registros.Clear();
                foreach (var item in lista)
                    Registros.Add(item);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Falha ao carregar auditoria: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

}
