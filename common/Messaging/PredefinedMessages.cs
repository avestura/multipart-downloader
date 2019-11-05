using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Messaging
{
    public static class PublicData
    {
        public static byte[] EOF { get; } = Encoding.ASCII.GetBytes("<THIS IS THE END OF IT>");
    }
}
