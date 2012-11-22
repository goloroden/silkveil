using System;
using System.IO;

namespace Silkveil.Net.Prototype
{
    public class Redirect
    {
        public Redirect()
        {

        }

        public void OnRedirectAvailable(Stream stream)
        {
            Console.WriteLine("Redirected: " + stream.ToString());
        }

        public event Action<Stream> RedirectAvailable;
    }
}