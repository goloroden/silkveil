using cherryflavored.net;

using silkveil.net.Contracts.Authentication;

using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace silkveil.net.Authentication
{
    /// <summary>
    /// Represents a HTTP digest authentication.
    /// </summary>
    /// <remarks>
    /// Includes code from http://www.rassoc.com/gregr/weblog/2002/07/09/web-services-security-http-digest-authentication-without-active-directory/
    /// </remarks>
    public class HttpDigestAuthentication : AuthenticationBase
    {
        /// <summary>
        /// Creates a new instance of the <see cref="HttpDigestAuthentication"/> type.
        /// </summary>
        public HttpDigestAuthentication()
            : this("", "")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpDigestAuthentication" /> type.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="password">The password.</param>
        public HttpDigestAuthentication(string userName, string password)
            : base(userName, password)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpDigestAuthentication" /> type.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="password">The password.</param>
        /// <param name="realm">The realm.</param>
        public HttpDigestAuthentication(string userName, string password, string realm)
            : base(userName, password, realm)
        {
        }

        /// <summary>
        /// Gets the authentication type.
        /// </summary>
        /// <value>The authentication type.</value>
        public override AuthenticationType AuthenticationType
        {
            get
            {
                return AuthenticationType.HttpDigestAuthentication;
            }
        }

        /// <summary>
        /// Authorizes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        protected override void AuthenticateCore(WebRequest context)
        {
            context = Enforce.NotNull(context, () => context);

            // Authorize the request.
            context.Credentials = this.Credentials;
        }

        /// <summary>
        /// Requests authentication for the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public override void RequestAuthentication(HttpContext context)
        {
            context = Enforce.NotNull(context, () => context);

            string nonce = CreateNonce();
            bool isNonceStale = false;
            object staleObj = context.Items["staleNonce"];
            if (staleObj != null)
            {
                isNonceStale = (bool)staleObj;
            }

            StringBuilder challenge = new StringBuilder("Digest");
            challenge.Append(" realm=\"");
            challenge.Append(this.Realm);
            challenge.Append("\"");
            challenge.Append(", nonce=\"");
            challenge.Append(nonce);
            challenge.Append("\"");
            challenge.Append(", opaque=\"0000000000000000\"");
            challenge.Append(", stale=");
            challenge.Append(isNonceStale ? "true" : "false");
            challenge.Append(", algorithm=MD5");
            challenge.Append(", qop=\"auth\"");

            context.Response.AppendHeader("WWW-Authenticate", challenge.ToString());
            context.Response.StatusCode = 401;
            context.Response.End();
 }

        /// <summary>
        /// Gets whether the specified context's request is authenticated yet.
        /// </summary>
        /// <param name="context">The context whose request should be checked.</param>
        /// <returns><value>true</value> if the request is authenticated, otherwise <value>false</value>.</returns>
        public override bool IsAuthenticated(HttpContext context)
        {
            context = Enforce.NotNull(context, () => context);

            string authStr = context.Request.Headers["Authorization"];

            if (String.IsNullOrEmpty(authStr))
            {
                // No credentials; anonymous request
                return false;
            }

            authStr = authStr.Trim();
            if (authStr.IndexOf("Digest", StringComparison.Ordinal) != 0)
            {
                // Don't understand this header...we'll pass it along and 
                // assume someone else will handle it
                return false;
            }

            authStr = authStr.Substring(7);

            ListDictionary reqInfo = new ListDictionary();

            string[] elems = authStr.Split(new char[] { ',' });
            foreach (string elem in elems)
            {
                // form key="value"
                string[] parts = elem.Split(new char[] { '=' }, 2);
                string key = parts[0].Trim(new char[] { ' ', '\"' });
                string val = parts[1].Trim(new char[] { ' ', '\"' });
                reqInfo.Add(key, val);
            }

            // calculate the Digest hashes

            // A1 = unq(username-value) ":" unq(realm-value) ":" passwd
            string A1 = String.Format(CultureInfo.InvariantCulture,
                                      "{0}:{1}:{2}", (string)reqInfo["username"], this.Realm, this.Password);

            // H(A1) = MD5(A1)
            string HA1 = GetMD5HashBinHex(A1);

            // A2 = Method ":" digest-uri-value
            string A2 = String.Format(CultureInfo.InvariantCulture,
                                      "{0}:{1}", context.Request.HttpMethod, (string)reqInfo["uri"]);

            // H(A2)
            string HA2 = GetMD5HashBinHex(A2);

            // KD(secret, data) = H(concat(secret, ":", data))
            // if qop == auth:
            // request-digest  = <"> < KD ( H(A1),     unq(nonce-value)
            //                              ":" nc-value
            //                              ":" unq(cnonce-value)
            //                              ":" unq(qop-value)
            //                              ":" H(A2)
            //                            ) <">
            // if qop is missing,
            // request-digest  = <"> < KD ( H(A1), unq(nonce-value) ":" H(A2) ) > <">

            string unhashedDigest;
            if (reqInfo["qop"] != null)
            {
                unhashedDigest =
                    String.Format(CultureInfo.InvariantCulture, "{0}:{1}:{2}:{3}:{4}:{5}",
                                  HA1,
                                  (string)reqInfo["nonce"],
                                  (string)reqInfo["nc"],
                                  (string)reqInfo["cnonce"],
                                  (string)reqInfo["qop"],
                                  HA2);
            }
            else
            {
                unhashedDigest =
                    String.Format(CultureInfo.InvariantCulture, "{0}:{1}:{2}",
                                  HA1,
                                  (string)reqInfo["nonce"],
                                  HA2);
            }

            string hashedDigest = GetMD5HashBinHex(unhashedDigest);

            bool isNonceStale = !IsValidNonce((string)reqInfo["nonce"]);
            context.Items["staleNonce"] = isNonceStale;

            return (((string)reqInfo["response"] == hashedDigest) && (!isNonceStale));
        }

        /// <summary>
        /// Gets an MD5 hash for the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The hash.</returns>
        private static string GetMD5HashBinHex(string value)
        {
            value = Enforce.NotNullOrEmpty(value, () => value);

            Encoding enc = new ASCIIEncoding();
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bHA1 = md5.ComputeHash(enc.GetBytes(value));
            string HA1 = "";
            for (int i = 0; i < 16; i++)
            {
                HA1 += String.Format(CultureInfo.InvariantCulture, "{0:x02}", bHA1[i]);
            }
            return HA1;
        }

        /// <summary>
        /// Creates a nonce which is the text representation of the current time plus one minute.
        /// So this nonce will be valid for this one minute.
        /// </summary>
        /// <returns>The newly created nonce.</returns>
        protected virtual string CreateNonce()
        {
            DateTime nonceTime = DateTime.Now + TimeSpan.FromMinutes(1);
            string expireStr = nonceTime.ToString("G", CultureInfo.InvariantCulture);

            Encoding enc = new ASCIIEncoding();
            byte[] expireBytes = enc.GetBytes(expireStr);
            string nonce = Convert.ToBase64String(expireBytes);

            // nonce can't end in '=', so trim them from the end
            nonce = nonce.TrimEnd(new Char[] { '=' });
            return nonce;
        }

        /// <summary>
        /// Returns whether the specified nonce is valid.
        /// </summary>
        /// <param name="nonce">The nonce to check.</param>
        /// <returns><c>true</c> if the nonce is valid; <c>false</c> otherwise.</returns>
        protected virtual bool IsValidNonce(string nonce)
        {
            nonce = Enforce.NotNull(nonce, () => nonce);

            DateTime expireTime;

            // pad nonce on the right with '=' until length is a multiple of 4
            int numPadChars = nonce.Length % 4;
            if (numPadChars > 0)
                numPadChars = 4 - numPadChars;
            string newNonce = nonce.PadRight(nonce.Length + numPadChars, '=');

            try
            {
                byte[] decodedBytes = Convert.FromBase64String(newNonce);
                string expireStr = new ASCIIEncoding().GetString(decodedBytes);
                expireTime = DateTime.Parse(expireStr, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                return false;
            }

            return (DateTime.Now <= expireTime);
        }
    }
}