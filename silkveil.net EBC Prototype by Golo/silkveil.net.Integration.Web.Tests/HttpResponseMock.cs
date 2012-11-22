using System.Collections.Specialized;
using System.IO;
using System.Web;

namespace silkveil.net.Integration.Web.Tests
{
    public class HttpResponseMock : HttpResponseBase
    {
        private readonly MemoryStream _stream;

        private readonly NameValueCollection _headers;

        public HttpResponseMock()
        {
            this._stream = new MemoryStream();
            this._headers = new NameValueCollection();
        }

        public override Stream OutputStream
        {
            get
            {
                return this._stream;
            }
        }

        public override int StatusCode
        {
            get;
            set;
        }

        public override void AppendHeader(string name, string value)
        {
            this._headers.Add(name, value);
        }

        public override NameValueCollection Headers
        {
            get
            {
                return this._headers;
            }
        }

        public override void End()
        {
        }
    }
}