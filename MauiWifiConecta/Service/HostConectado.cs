using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace MauiWifiConecta.Service
{
    public class HostConectado
    {
        public List<IPAddress> HostEncontrado { get; set; } = new();
        public List<IPAddress> MeuHost { get; set; } = new()!;

        public HostConectado()
        {
            if (EstaConnectadoWifi())
            {
                IPAddress ip;
#if ANDROID
                ip = IPAddress.Parse(MauiWifiConecta.MainActivity.GetGatewayAddress());
#else
                ip = IPAddress.Parse(GetGatewayAddress());
#endif
                if (ip.ToString() != "0.0.0.0") HostEncontrado.Add(ip);
            }
            GetLocalIPs();
        }

        private bool EstaConnectadoWifi()
        {
            var profiles = Connectivity.ConnectionProfiles;

            if (DevicePlatform.Android == DeviceInfo.Platform)
                // Verifica se o dispositivo está conectado a uma rede Wi-Fi
                return profiles.Contains(ConnectionProfile.Cellular);
            else
                // Verifica se o dispositivo está conectado a uma rede Wi-Fi
                return profiles.Contains(ConnectionProfile.WiFi);
        }

        public void GetLocalIPs()
        {
            MeuHost.Clear();
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (DevicePlatform.Android == DeviceInfo.Platform)
                {
                    if (ni.OperationalStatus == OperationalStatus.Up &&
                        (ni.NetworkInterfaceType == NetworkInterfaceType.Loopback))
                    {
                        foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                MeuHost.Add(ip.Address);
                            }
                        }
                    }
                }
                else
                {
                    if (ni.OperationalStatus == OperationalStatus.Up &&
                        (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211))
                    {
                        foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                MeuHost.Add(ip.Address);
                            }
                        }
                    }
                }
            }
        }

        public string GetGatewayAddress()
        {
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus == OperationalStatus.Up &&
                    (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                    ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet))
                {
                    var gatewayAddress = ni.GetIPProperties().GatewayAddresses
                        .FirstOrDefault(g => g.Address.AddressFamily == AddressFamily.InterNetwork);

                    if (gatewayAddress != null)
                    {
                        return gatewayAddress.Address.ToString(); // Retorna o gateway da rede
                    }
                }
            }
            return IPAddress.Any.ToString();
            //throw new Exception("Não foi possível encontrar o gateway da rede.");
        }
    }
}
