using patrimonio_digital.MVVM.Model;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace patrimonio_digital.Utils
{
    public static class ItemStorage
    {

        private static readonly string pastaDesktop = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            "digiPat");

        private static readonly string arquivo = Path.Combine(pastaDesktop, "itens.json");
        private static readonly string arquivoInstituicao = Path.Combine(pastaDesktop, "instituicao.json");


        public static void Salvar(ObservableCollection<Item> itens)
        {

            if (!Directory.Exists(pastaDesktop))
                Directory.CreateDirectory(pastaDesktop);

            var json = JsonSerializer.Serialize(itens, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(arquivo, json);
        }

        public static ObservableCollection<Item> Carregar()
        {
            if (!File.Exists(arquivo)) return new ObservableCollection<Item>();
            var json = File.ReadAllText(arquivo);
            return JsonSerializer.Deserialize<ObservableCollection<Item>>(json) ?? new ObservableCollection<Item>();
        }

        public static void SalvarInstituicao(string instituicao)
        {
            if (!Directory.Exists(pastaDesktop))
                Directory.CreateDirectory(pastaDesktop);

            var json = JsonSerializer.Serialize(instituicao, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(arquivoInstituicao, json);
        }

        public static string CarregarInstituicao()
        {
            if (!File.Exists(arquivoInstituicao))
                return string.Empty;

            var json = File.ReadAllText(arquivoInstituicao);
            return JsonSerializer.Deserialize<string>(json) ?? string.Empty;
        }
    }
}
