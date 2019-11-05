using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using static Common.Utils;

namespace Server.IO
{
    public class ChunkReader
    {
        public string Path { get; }

        public int Part { get; }

        public int Parts { get; }

        public ChunkReader(string path, int part, int parts)
        {
            WriteLog($"Initializing Chunk for Part:{part} of {parts}");
            Path = path;
            Part = part;
            Parts = parts;
            WriteLog($"Initialized for Chunk {part} of {parts}");
        }

        public byte[] GetChunck()
        {
            using (var stream = File.OpenRead(Path))
            {
                int partSize = (int)Math.Ceiling((double)stream.Length / Parts);

                WriteLog($"Getting chunck bytes... PartSize = {partSize}");
                using (var ms = new MemoryStream())
                {
                    for (int i = 1; i <= Parts; i++)
                    {


                        byte[] buffer = new byte[partSize];

                        int read = stream.Read(buffer, 0, buffer.Length);
                        if (i == Part)
                        {
                            ms.Write(buffer, 0, read);
                              break;
                        }

                    }
                WriteLog($"Chunk written for Part:{Part} of {Parts} segments");
                return ms.ToArray();
                }
            }
        }
    }
}
