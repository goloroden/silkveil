using cherryflavored.net;
using cherryflavored.net.Contracts.Resources;
using cherryflavored.net.ExtensionMethods.System;
using cherryflavored.net.ExtensionMethods.System.IO;
using cherryflavored.net.ExtensionMethods.System.Security.Cryptography;

using LightCore;

using Microsoft.IdentityModel.Claims;

using silkveil.net.Contracts.Providers;
using silkveil.net.Contracts.Security;
using silkveil.net.Providers.Users;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Principal;
using System.Web;
using System.Xml.Linq;

namespace silkveil.net.Providers.Xml
{
    /// <summary>
    /// Represents an XML-based user provider.
    /// </summary>
    public class UserProvider : UserProviderBase<XElement>
    {
        /// <summary>
        /// Contains the XML namespace for the users configuration file.
        /// </summary>
        private readonly XNamespace _userNamespace = XNamespace.Get("http://schemas.silkveil.net/users/");

        /// <summary>
        /// Contains the path to the users configuration file including the file name.
        /// </summary>
        private string _userConfigurationFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProvider" /> type.
        /// </summary>
        /// <param name="container">The container.</param>
        public UserProvider(IContainer container)
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
                // Check whether the configuration files exist.
                return !File.Exists(this._userConfigurationFile);
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
            this._userConfigurationFile =
                Path.Combine(HttpContext.Current.Server.MapPath(configurationData["Path"]), "Users.xml");
        }

        /// <summary>
        /// Initializes the configuration.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        protected override void RunFirstTimeSetup(IProviderConfigurationData configurationData)
        {
            // Create a new users file.
            using (var userStream =
                this.Container.Resolve<IResourceManager>().GetResource(
                    Assembly.GetExecutingAssembly(), "silkveil.net.Providers.Xml.Users.xml"))
            {
                using (var fileStream = new FileStream(this._userConfigurationFile, FileMode.CreateNew, FileAccess.Write))
                {
                    userStream.CopyTo(fileStream);
                }
            }
        }

        /// <summary>
        /// Gets the user data context.
        /// </summary>
        /// <returns>The user data context.</returns>
        protected override XElement GetUserDataContext()
        {
            return XElement.Load(this._userConfigurationFile);
        }

        /// <summary>
        /// Saves the user data context.
        /// </summary>
        /// <returns>The user data context.</returns>
        protected override void SaveUserDataContext(XElement userDataContext)
        {
            userDataContext.Save(this._userConfigurationFile);
        }

        /// <summary>
        /// Disposes the user data context.
        /// </summary>
        /// <returns>The user data context.</returns>
        protected override void DisposeUserDataContext(XElement userDataContext)
        {
        }

        /// <summary>
        /// Logs on the user with the specified credentials.
        /// </summary>
        /// <param name="userDataContext">The user data context.</param>
        /// <param name="login">The login.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        /// An instance of <see cref="IPrincipal" /> if the logon was successful. Otherwise, a
        /// <see cref="SecurityException" /> is thrown.
        /// </returns>
        /// <remarks>The returned principal only contains the name of the user.</remarks>
        protected override IPrincipal Logon(XElement userDataContext, string login, string password)
        {
            // Get all users that match the login and password.
            var users =
                from u in userDataContext.Elements(this._userNamespace, "user")
                where
                    u.Element(this._userNamespace, "credentials").Element(this._userNamespace, "login").Value == login &&
                    u.Element(this._userNamespace, "credentials").Element(this._userNamespace, "password").Value == password.Hash()
                select u;

            // If no or more than one user was found, throw an exception.
            if (users.Count() != 1)
            {
                throw new SecurityException(String.Format(CultureInfo.CurrentUICulture,
                    "Logon denied for login '{0}'.", login));
            }

            // Otherwise, set up a basic principal.
            var principal = new GenericPrincipal(new GenericIdentity(login), new string[0]);

            // Return the principal to the caller.
            return principal;
        }

        /// <summary>
        /// Reads the user's security ID by the login.
        /// </summary>
        /// <param name="userDataContext">The user data context.</param>
        /// <param name="login">The login.</param>
        /// <returns>The security ID.</returns>
        protected override ISecurityId ReadSecurityIdByLogin(XElement userDataContext, string login)
        {
            // Get the security id as GUID from the specified user.
            var id =
                (from u in userDataContext.Elements(this._userNamespace, "user")
                 where u.Element(this._userNamespace, "credentials").Element(this._userNamespace, "login").Value == login
                 select u.Element(this._userNamespace, "securityId").Value).Single().ToOrDefault<Guid>();

            // Create a security id from the GUID.
            var securityId = this.Container.Resolve<ISecurityId>();
            securityId.Value = id;

            // Return the security id to the caller.
            return securityId;
        }

        /// <summary>
        /// Reads the claims for the user with the specified security ID.
        /// </summary>
        /// <param name="userDataContext">The user data context.</param>
        /// <param name="securityId">The security ID.</param>
        /// <returns>A list of claims.</returns>
        protected override IEnumerable<Claim> ReadClaimsBySecurityId(XElement userDataContext, ISecurityId securityId)
        {
            // Get the login.
            var login =
                (from u in userDataContext.Elements(this._userNamespace, "user")
                where u.Element(this._userNamespace, "securityId").Value.ToOrDefault<Guid>() == securityId.Value
                select u.Element(this._userNamespace, "credentials").Element(this._userNamespace, "login").Value).Single();

            // Get the claims as XML.
            var claimsAsXml =
                (from u in userDataContext.Elements(this._userNamespace, "user")
                where u.Element(this._userNamespace, "securityId").Value.ToOrDefault<Guid>() == securityId.Value
                select u).Single().Element(this._userNamespace, "claims").Elements(this._userNamespace, "claim");

            // Create the list of claims.
            var claims = new List<Claim>();

            // Add security id and login.
            claims.Add(new Claim(
                silkveil.net.Contracts.Identity.ClaimTypes.SecurityId, securityId.Value.ToString()));
            claims.Add(new Claim(
                silkveil.net.Contracts.Identity.ClaimTypes.Login, login));

            // Add all other claims.
            foreach(var claimAsXml in claimsAsXml)
            {
                claims.Add(new Claim(
                    claimAsXml.Attribute("type").Value, claimAsXml.Attribute("value").Value));
            }

            // Return the claims to the caller.
            return claims;
        }
    }
}