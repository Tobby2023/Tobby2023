using MauiWifiConecta.Modelo;

namespace MauiWifiConecta.Components.Pages.ObjectoDeJogo
{
    public partial class TabuleiroNante
    {
        public Dados Dados { get; set; } = new();
        List<NanteCaminho> Caminhos { get; set; } = NanteCaminho.NanteCaminhos(Colors.WhiteSmoke);

        protected override void OnInitialized()
        {
            Nante nante11 = new() { Color = Colors.DarkBlue, Peca = "record-circle-fill" };

            Nante nante21 = new() { Color = Colors.DarkGreen, Peca = "record-circle-fill" };

            Nante nante31 = new() { Color = Colors.DarkRed, Peca = "record-circle-fill" };

            Nante nante41 = new() { Color = Colors.DarkGrey, Peca = "record-circle-fill" };

            Caminhos[0].ColocarNante(nante11, 0);

            Caminhos[30].ColocarNante(nante21, 30);

            Caminhos[60].ColocarNante(nante31, 60);

            Caminhos[90].ColocarNante(nante41, 90);

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
                await Task.Delay(TimeSpan.FromSeconds(0.5));
                await InvokeAsync(StateHasChanged);
            }
        }

    }
}
