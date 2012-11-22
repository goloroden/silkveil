using cherryflavored.net;
using cherryflavored.net.ExtensionMethods.System;

using LightCore;

using silkveil.net.Application;
using silkveil.net.Authentication;
using silkveil.net.Contracts;
using silkveil.net.Contracts.Authentication;
using silkveil.net.Contracts.Redirects;
using silkveil.net.Contracts.Services;
using silkveil.net.Contracts.Users;
using silkveil.net.Web;

using System;
using System.Globalization;
using System.Web;

namespace silkveil.net.Redirects
{
    /// <summary>
    /// Represents a factory for the redirect handlers.
    /// </summary>
    public class RedirectHandlerFactory : HandlerFactoryBase
    {
        /// <summary>
        /// Contains the redirect service.
        /// </summary>
        private IRedirectService _redirectService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectHandlerFactory"/> type.
        /// </summary>
        /// <param name="redirectService">The redirect service.</param>
        public RedirectHandlerFactory(IRedirectService redirectService)
        {
            this._redirectService = redirectService;
        }

        /// <summary>
        /// Returns the right IHttpHandler for the current request
        /// </summary>
        /// <param name="context">The current HttpContext</param>
        /// <param name="requestType">The current RequestType (HTTP verb)</param>
        /// <param name="url">The requestes Url</param>
        /// <param name="pathTranslated">The translated physical path</param>
        /// <param name="container">The container.</param>
        /// <returns>The right IHttpHandler instance for the current request</returns>
        protected override IHttpHandler GetHandlerCore(HttpContext context, string requestType, string url, string pathTranslated, IContainer container)
        {
            context = Enforce.NotNull(context, () => context);
            requestType = Enforce.NotNullOrEmpty(requestType, () => requestType);
            url = Enforce.NotNullOrEmpty(url, () => url);
            pathTranslated = Enforce.NotNullOrEmpty(pathTranslated, () => pathTranslated);

            // Get the redirect.
            IRedirect redirect = null;

            // Try fetch the Redirect with identifier.
            string id = this.Data.Trim('/');
            if (!id.IsNullOrEmpty() && id.IsGuid())
            {
                redirect =
                    this._redirectService.ReadRedirect(
                        SessionFacade.GetSessionValue<IUser>(SessionFacadeKey.CurrentlyLoggedOnUser),
                        id.ToOrDefault<Guid>());

                if (redirect == null)
                {
                    throw new SilkveilException(String.Format(CultureInfo.CurrentCulture,
                        "The id '{0}' is invalid.", id));
                }
            }
            else
            {
                // Try fetch the Redirect with name.
                if (!String.IsNullOrEmpty(id))
                {
                    redirect =
                        this._redirectService.ReadRedirectByName(
                            SessionFacade.GetSessionValue<IUser>(SessionFacadeKey.CurrentlyLoggedOnUser),
                            id);

                    if (redirect == null)
                    {
                        throw new SilkveilException(String.Format(CultureInfo.CurrentCulture,
                            "The name '{0}' is invalid.", id));
                    }
                }
            }

            // No valid guid or name is provided, fail.
            if (redirect == null)
            {
                throw new RedirectNotFoundException(
                    String.Format(
                        CultureInfo.CurrentCulture, "There was no redirect found for ID '{0}'.", id));
            }

            RedirectHandler handler = new RedirectHandler();

            // Grab the right handler correnspondents to our redirect, if there any source authentication set
            if (redirect.SourceAuthentication != null)
            {
                switch (redirect.SourceAuthentication.AuthenticationType)
                {
                    case AuthenticationType.HttpBasicAuthentication:
                        handler.AuthenticationStrategy = new HttpBasicAuthentication();
                        break;
                    case AuthenticationType.HttpDigestAuthentication:
                        handler.AuthenticationStrategy = new HttpDigestAuthentication();
                        break;
                }

                // Set properties needed for the autentication strategy.
                handler.AuthenticationStrategy.Realm = redirect.Id.ToString();
                handler.AuthenticationStrategy.UserName = redirect.SourceAuthentication.UserName;
                handler.AuthenticationStrategy.Password = redirect.SourceAuthentication.Password;
            }

            // Set the handlers properties.
            handler.Redirect = redirect;

            return handler;
        }
    }
}