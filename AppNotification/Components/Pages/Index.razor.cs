using AppNotification.Service;
using BClasses.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Net;

namespace AppNotification.Components.Pages
{
    public partial class Index
    {
        [Inject] public ConectDevice MySocket { get; set; } = default!;
        private (bool, int) result = (false, 0);
        private int Posicao { get; set; } = 0;
        private Ambiente Tabuleiro { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            if (MyIP.GetAway == IPAddress.Any)
            {
                result = (false, 1);
                bool IsServer = await MySocket.InitServer();
                await SegundoPlanoServidor(IsServer);
            }
            else
            {
                result = (false, 2);
            }
        }

        private async Task PartilharTabuleiro()
        {
            Tabuleiro.Data = DateTime.Now;
            await MySocket.SendMessageAsync(Tabuleiro);
        }

        private async Task Iniciar()
        {
            if (result.Item2 == 1)
            {
                result = (await MySocket.ConectToServer(MyIP.IP), result.Item2);
                _ = Task.Run(() => SegundoPlanoCliente(result.Item1));
            }
            else if(result.Item2 == 2)
            {
                result = (await MySocket.ConectToServer(), result.Item2);
                _ = Task.Run(() => SegundoPlanoCliente(result.Item1));
            }
            else
            {
                await Notify.ScheduleNotification("Erro", "Não estás conectado à nenhum dispositivo!");
            }
        }

        private async Task SegundoPlanoServidor(bool resultado)
        {
            while (resultado)
            {
                var context = await MySocket.GetContextAsync();
                if (context.Request.IsWebSocketRequest)
                {
                    var wsContext = await context.AcceptWebSocketAsync(null);
                    var socket = wsContext.WebSocket;
                    MySocket.TodosSocket.Add(socket);

                    Snackbar.Add($"Nova pessoa entrou na rede: {socket.GetHashCode()}", Severity.Info);
                    Tabuleiro.AddJogador(socket.GetHashCode());
                    for (int i = 0; i < 10; i++)
                    {
                        Tabuleiro.DarCartaJogador(socket.GetHashCode());
                    }
                    await PartilharTabuleiro();

                    // Processa as mensagens recebidas do cliente
                    _ = Task.Run(() => MySocket.HandleClientSocketAsync(socket));
                }
            }
        }

        private async Task SegundoPlanoCliente(bool resultado)
        {
            while (resultado)
            {
                Tabuleiro = await MySocket.ReceiveMessageAsync();
                if (MySocket.MeuCodigo() == 0)
                {
                    Posicao = Tabuleiro.Jogadores.Count - 1;
                    MySocket.SetCodigo(Tabuleiro.Jogadores[Posicao]);
                }
                await Task.Delay(TimeSpan.FromSeconds(1));
                await InvokeAsync(StateHasChanged);
            }
        }
    }
}
