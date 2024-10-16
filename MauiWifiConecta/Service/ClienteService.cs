using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace MauiWifiConecta.Service
{
    public class ClienteService
    {
        private readonly string _serverIp = "192.168.43.1"; // IP do servidor na rede
        private readonly int _serverPort = 5000; // Porta do servidor

        public async Task EnviarDadosParaServidor(Dictionary<string, string> data)
        {
            try
            {
                string jsonData = JsonSerializer.Serialize(data);

                using TcpClient client = new TcpClient();
                await client.ConnectAsync(_serverIp, _serverPort);

                NetworkStream stream = client.GetStream();
                byte[] dataToSend = Encoding.UTF8.GetBytes(jsonData);
                await stream.WriteAsync(dataToSend, 0, dataToSend.Length);

                // Recebe a resposta do servidor
                byte[] buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                // Exibe a resposta recebida do servidor
                await App.Current!.MainPage!.DisplayAlert("Resposta", "Resposta do Servidor: " + response, "Ok");
            }
            catch (Exception ex)
            {
                // Exibe um erro em caso de falha
                await App.Current!.MainPage!.DisplayAlert("Erro", "Erro: " + ex.Message, "Ok");
            }
        }
    }
}
