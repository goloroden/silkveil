using cherryflavored.net;
using cherryflavored.net.ExtensionMethods.System;

using LightCore;

using silkveil.net.Application;
using silkveil.net.Authentication;
using silkveil.net.Contracts;
using silkveil.net.Contracts.Authentication;
using silkveil.net.Contracts.Mappings;
using silkveil.net.Contracts.Services;
using silkveil.net.Contracts.Users;
using silkveil.net.DataSources;
using silkveil.net.Web;

using System;
using System.Globalization;
using System.Web;

namespace silkveil.net.Downloads
{
    /// <summary>
    /// Represents a factory for the download handlers.
    /// </summary>
    public class DownloadHandlerFactory : HandlerFactoryBase
    {
        /// <summary>
        /// Contains the mapping service.
        /// </summary>
        private IMappingService _mappingService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadHandlerFactory"/> type.
        /// </summary>
        /// <param name="mappingService">The mapping service.</param>
        public DownloadHandlerFactory(IMappingService mappingService)
        {
            this._mappingService = mappingService;
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

            // Get the mapping for the requested name.
            IMapping mapping = null;

            // Try fetch the Mapping with identifier.
            string id = this.Data.Trim('/');

            if (id != null && id.IsGuid())
            {
                mapping =
                    this._mappingService.ReadMapping(
                        SessionFacade.GetSessionValue<IUser>(SessionFacadeKey.CurrentlyLoggedOnUser),
                        id.ToOrDefault<Guid>());

                if (mapping == null)
                {
                    throw new SilkveilException(String.Format(CultureInfo.CurrentCulture,
                        "The id '{0}' is invalid.", id));
                }
            }
            else
            {
                // Try fetch the Mapping with name.
                if (!String.IsNullOrEmpty(id))
                {
                    mapping =
                        this._mappingService.ReadMappingByName(
                            SessionFacade.GetSessionValue<IUser>(SessionFacadeKey.CurrentlyLoggedOnUser),
                            id);

                    if (mapping == null)
                    {
                        throw new SilkveilException(String.Format(CultureInfo.CurrentCulture,
                            "The name '{0}' is invalid.", id));
                    }
                }
            }

            // No valid guid or name is provided, fail.
            if (mapping == null)
            {
                throw new MappingNotFoundException(
                    String.Format(
                        CultureInfo.CurrentCulture, "There was no mapping found for ID '{0}'.", id));
            }

            // Get the data source that holds the download and write the download to the output stream.
            IDataSource dataSource = DataSourceFactory.GetMappedDataSource(mapping, container);

            DownloadHandler handler = new DownloadHandler();

            // Grab the right Handler correnspondents to our Mapping, if there any source authentication set
            if (mapping.SourceAuthentication != null)
            {
                switch (mapping.SourceAuthentication.AuthenticationType)
                {
                    case AuthenticationType.HttpBasicAuthentication:
                        handler.AuthenticationStrategy = new HttpBasicAuthentication();
                        break;
                    case AuthenticationType.HttpDigestAuthentication:
                        handler.AuthenticationStrategy = new HttpDigestAuthentication();
                        break;
                }

                // Set properties needed for the autentication strategy.
                handler.AuthenticationStrategy.Realm = mapping.Id.ToString();
                handler.AuthenticationStrategy.UserName = mapping.SourceAuthentication.UserName;
                handler.AuthenticationStrategy.Password = mapping.SourceAuthentication.Password;
            }

            // Set Mapping and DataSource to the handlers properties
            handler.Mapping = mapping;
            handler.DataSource = dataSource;

            return handler;
        }
    }
}