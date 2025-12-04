namespace MauiWifiConecta.Modelo
{
    public class Dados
    {
        public int Dados1 { get; set; } = 0;
        public int Dados2 { get; set; } = 0;
        public int IsContagem10 { get; set; } = 0;
        public int IsContagem20 { get; set; } = 0;

        public void Rolardado()
        {
            Random random = new Random();
            if ((IsContagem10 <= 0) || (IsContagem20 <= 0))
            {
                if (Dados1 == Dados2)
                {
                    Dados1 = random.Next(1, 7);
                    Dados2 = random.Next(1, 7);
                }
                else if (Dados1 < 0 || Dados2 < 0)
                {
                    if (Dados1 < 0)
                    {
                        Dados1 = random.Next(1, 7);
                    }
                    if (Dados2 < 0)
                    {
                        Dados2 = random.Next(1, 7);
                    }
                }
            }
        }
    }

    public class Nante
    {
        public string Peca { get; set; } = string.Empty;
        public Color Color { get; set; } = Colors.DarkGreen;
        public int Casa { get; set; }
        public int Passos { get; set; } = 0;


        public void Andar(List<NanteCaminho> caminhos, int n, Dados dados)
        {
            try
            {

                bool result = false;
                if ((Casa < 16) && (n > 16)) { result = true; }
                else if ((Casa < 46) && (n > 46)) { result = true; }
                else if ((Casa < 76) && (n > 76)) { result = true; }
                else if ((Casa < 106) && (n > 106)) { result = true; }

                if (n >= caminhos.Count) { n -= caminhos.Count; }
                if ((Passos < 75) && ((caminhos[n].Color == Colors.LightYellow || caminhos[n].Color == Colors.GreenYellow) || result))
                {
                    n += 10;
                }
                if (n >= caminhos.Count) { n -= caminhos.Count; }

                //App.Current!.MainPage!.DisplayAlert("Trapaceou", "Passos: " + Passos + "\nPosição: " + n + "\nPosição Atual: " + Casa, "Ok");
                caminhos[Casa].RetirarNante(this, 0);
                caminhos[n].ColocarNante(this, n, dados);
            }
            catch (Exception ex)
            {
                App.Current!.MainPage!.DisplayAlert("Erro", ex.Message, "Ok");
            }
        }
    }

    public class NanteCaminho
    {
        public int Linha { get; set; }
        public int Coluna { get; set; }
        public string Valor { get; set; } = string.Empty;
        public Color Color { get; set; } = Colors.White;
        public List<Nante> Nantes { get; set; } = new();

        public void ColocarNante(Nante nante, int pos)
        {
            this.Nantes.Add(nante);
            nante.Casa = pos;
        }

        public void ColocarNante(Nante nante, int pos, Dados dados)
        {
            for (int n = 0; n < Nantes.Count; n++)
            {
                if ((Nantes[n].Color != nante.Color) && ((Valor != "F") && (Valor != "FICHA")))
                {
                    RetirarNante(Nantes[n], 0);
                    dados.IsContagem20++;
                }
            }
            Nantes.Add(nante);
            nante.Casa = pos;
        }

        public void RetirarNante(Nante nante, int pos)
        {
            this.Nantes.Remove(nante);
            nante.Casa = pos;
        }


        public static List<NanteCaminho> NanteCaminhos(Color color)
        {
            List<NanteCaminho> caminhos =
            [
                new() { Linha = 12, Coluna = 5, Valor = "F", Color = Colors.LightGreen }, // 0
                new() { Linha = 12, Coluna = 6, Valor = "1", Color = color },
                new() { Linha = 12, Coluna = 7, Valor = "2", Color = color },
                new() { Linha = 12, Coluna = 8, Valor = "3", Color = color },
                new() { Linha = 12, Coluna = 9, Valor = "4", Color = color },
                new() { Linha = 12, Coluna = 10, Valor = "5", Color = color },
                new() { Linha = 13, Coluna = 10, Valor = "6", Color = color },
                new() { Linha = 14, Coluna = 10, Valor = "7", Color = color },
                new() { Linha = 15, Coluna = 10, Valor = "8", Color = color },
                new() { Linha = 16, Coluna = 10, Valor = "9", Color = color },
                new() { Linha = 17, Coluna = 10, Valor = "F", Color = Colors.LightGreen },
                new() { Linha = 18, Coluna = 10, Valor = "11", Color = color },
                new() { Linha = 19, Coluna = 10, Valor = "12", Color = color },
                new() { Linha = 20, Coluna = 10, Valor = "13", Color = color },
                new() { Linha = 21, Coluna = 10, Valor = "14", Color = color },
                new() { Linha = 21, Coluna = 11, Valor = "FICHA", Color = color },
                #region "[ENTRADA 1]"
                new() { Linha = 20, Coluna = 11, Valor = "I", Color = Colors.LightYellow },
                new() { Linha = 19, Coluna = 11, Valor = "II", Color = Colors.LightYellow },
                new() { Linha = 18, Coluna = 11, Valor = "III", Color = Colors.LightYellow },
                new() { Linha = 17, Coluna = 11, Valor = "IV", Color = Colors.LightYellow },
                new() { Linha = 16, Coluna = 11, Valor = "V", Color = Colors.LightYellow },
                new() { Linha = 15, Coluna = 11, Valor = "VI", Color = Colors.LightYellow },
                new() { Linha = 14, Coluna = 11, Valor = "VII", Color = Colors.LightYellow },
                new() { Linha = 13, Coluna = 11, Valor = "VIII", Color = Colors.LightYellow },
                new() { Linha = 12, Coluna = 11, Valor = "IX", Color = Colors.LightYellow },
                new() { Linha = 11, Coluna = 11, Valor = "X", Color = Colors.GreenYellow },
                #endregion
                new() { Linha = 21, Coluna = 12, Valor = "16", Color = color },
                new() { Linha = 20, Coluna = 12, Valor = "17", Color = color },
                new() { Linha = 19, Coluna = 12, Valor = "18", Color = color },
                new() { Linha = 18, Coluna = 12, Valor = "19", Color = color },


                new() { Linha = 17, Coluna = 12, Valor = "F", Color = Colors.LightGreen }, // 30
                new() { Linha = 16, Coluna = 12, Valor = "1", Color = color },
                new() { Linha = 15, Coluna = 12, Valor = "2", Color = color },
                new() { Linha = 14, Coluna = 12, Valor = "3", Color = color },
                new() { Linha = 13, Coluna = 12, Valor = "4", Color = color },
                new() { Linha = 12, Coluna = 12, Valor = "5", Color = color },
                new() { Linha = 12, Coluna = 13, Valor = "6", Color = color },
                new() { Linha = 12, Coluna = 14, Valor = "7", Color = color },
                new() { Linha = 12, Coluna = 15, Valor = "8", Color = color },
                new() { Linha = 12, Coluna = 16, Valor = "9", Color = color },
                new() { Linha = 12, Coluna = 17, Valor = "F", Color = Colors.LightGreen },
                new() { Linha = 12, Coluna = 18, Valor = "11", Color = color },
                new() { Linha = 12, Coluna = 19, Valor = "12", Color = color },
                new() { Linha = 12, Coluna = 20, Valor = "13", Color = color },
                new() { Linha = 12, Coluna = 21, Valor = "14", Color = color },
                new() { Linha = 11, Coluna = 21, Valor = "FICHA", Color = color },
                #region "[ENTRADA 2]"
                new() { Linha = 11, Coluna = 20, Valor = "I", Color = Colors.LightYellow },
                new() { Linha = 11, Coluna = 19, Valor = "II", Color = Colors.LightYellow },
                new() { Linha = 11, Coluna = 18, Valor = "III", Color = Colors.LightYellow },
                new() { Linha = 11, Coluna = 17, Valor = "IV", Color = Colors.LightYellow },
                new() { Linha = 11, Coluna = 16, Valor = "V", Color = Colors.LightYellow },
                new() { Linha = 11, Coluna = 15, Valor = "VI", Color = Colors.LightYellow },
                new() { Linha = 11, Coluna = 14, Valor = "VII", Color = Colors.LightYellow },
                new() { Linha = 11, Coluna = 13, Valor = "VIII", Color = Colors.LightYellow },
                new() { Linha = 11, Coluna = 12, Valor = "IX", Color = Colors.LightYellow },
                new() { Linha = 11, Coluna = 11, Valor = "X", Color = Colors.GreenYellow },
                #endregion
                new() { Linha = 10, Coluna = 21, Valor = "16", Color = color },
                new() { Linha = 10, Coluna = 20, Valor = "17", Color = color },
                new() { Linha = 10, Coluna = 19, Valor = "18", Color = color },
                new() { Linha = 10, Coluna = 18, Valor = "19", Color = color },


                new() { Linha = 10, Coluna = 17, Valor = "F", Color = Colors.LightGreen }, // 60
                new() { Linha = 10, Coluna = 16, Valor = "1", Color = color },
                new() { Linha = 10, Coluna = 15, Valor = "2", Color = color },
                new() { Linha = 10, Coluna = 14, Valor = "3", Color = color },
                new() { Linha = 10, Coluna = 13, Valor = "4", Color = color },
                new() { Linha = 10, Coluna = 12, Valor = "5", Color = color },
                new() { Linha = 9, Coluna = 12, Valor = "6", Color = color },
                new() { Linha = 8, Coluna = 12, Valor = "7", Color = color },
                new() { Linha = 7, Coluna = 12, Valor = "8", Color = color },
                new() { Linha = 6, Coluna = 12, Valor = "9", Color = color },
                new() { Linha = 5, Coluna = 12, Valor = "F", Color = Colors.LightGreen },
                new() { Linha = 4, Coluna = 12, Valor = "11", Color = color },
                new() { Linha = 3, Coluna = 12, Valor = "12", Color = color },
                new() { Linha = 2, Coluna = 12, Valor = "13", Color = color },
                new() { Linha = 1, Coluna = 12, Valor = "14", Color = color },
                new() { Linha = 1, Coluna = 11, Valor = "FICHA", Color = color },
                #region "[ENTRADA 3]"
                new() { Linha = 2, Coluna = 11, Valor = "I", Color = Colors.LightYellow },
                new() { Linha = 3, Coluna = 11, Valor = "II", Color = Colors.LightYellow },
                new() { Linha = 4, Coluna = 11, Valor = "III", Color = Colors.LightYellow },
                new() { Linha = 5, Coluna = 11, Valor = "IV", Color = Colors.LightYellow },
                new() { Linha = 6, Coluna = 11, Valor = "V", Color = Colors.LightYellow },
                new() { Linha = 7, Coluna = 11, Valor = "VI", Color = Colors.LightYellow },
                new() { Linha = 8, Coluna = 11, Valor = "VII", Color = Colors.LightYellow },
                new() { Linha = 9, Coluna = 11, Valor = "VIII", Color = Colors.LightYellow },
                new() { Linha = 10, Coluna = 11, Valor = "IX", Color = Colors.LightYellow },
                new() { Linha = 11, Coluna = 11, Valor = "X", Color = Colors.GreenYellow },
                #endregion
                new() { Linha = 1, Coluna = 10, Valor = "16", Color = color },
                new() { Linha = 2, Coluna = 10, Valor = "17", Color = color },
                new() { Linha = 3, Coluna = 10, Valor = "18", Color = color },
                new() { Linha = 4, Coluna = 10, Valor = "19", Color = color },


                new() { Linha = 5, Coluna = 10, Valor = "F", Color = Colors.LightGreen }, // 90
                new() { Linha = 6, Coluna = 10, Valor = "1", Color = color },
                new() { Linha = 7, Coluna = 10, Valor = "2", Color = color },
                new() { Linha = 8, Coluna = 10, Valor = "3", Color = color },
                new() { Linha = 9, Coluna = 10, Valor = "4", Color = color },
                new() { Linha = 10, Coluna = 10, Valor = "5", Color = color },
                new() { Linha = 10, Coluna = 9, Valor = "6", Color = color },
                new() { Linha = 10, Coluna = 8, Valor = "7", Color = color },
                new() { Linha = 10, Coluna = 7, Valor = "8", Color = color },
                new() { Linha = 10, Coluna = 6, Valor = "9", Color = color },
                new() { Linha = 10, Coluna = 5, Valor = "F", Color = Colors.LightGreen },
                new() { Linha = 10, Coluna = 4, Valor = "11", Color = color },
                new() { Linha = 10, Coluna = 3, Valor = "12", Color = color },
                new() { Linha = 10, Coluna = 2, Valor = "13", Color = color },
                new() { Linha = 10, Coluna = 1, Valor = "14", Color = color },
                new() { Linha = 11, Coluna = 1, Valor = "FICHA", Color = color },
                #region "[ENTRADA 4]"
                new() { Linha = 11, Coluna = 2, Valor = "I", Color = Colors.LightYellow },
                new() { Linha = 11, Coluna = 3, Valor = "II", Color = Colors.LightYellow },
                new() { Linha = 11, Coluna = 4, Valor = "III", Color = Colors.LightYellow },
                new() { Linha = 11, Coluna = 5, Valor = "IV", Color = Colors.LightYellow },
                new() { Linha = 11, Coluna = 6, Valor = "V", Color = Colors.LightYellow },
                new() { Linha = 11, Coluna = 7, Valor = "VI", Color = Colors.LightYellow },
                new() { Linha = 11, Coluna = 8, Valor = "VII", Color = Colors.LightYellow },
                new() { Linha = 11, Coluna = 9, Valor = "VIII", Color = Colors.LightYellow },
                new() { Linha = 11, Coluna = 10, Valor = "IX", Color = Colors.LightYellow },
                new() { Linha = 11, Coluna = 11, Valor = "X", Color = Colors.GreenYellow },
                #endregion
                new() { Linha = 12, Coluna = 1, Valor = "16", Color = color },
                new() { Linha = 12, Coluna = 2, Valor = "17", Color = color },
                new() { Linha = 12, Coluna = 3, Valor = "18", Color = color },
                new() { Linha = 12, Coluna = 4, Valor = "19", Color = color },
            ];
            return caminhos;
        }
    }
}
