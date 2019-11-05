using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using static System.Console;
using static Common.Utils;

namespace Server.Network
{
    public class LocalServerInitializer
    {

        public delegate Task ClientReqProcessor(Socket clientSocket);

        public int Port { get; }

        public Socket ServerSocket { get; }

        private ClientReqProcessor Processor { get; }

        public IPEndPoint ServerEndPoint { get; }

        public LocalServerInitializer(int port, ClientReqProcessor processor)
        {
            Port = port;
            (ServerSocket, ServerEndPoint) = InitServer(port);
            Processor = processor;
        }

        private (Socket, IPEndPoint) InitServer(int port)
        {
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            WriteLine(ipAddr.ToString());
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, port);

            var server = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            WriteLog("Server and EndPoint created.");
            return (server, localEndPoint);
        }

        public void StartListening()
        {
            try
            {
                ServerSocket.Bind(ServerEndPoint);
                ServerSocket.Listen(100);

                while (true)
                {
                    WriteLine("> Waiting connection ... ");
                    Socket clientSocket = ServerSocket.Accept();
                    Write("$ Accepting connection from REMOTE/");
                    WriteColored(clientSocket.RemoteEndPoint.ToString(), ConsoleColor.Green);
                    Write(", LOCAL/");
                    WriteColored(clientSocket.LocalEndPoint.ToString(), ConsoleColor.Green);
                    WriteLine();

                    WriteLog("Processing Request...");
                    _ = Processor(clientSocket);
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
