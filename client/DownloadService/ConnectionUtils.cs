using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Client.DownloadService
{
    public static class ConnectionUtils
    {

        public static (Socket, IPEndPoint) EstablishConnection()
        {

            try
            {

                IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddr = ipHost.AddressList[0];
                Console.WriteLine(ipAddr.ToString());
                IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 12714);
                return (new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp), localEndPoint);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return (null, null);

            }
        }
    }
}
