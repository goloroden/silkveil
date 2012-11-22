using System;
using System.IO;

namespace Silkveil.Net.Prototype
{
    public class Download
    {
        public Download()
        {

        }

        public void OnDownloadAvailable(Stream stream)
        {
            Console.WriteLine("Downloaded: " + stream.ToString());
        }

        public event Action<Stream> DownloadAvailable;
    }
}