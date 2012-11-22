using System;
using System.IO;

namespace Silkveil.Net.Prototype
{
    public class RequestAnalyzer
    {
        private static Random random = new Random();
        // private IEnumerable<IRequestAnalyzer> _analyzers;

        // public RequestAnalyzer(IEnumerable<IRequestAnalyzer> analyzers)
        public RequestAnalyzer()
        {

        }

        public void Analyze(Stream stream)
        {
            if(random.NextDouble() > 0.5)
            {
                this.DownloadAvailable(stream);
            }
            else
            {
                this.RedirectAvailable(stream);
            }
        }

        public event Action<Stream> DownloadAvailable;
        public event Action<Stream> RedirectAvailable;
    }
}