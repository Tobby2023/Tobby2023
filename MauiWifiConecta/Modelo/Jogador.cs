namespace MauiWifiConecta.Modelo
{
    public class Jogador
    {
        public int Codigo { get; set; }
        public string Nome { get; set; } = string.Empty;
        public Color Cor { get; set; } = Color.FromRgb(0,0,0);
        public bool SuaVez { get; set; } = false;

    }
}
