using System.IO;

namespace Silkveil.Net.Prototype
{
    public interface IBoard<TStartData>
    {
        void Start(TStartData startData);
    }

    public abstract class Board<TStartData> : IBoard<TStartData>
    {
        public abstract void Start(TStartData startData);
    }

    public class SilkveilBoard : Board<Stream>
    {
        private RequestAnalyzer _analyzer;

        public SilkveilBoard(RequestAnalyzer requestAnalyzer, Download download, Redirect redirect)
        {
            this._analyzer = requestAnalyzer;

            this._analyzer.DownloadAvailable += download.OnDownloadAvailable;
            this._analyzer.RedirectAvailable += redirect.OnRedirectAvailable;
        }

        public override void Start(Stream startData)
        {
            using(startData)
            {
                this._analyzer.Analyze(startData);
            }
        }
    }
}