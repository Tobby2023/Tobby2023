using System.Net.WebSockets;
using System.Text;

namespace MauiWifiConecta.Service
{
    public class WebSocketClientService
    {
        private ClientWebSocket _clientWebSocket;

        public async Task ConnectToServerAsync(string serverIp, int port)
        {
            try
            {
                _clientWebSocket = new ClientWebSocket();
                await _clientWebSocket.ConnectAsync(new Uri($"ws://{serverIp}:{port}/"), CancellationToken.None);
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Erro", "Ocorreu um erro: " + ex.Message, "Ok");
            }
        }

        public async Task SendMessageAsync(string message)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);
            await _clientWebSocket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        // Método que aguarda e retorna a mensagem recebida
        public async Task<string> ReceiveMessageAsync()
        {
            var buffer = new byte[1024];
            var result = await _clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

            // Retorna a mensagem recebida
            return message;
        }
    }
}
