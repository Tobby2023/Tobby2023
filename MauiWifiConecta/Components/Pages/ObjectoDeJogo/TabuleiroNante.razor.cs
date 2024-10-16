using MauiWifiConecta.Modelo;

namespace MauiWifiConecta.Components.Pages.ObjectoDeJogo
{
    public partial class TabuleiroNante
    {
        public Dados Dados { get; set; } = new();
        List<NanteCaminho> Caminhos { get; set; } = NanteCaminho.NanteCaminhos(Colors.WhiteSmoke);

        protected override void OnInitialized()
        {
            Nante nante1 = new() { Color = Colors.DarkBlue, Peca = "record-circle-fill" };
            Nante nante2 = new() { Color = Colors.DarkGreen, Peca = "record-circle-fill" };
            Nante nante3 = new() { Color = Colors.DarkRed, Peca = "record-circle-fill" };
            Nante nante4 = new() { Color = Colors.DarkGrey, Peca = "record-circle-fill" };

            Caminhos[0].ColocarNante(nante1, 0);
            Caminhos[30].ColocarNante(nante2, 30);
            Caminhos[60].ColocarNante(nante3, 60);
            Caminhos[90].ColocarNante(nante4, 90);

            Task.Run(Atualizar);
        }

        void RolarDados()
        {
            Dados.Rolardado();
            StateHasChanged();
        }

        private async Task Atualizar()
        {
            while (true)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                await InvokeAsync(StateHasChanged);
            }
        }

    }
}
