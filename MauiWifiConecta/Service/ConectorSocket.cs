using System.Net;
using System.Net.WebSockets;
using System.Text;

namespace MauiWifiConecta.Service
{
    public class ConectorSocket(HostConectado host)
    {

        public ClientWebSocket EuSocket;
        public List<WebSocket> TodosSocket { get; set; } = new();

        private readonly HostConectado host = host;
        public HttpListener? Servidor { get; set; }

        public async Task<bool> IniciarServidor(int index)
        {
            try
            {
                Servidor = new HttpListener();
                Servidor.Prefixes.Add($"http://{host.MeuHost[index]}:1909/");
                Servidor.Start();
                return true;
            }
            catch (Exception ex)
            {
                await Application.Current!.MainPage!.DisplayAlert("Erro", "Erro: " + ex.Message, "Ok");
                return false;
            }
        }

        public async Task<bool> ConectarServidor(int index)
        {
            try
            {
                EuSocket = new ClientWebSocket();
                await EuSocket.ConnectAsync(new Uri($"ws://{host.HostEncontrado[index]}:1909/"), CancellationToken.None);
                return true;
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Erro", "Ocorreu um erro: " + ex.Message, "Ok");
                return false;
            }
        }

        // Método que envia a mensagem
        public async Task SendMessageAsync(string message)
        {
            try
            {
                var messageBytes = Encoding.UTF8.GetBytes(message);
                await EuSocket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Erro", "Ocorreu um erro: " + ex.Message, "Ok");
            }
        }

        // Método que aguarda e retorna a mensagem recebida
        public async Task<string> ReceiveMessageAsync()
        {
            var buffer = new byte[1024];
            var result = await EuSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

            // Retorna a mensagem recebida
            return message;
        }



        // Processar as mensagens de um cliente conectado
        public async Task HandleClientSocketAsync(WebSocket socket)
        {
            var buffer = new byte[1024];
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    await BroadcastToAllClientsAsync($"{socket.GetHashCode()}: {message}");
                }
            }
            if (socket.State != WebSocketState.Open)
            {
                TodosSocket.Remove(socket);
            }
        }

        // Enviar uma mensagem para todos os clientes conectados
        public async Task BroadcastToAllClientsAsync(string message)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);
            foreach (var client in TodosSocket)
            {
                if (client.State == WebSocketState.Open)
                {
                    await client.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }
}
