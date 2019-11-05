using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;
using System.Net;
using System.IO;
using System.Net.Sockets;

namespace Common
{
    public static class Utils
    {

        public static void ColorWriter(Action writer, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            writer();
            Console.ResetColor();
        }

        public static void WriteColored(string text, ConsoleColor color) => ColorWriter(() => Console.Write(text), color);

        public static void WriteLnColored(string text, ConsoleColor color) => ColorWriter(() => Console.WriteLine(text), color);

        public static void WriteLog(string text) => WriteLnColored($"LOG> {text}", ConsoleColor.Gray);

        public static void ShutAndClose(this Socket socket)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        public static int GetNumber(string prompt)
        {
            Write(prompt);
            WriteLine();

            return int.Parse(Console.ReadLine());
        }


        public static string AsUTFString(this byte[] bytes) => Encoding.UTF8.GetString(bytes);
        public static byte[] ToUTFBytes(this string str) => Encoding.UTF8.GetBytes(str);

        public static (string, string) ExtractFileExtension(string fileName)
        {
            var name = Path.GetFileNameWithoutExtension(fileName);
            var ext = Path.GetExtension(fileName);

            return (name, ext);
        }

        public static string GetPartFileName(string fileName, int partNumber)
        {

            var (name, ext) = ExtractFileExtension(fileName);
            return $"{name}{ext}.{partNumber}.part";
        }
    }
}
