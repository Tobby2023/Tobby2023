using System.Drawing;

namespace BClasses.Models
{
    public class Carta
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public Color Color { get; set; } = Color.Yellow;
        public Jogador? Jogador { get; set; }

        public Carta(int id, string name, Color color, Jogador? jogador = null)
        {
            Name = name;
            Color = color;
            Jogador = jogador;
        }

        public void DarJogador(Jogador jogador)
        {
            jogador.DarCarta(this);
        }
    }
}
