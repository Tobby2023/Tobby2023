using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace AppNotification.Service
{
    public class IPService
    {
        public IPAddress IP { get; init; }
        public IPAddress GetAway { get; init; }

        public IPService()
        {
            IP = GetLocalIP();
            GetAway = GetGatewayAddress();
        }

        public IPAddress GetLocalIP()
        {
            var listIps = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface ni in listIps)
            {
                if (DevicePlatform.Android == DeviceInfo.Platform)
                {
                    if (ni.OperationalStatus == OperationalStatus.Up &&
                        (ni.NetworkInterfaceType == NetworkInterfaceType.Unknown ||
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
                else
                {
                    if (ni.OperationalStatus == OperationalStatus.Up &&
                        (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211))
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
            }
            return IPAddress.Any;
        }

        public IPAddress GetGatewayAddress()
        {
            if (DevicePlatform.WinUI == DeviceInfo.Platform)
            {
                foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (ni.OperationalStatus == OperationalStatus.Up &&
                        (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                        ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet))
                    {
                        var gatewayAddress = ni.GetIPProperties()!.GatewayAddresses!
                            .FirstOrDefault(g => g.Address.AddressFamily == AddressFamily.InterNetwork);

                        if (gatewayAddress != null)
                        {
                            return gatewayAddress.Address; // Retorna o gateway da rede
                        }
                    }
                }
            }
            return IPAddress.Any;
        }
    }
}
