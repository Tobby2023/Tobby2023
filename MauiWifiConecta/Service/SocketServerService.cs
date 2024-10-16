using System.Net.Sockets;
using System.Net;
using System.Text;

namespace MauiWifiConecta.Service
{
    public class SocketServerService
    {
        private TcpListener? _listener;

        public void StartSocketServer()
        {
            Task.Run(() =>
            {
                try
                {
                    // Endereço do servidor - pode ser IP local ou Wi-Fi hotspot
                    _listener = new TcpListener(IPAddress.Any, 5000);
                    _listener.Start();

                    while (true)
                    {
                        var client = _listener.AcceptTcpClient();
                        Task.Run(() => HandleClient(client));
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions (logs, UI notifications)
                }
            });
        }

        private async Task HandleClient(TcpClient client)
        {
            var buffer = new byte[1024];
            var stream = client.GetStream();

            // Recebe dados do cliente
            var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            var request = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            // Responde ao cliente
            var response = Encoding.UTF8.GetBytes("Dados recebidos com sucesso");
            await stream.WriteAsync(response, 0, response.Length);

            client.Close();
        }

        public void StopSocketServer()
        {
            _listener?.Stop();
        }
    }
}
