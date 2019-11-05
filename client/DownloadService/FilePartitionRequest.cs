using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using static Common.Utils;

namespace Client.DownloadService
{
    public class FilePartitionRequest : SocketRequest<byte[]>
    {
        private RequestInfo Info { get; }

        public FilePartitionRequest(RequestInfo info, Socket server, IPEndPoint endpoint) : base(server, endpoint)
        {
            Info = info;
        }

        public override byte[] SendRequest() => SendRequest($"part:{Info.PartNumber}/{Info.TotalNumberOfParts}");
    }

    public class RequestInfo
    {
        public int PartNumber { get; set; }

        public int TotalNumberOfParts { get; set; }
    }
}
