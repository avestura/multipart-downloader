using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using static Common.Utils;

namespace Client.DownloadService
{
    public class FileMetadataRequest : SocketRequest<string>
    {
        public FileMetadataRequest(Socket server, IPEndPoint endPoint) : base(server, endPoint)
        {

        }

        public override string SendRequest() => SendRequest("meta").AsUTFString();
    }
}
