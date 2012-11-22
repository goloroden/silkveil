using cherryflavored.net;

using LightCore;

using silkveil.net.Contracts.Providers;
using silkveil.net.Contracts.Redirects;
using silkveil.net.Contracts.Users;
using silkveil.net.Providers.Redirects;

using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Xml.Linq;

namespace silkveil.net.Providers.Xml
{
    /// <summary>
    /// Represents the XML provider for redirects.
    /// </summary>
    public class RedirectProvider : RedirectProviderBase<XElement>
    {
        /// <summary>
        /// Contains the path to the configuration file including the file name.
        /// </summary>
        private string _configurationFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectProvider" /> type.
        /// </summary>
        /// <param name="container">The container.</param>
        public RedirectProvider(IContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Gets whether this provider is used for the first time.
        /// </summary>
        /// <value><c>true</c> if this provider is used for the first time; <c>false</c> otherwise.</value>
        protected override bool IsUsedForTheFirstTime
        {
            get
            {
                // Check if the configuration file exists.
                return !File.Exists(this._configurationFile);
            }
        }

        /// <summary>
        /// Initializes the provider.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        protected override void InitializeInternal(IProviderConfigurationData configurationData)
        {
            configurationData = Enforce.NotNull(configurationData, () => configurationData);

            // Set the configuration file and resolve the path if necessary.
            this._configurationFile =
                Path.Combine(HttpContext.Current.Server.MapPath(configurationData["Path"]), "Redirects.xml");
        }

        /// <summary>
        /// Initializes the configuration.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        protected override void RunFirstTimeSetup(IProviderConfigurationData configurationData)
        {
            // Create a new redirects file.
            XElement root = new XElement("redirects");
            root.Save(this._configurationFile);
        }

        /// <summary>
        /// Gets the redirect data context.
        /// </summary>
        /// <returns>The redirect data context.</returns>
        protected override XElement GetRedirectDataContext()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves the redirect data context.
        /// </summary>
        /// <returns>The redirect data context.</returns>
        protected override void SaveRedirectDataContext(XElement userDataContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Disposes the redirect data context.
        /// </summary>
        /// <returns>The redirect data context.</returns>
        protected override void DisposeRedirectDataContext(XElement dataContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads all redirects.
        /// </summary>
        /// <param name="redirectDataContext">The redirect data context.</param>
        /// <returns>The redirects.</returns>
        protected override IEnumerable<IRedirect> ReadRedirects(XElement redirectDataContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads all redirects for the specified user.
        /// </summary>
        /// <param name="redirectDataContext">The redirect data context.</param>
        /// <param name="user">The user.</param>
        /// <returns>The redirects.</returns>
        protected override IEnumerable<IRedirect> ReadRedirects(XElement redirectDataContext, IUser user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads the redirect for the specified ID.
        /// </summary>
        /// <param name="redirectDataContext">The redirect data context.</param>
        /// <param name="id">The ID.</param>
        /// <returns>The redirect.</returns>
        protected override IRedirect ReadRedirect(XElement redirectDataContext, Guid id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads the redirect for the specified name.
        /// </summary>
        /// <param name="redirectDataContext">The redirect data context.</param>
        /// <param name="name">The name.</param>
        /// <returns>The redirect.</returns>
        protected override IRedirect ReadRedirectByName(XElement redirectDataContext, string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates the specified redirect.
        /// </summary>
        /// <param name="redirectDataContext">The redirect data context.</param>
        /// <param name="redirect">The redirect.</param>
        /// <returns>The redirect.</returns>
        protected override IRedirect CreateRedirect(XElement redirectDataContext, IRedirect redirect)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes the specified redirect.
        /// </summary>
        /// <param name="redirectDataContext">The redirect data context.</param>
        /// <param name="redirect">The redirect.</param>
        protected override void DeleteRedirect(XElement redirectDataContext, IRedirect redirect)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the specified redirect.
        /// </summary>
        /// <param name="redirectDataContext">The redirect data context.</param>
        /// <param name="redirect">The redirect.</param>
        /// <returns>The redirect.</returns>
        protected override IRedirect UpdateRedirect(XElement redirectDataContext, IRedirect redirect)
        {
            throw new NotImplementedException();
        }
    }
}