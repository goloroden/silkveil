using System.Web;

namespace silkveil.net.Integration.Web.Tests
{
    public class HttpContextMock : HttpContextBase
    {
        private readonly HttpRequestBase _httpRequest;
        
        private readonly HttpResponseBase _httpResponse;

        public HttpContextMock(string appRelativeCurrentExecutionPath)
        {
            this._httpRequest = new HttpRequestMock(appRelativeCurrentExecutionPath);
            this._httpResponse = new HttpResponseMock();
        }

        public override HttpRequestBase Request
        {
            get
            {
                return this._httpRequest;
            }
        }

        public override HttpResponseBase Response
        {
            get
            {
                return this._httpResponse;
            }
        }
    }
}