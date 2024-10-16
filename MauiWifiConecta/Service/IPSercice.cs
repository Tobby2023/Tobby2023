using System.Net.Sockets;
using System.Net;
using System.Net.NetworkInformation;

namespace MauiWifiConecta.Service
{
    public class IPSercice
    {
        public IPSercice()
        {
            // Obtém todos os endereços IP do dispositivo
            OutrosIPs = Dns.GetHostAddresses(Dns.GetHostName()).Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToList();

            // Identifica o "MeuIP" com base nos critérios de rede
            MeuIP = DescobrirMeuIP();

            // Pode definir o IPServidor como o MeuIP ou outro IP válido
            IPServidor = MeuIP ?? OutrosIPs.First();
        }

        public IPAddress IPServidor { get; set; }
        public IPAddress? MeuIP { get; set; }
        public List<IPAddress> OutrosIPs { get; set; }

        private IPAddress? DescobrirMeuIP()
        {
            foreach (var ip in OutrosIPs)
            {
                // Ignora 127.0.0.1 (loopback)
                if (ip.Equals(IPAddress.Loopback))
                    continue;

                // Verifica se o IP é válido na rede local (192.168.x.x ou 10.x.x.x)
                byte[] bytes = ip.GetAddressBytes();
                if (bytes[0] == 192 && bytes[1] == 168 || bytes[0] == 10)
                {
                    return ip;  // Este é o IP da rede local
                }
            }

            // Se nenhum IP foi encontrado, retorna nulo ou o primeiro IP disponível
            return OutrosIPs.FirstOrDefault();
        }

        public static IPAddress GetLocalIPAddress()
        {
#if ANDROID
            _ = App.Current!.MainPage!.DisplayAlert("Getway", MauiWifiConecta.MainActivity.GetGatewayAddress(), "OK");
#endif
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;  // Este é o endereço IPv4 do dispositivo
                }
            }
            throw new Exception("Nenhum endereço IPv4 encontrado.");
        }

        public IPAddress GetLocalIP()
        {
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus == OperationalStatus.Up &&
                    (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                    ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet))
                {
                    foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            return ip.Address; // IP local válido encontrado
                        }
                    }
                }
            }
            return IPAddress.Any;
            // throw new Exception("Não foi possível encontrar o IP da rede.");
        }

        public List<IPAddress> GetLocalIPs()
        {
            List<IPAddress> ips = new List<IPAddress>();
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            ips.Add(ip.Address); // IP local válido encontrado
                        }
                    }
                }
            }
            return ips;
            // throw new Exception("Não foi possível encontrar o IP da rede.");
        }

        public IPAddress GetGatewayAddress()
        {
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus == OperationalStatus.Up &&
                    (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                    ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet))
                {
                    var gatewayAddress = ni.GetIPProperties().GatewayAddresses
                        .FirstOrDefault(g => g.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);

                    if (gatewayAddress != null)
                    {
                        return gatewayAddress.Address; // Retorna o gateway da rede
                    }
                }
            }
            return IPAddress.Any;
            //throw new Exception("Não foi possível encontrar o gateway da rede.");
        }
    }
}
