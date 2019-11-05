using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using static Common.Utils;

namespace Client.DownloadService
{
    public abstract class SocketRequest<T>
    {

        protected Socket ServerSocket { get; }
        protected IPEndPoint EndPoint { get; }

        public SocketRequest(Socket server, IPEndPoint endpoint)
        {
            ServerSocket = server;
            EndPoint = endpoint;
        }

        protected virtual byte[] SendRequest(string message)
        {
            WriteLog("Connecting to server socket...");
            ServerSocket.Connect(EndPoint);
            WriteLog("Server Soket connected. Sending message...");

            byte[] messageSent = $"{message}<EOF>".ToUTFBytes();
            int byteSent = ServerSocket.Send(messageSent);

            WriteLog("Message Sent. Waiting for server's response...");

            var wholeMessage = new List<byte[]>();

            byte[] messageReceived = new byte[2048];

            while (true)
            {

                int byteRecv = ServerSocket.Receive(messageReceived);

                if (byteRecv == 0)
                    break;
                else
                    wholeMessage.Add(messageReceived.Take(byteRecv).ToArray());

                Console.Write($"Downloaded: ");
                WriteColored(byteRecv.ToString(), ConsoleColor.Green);
                Console.WriteLine(" bytes");


            }

            WriteLog("Sutting down request.");

            ServerSocket.Shutdown(SocketShutdown.Both);
            ServerSocket.Close();

            return wholeMessage.SelectMany(a => a).ToArray();
        }

        public abstract T SendRequest();
    }
}
