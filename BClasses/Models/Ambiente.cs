using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace BClasses.Models
{
    public class Ambiente
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public DateTime Data { get; set; } = DateTime.Now;
        public List<string> Baralho { get; set; }
        public List<int> Jogadores { get; set; }

        public Ambiente()
        {
            Data = DateTime.Now;
            Baralho = new List<string>();
            Jogadores = new List<int>();
            Baralhar();
        }

        public void DarCartaJogador(int code)
        {
            Random random = new Random();
            int pos = 0;
            do
            {
                pos = random.Next(0, Baralho.Count - 1);

            } while (Baralho[pos].EndsWith(",0") == false);

            Baralho[pos] = $"{Baralho[pos].Split(",")[0]},{Baralho[pos].Split(",")[1]},{Baralho[pos].Split(",")[2]},{code}";
        }

        public void AddJogador(int code) => Jogadores.Add(code);

        private void Baralhar()
        {
            foreach (string color in new List<string>() { "#FDD835", Color.Red.Name.ToLower(), Color.DodgerBlue.Name.ToLower(), "#43A047" })
            {
                Baralho.Add($"0,0,{color},0");
                for (int i = 1; i < 18; i++)
                {
                    if (i == 17)
                    {
                        Baralho.Add($"{i},+4,{color},0");
                    }
                    else if (i == 16)
                    {
                        Baralho.Add($"{i},Color,{color},0");
                    }
                    else if ((i == 14) || (i == 15))
                    {
                        Baralho.Add($"{i},+2,{color},0");
                    }
                    else if ((i == 12) || (i == 13))
                    {
                        Baralho.Add($"{i},Inverte,{color},0");
                    }
                    else if ((i == 10) || (i == 11))
                    {
                        Baralho.Add($"{i},Pular,{color},0");
                    }
                    else
                    {
                        Baralho.Add($"{i},{i},{color},0");
                        i++;
                        Baralho.Add($"{i},{(i - 1)},{color},0");
                        i++;
                        i -= 2;
                    }
                }
            }
        }
    }
}
