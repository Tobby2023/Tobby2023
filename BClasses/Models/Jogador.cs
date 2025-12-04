using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BClasses.Models
{
    public class Jogador
    {
        public int Id { get; set; }
        public List<Carta> Cartas { get; set; }
        public Jogador(int id) {  Id = id; Cartas = new List<Carta>(); }

        public void DarCarta(Carta carta)
        {
            if (Cartas.FirstOrDefault(c => c.Id == carta.Id) == null)
            {
                Cartas.Add(carta);
            }
        }
    }
}
