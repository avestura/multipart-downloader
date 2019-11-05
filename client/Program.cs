using System;
using System.Collections.Generic;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using static System.Console;
using static Common.Utils;
using Client.DownloadService;

namespace Client
{
    internal class Program
    {


        public static int NumberOfParts { get; private set; }

        public static Socket ServerSocket { get; set; }

        public static IPEndPoint EndPoint { get; set; }

        public static string FileName { get; private set; }

        public static long FileSize { get; private set; }

        static void Main(string[] args)
        {
            NumberOfParts = GetNumber("Please enter number of parts: ");
            FetchMeta();

            Console.Write("Server is serving: ");
            WriteLnColored(FileName, ConsoleColor.Green);
            Console.Write("File Size: ");
            WriteLnColored(FileSize.ToString(), ConsoleColor.Yellow);

            while (true)
            {
                int partNumber = GetNumber("Enter the number of part you want: ");

                if(partNumber > NumberOfParts || partNumber < 0)
                {
                    WriteLine("> Not valid");
                    continue;
                }

                var (server, endpoint) = ConnectionUtils.EstablishConnection();
                var req = new FilePartitionRequest(new RequestInfo
                {
                    PartNumber = partNumber,
                    TotalNumberOfParts = NumberOfParts
                }, server, endpoint);

                var data = req.SendRequest();
                string partFileName = GetPartFileName(FileName, partNumber);
                Write("Writing part data to: ");
                WriteLnColored(partFileName, ConsoleColor.Blue);

                File.WriteAllBytes(partFileName, data);

                if (CheckIfAllPartsDownloaded())
                {
                    AssembleParts();
                    return;
                }

            }
        }

        static bool CheckIfAllPartsDownloaded()
        {
            WriteLine();
            bool allExist = true;
            for(int i = 1; i <= NumberOfParts; i++)
            {
                var partFileName = GetPartFileName(FileName, i);
                Write("> Checking whether file \"");
                WriteColored(partFileName, ConsoleColor.Yellow);
                Write("\" exists: ");
                if (File.Exists(partFileName))
                {
                    WriteLnColored("DOWNLOADED", ConsoleColor.Green);
                }
                else
                {
                    allExist = false;
                    WriteLnColored("NOT DOWNLOADED", ConsoleColor.Red);
                }
            }

            return allExist;
        }

        static void AssembleParts()
        {
            using (var stream = new FileStream(FileName, FileMode.Append, FileAccess.Write))
            {
                for (int i = 1; i <= NumberOfParts; i++)
                {
                    using (var partData = File.OpenRead(GetPartFileName(FileName, i)))
                    {
                        partData.CopyTo(stream);
                    }

                    File.Delete(GetPartFileName(FileName, i));
                }

                Write("All files assembled together in \"");
                WriteColored(FileName, ConsoleColor.Green);
                WriteLine("\"");
            }
        }

        static void FetchMeta()
        {
            var (server, endpoint) = DownloadService.ConnectionUtils.EstablishConnection();
            var req = new FileMetadataRequest(server, endpoint);
            var resp = req.SendRequest();

            var tokens = resp.Split("/^/");
            FileName = tokens[0];
            FileSize = long.Parse(tokens[1]);

        }
    }
}
