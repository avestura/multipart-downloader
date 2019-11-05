using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;
using Server.Network;
using System.IO;
using static Common.Utils;
using Server.IO;

namespace Server
{
    static class Program
    {

        private static string FileName { get; set; }

        public static void Main(string[] args)
        {

            var path = FileResolver.ChooseFile();
            FileName = path;
            WriteLine($"File path: {path}");

            var serverInit = new LocalServerInitializer(12714, ProcessClientAsync);

            serverInit.StartListening();
        }

        static async Task ProcessClientAsync(Socket clientSocket)
        {
            await Task.Run(() =>
            {
                byte[] bytes = new byte[1024];
                string data = null;

                while (true)
                {

                    int numByte = clientSocket.Receive(bytes);
                    WriteLog($"Recieved {numByte} bytes from client.");
                    data += Encoding.ASCII.GetString(bytes, 0, numByte);

                    if (data.IndexOf("<EOF>") > -1) break;
                }

                WriteLog($"Text received: {data} ");

                var request = data.Replace("<EOF>", "");

                byte[] reply = null;
                if(request == "meta")
                {
                    WriteLnColored("META REQUEST", ConsoleColor.Magenta);
                    var size = new FileInfo(FileName).Length;
                    var response = $"{Path.GetFileName(FileName)}/^/{size}";
                    reply = response.ToUTFBytes();
                }
                else if (request.StartsWith("part:"))
                {
                    WriteLnColored("PART REQUEST", ConsoleColor.Magenta);
                    var purged = request.Replace("part:", "");
                    var tokens = purged.Split("/");
                    var (part, parts) = (int.Parse(tokens[0]), int.Parse(tokens[1]));

                    reply = new ChunkReader(FileName, part, parts).GetChunck();
                }

                WriteLog("Reply generated, sending Reply...");
                clientSocket.Send(reply);
                WriteLog("Reply Sent. Sending EOF stream");
                clientSocket.Send(new byte[] { });
                WriteLog("EOF sent, closing connection...");
                clientSocket.ShutAndClose();
                WriteLog("Connection processed and closed.");

            });
        }


    }
}
