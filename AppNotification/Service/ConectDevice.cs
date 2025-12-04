using BClasses.Models;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace AppNotification.Service
{
    public class ConectDevice
    {
        private readonly IPService IPService;
        private readonly NotificationService Notification;

        private HttpListener? Server { get; set; }
        private ClientWebSocket? MySocket { get; set; }
        private int Codigo { get; set; } = 0;
        public List<WebSocket> TodosSocket { get; set; } = new List<WebSocket>();

        public ConectDevice(IPService iPService, NotificationService notification)
        {
            IPService = iPService;
            Notification = notification;
        }

        public async Task<bool> InitServer()
        {
            try
            {
                Server = new HttpListener();
                Server.Prefixes.Add($"http://{IPService.IP}:1909/");
                Server.Start();
                return true;
            }
            catch (Exception ex)
            {
                await Notification.ScheduleNotification("Erro", "Erro: " + ex.Message, DateTime.Now.AddSeconds(5));
                return false;
            }
        }

        public async Task<bool> ConectToServer(IPAddress? iP = null)
        {
            try
            {
                MySocket = new ClientWebSocket();
                await MySocket.ConnectAsync(new Uri($"ws://{(iP == null ? IPService.GetAway : iP)}:1909/"), CancellationToken.None);
                return true;
            }
            catch (Exception ex)
            {
                await Notification.ScheduleNotification("Erro", "Erro: " + ex.Message, DateTime.Now.AddSeconds(5));
                return false;
            }
        }

        // Método que envia a mensagem
        public async Task SendMessageAsync(Ambiente message)
        {
            try
            {
                var str = JsonSerializer.Serialize(message);
                var messageBytes = Encoding.UTF8.GetBytes(str);
                await MySocket!.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (Exception ex)
            {
                await Notification.ScheduleNotification("Erro", "Ocorreu um erro: " + ex.Message, DateTime.Now);
            }
        }

        public string State() => MySocket != null ? MySocket.State.ToString() : (Server != null ? Server.IsListening.ToString() : "Unknow");

        // Processar as mensagens de um cliente conectado
        public async Task HandleClientSocketAsync(WebSocket socket)
        {
            var buffer = new byte[5000];
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var str = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Ambiente message = JsonSerializer.Deserialize<Ambiente>(str)!;
                    await BroadcastToAllClientsAsync(message);
                }
            }
            if (socket.State != WebSocketState.Open)
            {
                TodosSocket.Remove(socket);
            }
        }

        // Enviar uma mensagem para todos os clientes conectados
        public async Task BroadcastToAllClientsAsync(Ambiente message)
        {
            string str = JsonSerializer.Serialize(message);
            var messageBytes = Encoding.UTF8.GetBytes(str);

            foreach (var client in TodosSocket)
            {
                if (client.State == WebSocketState.Open)
                {
                    await client.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }

        // Método que aguarda e retorna a mensagem recebida
        public async Task<Ambiente> ReceiveMessageAsync()
        {
            var buffer = new byte[5000];
            var result = await MySocket!.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            // Retorna a mensagem recebida
            var str = Encoding.UTF8.GetString(buffer, 0, result.Count);
            var mens = JsonSerializer.Deserialize<Ambiente>(str)!;
            return mens;
        }

        public int MeuCodigo() => Codigo;
        public void SetCodigo(int codeigo)
        {
            if (Codigo == 0) Codigo = codeigo;
        }

        public async Task<HttpListenerContext> GetContextAsync() => await Server!.GetContextAsync();
    }
}
