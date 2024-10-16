using System.Net.WebSockets;
using System.Net;
using System.Text;

namespace MauiWifiConecta.Service
{
    public class WebSocketServerService
    {
        private List<WebSocket> _connectedClients = new();

        // Propriedade para armazenar a última mensagem recebida
        public string Mensagem { get; private set; } = string.Empty;

        // Iniciar o servidor WebSocket
        public async Task StartWebSocketServerAsync(string serverIp, int port)
        {
            try
            {
                var server = new HttpListener();
                server.Prefixes.Add($"http://{serverIp}:{port}/");
                server.Start();

                while (true)
                {
                    var context = await server.GetContextAsync();
                    if (context.Request.IsWebSocketRequest)
                    {
                        var wsContext = await context.AcceptWebSocketAsync(null);
                        var socket = wsContext.WebSocket;
                        _connectedClients.Add(socket);

                        // Opcional: Mensagem de notificação ao conectar um cliente
                        await BroadcastToAllClientsAsync($"Novo cliente conectado: {socket.GetHashCode()}");

                        // Processa as mensagens recebidas do cliente
                        _ = Task.Run(() => HandleClientSocketAsync(socket));
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Erro", "Erro: " + ex.Message, "Ok");
            }
        }

        // Processar as mensagens de um cliente conectado
        private async Task HandleClientSocketAsync(WebSocket socket)
        {
            var buffer = new byte[1024];
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    await BroadcastToAllClientsAsync($"{message}");
                }
            }

        }

        // Enviar uma mensagem para todos os clientes conectados
        private async Task BroadcastToAllClientsAsync(string message)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);
            Mensagem = message; // Armazena a última mensagem recebida
            foreach (var client in _connectedClients)
            {
                if (client.State == WebSocketState.Open)
                {
                    await client.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }

        public async Task SendMessageAsync(string message)
        {
            await BroadcastToAllClientsAsync(message);
        }
    }
}
