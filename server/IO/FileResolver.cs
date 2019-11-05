using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using static System.Console;
using static Common.Utils;

namespace Server.IO
{
    public static class FileResolver
    {
        public static string ChooseFile()
        {

            string input;
            while (true)
            {
                WriteLine();
                Write("Please enter relative path of file: ");
                input = Console.ReadLine();
                if (!File.Exists(input))
                {
                    WriteLnColored("File does not exist. Please input a valid path.", ConsoleColor.Red);
                }
                else break;
            }

            return input;
        }
    }
}
