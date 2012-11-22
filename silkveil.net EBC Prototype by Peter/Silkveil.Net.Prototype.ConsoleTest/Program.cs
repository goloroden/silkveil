using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Silkveil.Net.Prototype.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var requestAnalyzer = new RequestAnalyzer();
            var download = new Download();
            var redirect = new Redirect();

            var board = new SilkveilBoard(requestAnalyzer, download, redirect);

            using (var stream = new MemoryStream())
            {
                board.Start(stream);
            }

            Console.Read();
        }
    }
}