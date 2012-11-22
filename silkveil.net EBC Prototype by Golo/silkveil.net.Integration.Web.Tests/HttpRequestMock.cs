using System.Web;

namespace silkveil.net.Integration.Web.Tests
{
    public class HttpRequestMock : HttpRequestBase
    {
        private readonly string _appRelativeCurrentExecutionPath;

        public HttpRequestMock(string appRelativeCurrentExecutionPath)
        {
            this._appRelativeCurrentExecutionPath = appRelativeCurrentExecutionPath;
        }

        public override string AppRelativeCurrentExecutionFilePath
        {
            get
            {
                return this._appRelativeCurrentExecutionPath;
            }
        }
    }
}