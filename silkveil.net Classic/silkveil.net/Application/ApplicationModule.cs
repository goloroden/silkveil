using cherryflavored.net.ExtensionMethods.System;
using cherryflavored.net.ExtensionMethods.System.Collections.Generic;

using LightCore;
using LightCore.Configuration;

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Protocols.WSFederation;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Web;

using NLog;

using silkveil.net.Configuration;
using silkveil.net.Contracts.AddIns;
using silkveil.net.Contracts.Application;
using silkveil.net.Contracts.Mappings;
using silkveil.net.Contracts.Providers;
using silkveil.net.Contracts.Redirects;
using silkveil.net.Contracts.Security.Certificates;
using silkveil.net.Contracts.Users;
using silkveil.net.Security;
using silkveil.net.Wcf;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.ServiceModel.Security;
using System.Threading;
using System.Web;
using System.Web.Hosting;

namespace silkveil.net.Application
{
    /// <summary>
    /// Represents the application module that starts up silkveil.
    /// </summary>
    public class ApplicationModule : IHttpModule, IApplicationModule
    {
        /// <summary>
        /// Contains the microkernel.
        /// </summary>
        private static IContainer _container;

        /// <summary>
        /// Contains the logger for this class.
        /// </summary>
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">
        /// An <see cref="T:System.Web.HttpApplication" /> that provides access to the methods,
        /// properties, and events common to all application objects within an ASP.NET application.
        /// </param>
        public void Init(HttpApplication context)
        {
            // Attach a global exception handler.
            context.Error += context_Error;

            // Register the dependencies.
            RegisterDependencies();

            // Setup providers and add-ins.
            SetupProviders();
            SetupAddIns();

            // Activate the WCF services.
            ActivateServices();

            // Subscribe to the interesting events.
            context.BeginRequest += context_BeginRequest;
            context.AuthenticateRequest += context_AuthenticateRequest;
        }

        /// <summary>
        /// Executed when the authentication should be done.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void context_AuthenticateRequest(object sender, EventArgs e)
        {
            // Get the request.
            var request = ((HttpApplication)sender).Request;

            // The redirect back from the security token service is done using a POST method. Hence
            // abort processing of the current request, if it was not sent using POST.
            if (request.HttpMethod != "POST")
            {
                return;
            }

            // Check whether the request contains sign in data.
            if (request.Form["wa"] != WSFederationConstants.Actions.SignIn ||
                request.Form["wresult"].IsNullOrEmpty())
            {
                return;
            }

            // Otherwise, get the security token which is attached to the request, process it and
            // convert it to a principal. First, set up the federatec authentication module.
            var fam =
                new WSFederationAuthenticationModule
                    {
                        ServiceConfiguration =
                            new ServiceConfiguration
                                {
                                    AudienceRestriction = { AudienceMode = AudienceUriMode.Never },
                                    CertificateValidationMode =
                                        X509CertificateValidationMode.Custom,
                                    CertificateValidator = new CertificateValidator(c => true),
                                    IssuerNameRegistry = new X509CertificateIssuerNameRegistry()
                                }
                    };

            // Prepare the decryption by injecting the certificate to the service token resolver.
            var certificates =
                new List<SecurityToken>
                    {
                        new X509SecurityToken(
                            this.Container.Resolve<ICertificateManager>().GetEncryptingCertificate())
                    };
            var encryptedSecurityTokenHandler =
                (from handler in fam.ServiceConfiguration.SecurityTokenHandlers
                 where handler is EncryptedSecurityTokenHandler
                 select handler).First() as EncryptedSecurityTokenHandler;
            encryptedSecurityTokenHandler.Configuration.ServiceTokenResolver =
                SecurityTokenResolver.CreateDefaultSecurityTokenResolver(certificates.AsReadOnly(), false);

            // Get the security token from the request.
            var securityToken = fam.GetSecurityToken(request);

            // Validate the token and convert it to a collection of claims.
            var claims = fam.ServiceConfiguration.SecurityTokenHandlers.ValidateToken(securityToken);

            // Create a principal from the claims.
            IClaimsPrincipal principal = new ClaimsPrincipal(claims);

            // Set the current principal.
            HttpContext.Current.User = principal;
            Thread.CurrentPrincipal = principal;
        }

        /// <summary>
        /// Sets up the providers.
        /// </summary>
        private void SetupProviders()
        {
            // Get the providers.
            var providers =
                new List<IProvider>
                    {
                        _container.Resolve<IUserProvider>(),
                        _container.Resolve<IMappingProvider>(),
                        _container.Resolve<IRedirectProvider>()
                    };

            // Initialize the providers.
            var providerConfigurationData = _container.Resolve<IProviderConfigurationData>();
            providers.ForEach(p => p.Initialize(providerConfigurationData));
        }

        /// <summary>
        /// Registers the dependencies for the microkernel.
        /// </summary>
        private static void RegisterDependencies()
        {
            // Create the container builder.
            var builder = new ContainerBuilder();

            // Load the XML configuration.
            string xmlConfigurationPath =
                Path.Combine(
                    HttpContext.Current.Server.MapPath("~"), "silkveil.Kernel.config");
            builder.RegisterModule(new XamlRegistrationModule(xmlConfigurationPath));

            // Build the microkernel.
            var container = builder.Build();

            // Register the container.
            _container = container;
        }

        /// <summary>
        /// Executed when an unhandled exception occurs.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void context_Error(object sender, EventArgs e)
        {
            // Get the exception.
            Exception exception = HttpContext.Current.Server.GetLastError();

            // Log the exception.
            _log.FatalException(String.Format(CultureInfo.CurrentUICulture,
                "An unhandled exception occured."), exception);

            // Check whether bug reports should be sent to FogBugz. If not, return to the caller.
            if (!SilkveilConfiguration.Instance.SendBugReports)
            {
                return;
            }

            // Send the exception to FogBugz.
            string url = "https://silkveil.fogbugz.com/scoutSubmit.asp";
            string userName = "Golo Roden";
            string project = "Inbox";
            string area = "Undecided";
            string email = "no-reply@silkveil.net";
            bool forceNewBug = true;
            string message = "";
            bool addUserAndMachineInformation = true;
            bool appendAssemblyList = true;
            FogBugz.BugReport.Submit(
                url, userName, project, area, email, forceNewBug, message, exception,
                addUserAndMachineInformation, "{0}.{1}.{2}.{3}", appendAssemblyList);
        }

        /// <summary>
        /// Executes if the current request is processed by ASP.NET on the begin of request.
        /// Rewrites the Url if there IIS is configured for extensionless Urls.
        /// </summary>
        /// <param name="sender">The current <see cref="HttpApplication" />.</param>
        /// <param name="e">The EventArgs.</param>
        private void context_BeginRequest(object sender, EventArgs e)
        {
            // Get the current HTTP context.
            var context = ((HttpApplication)sender).Context;

            // Get the navigation service.
            var navigationService =
                this.Container.Resolve<INavigationService>();

            // Get the url from the current request.
            var currentPath = context.Request.Url;

            // Set up the table with the URI templates.
            var uriTemplateTable = new UriTemplateTable();

            // Set the table's base address.
            uriTemplateTable.BaseAddress = new Uri(navigationService.GetApplicationPath());

            // Set the rules.
            uriTemplateTable.KeyValuePairs.Add(new KeyValuePair<UriTemplate, object>(new UriTemplate(navigationService.GetDownloadUriTemplate() + "{data}"), HandlerAction.Download));
            uriTemplateTable.KeyValuePairs.Add(new KeyValuePair<UriTemplate, object>(new UriTemplate(navigationService.GetRedirectUriTemplate() + "{data}"), HandlerAction.Redirect));
            uriTemplateTable.KeyValuePairs.Add(new KeyValuePair<UriTemplate, object>(new UriTemplate(navigationService.GetSecurityTokenServiceUriTemplate()), HandlerAction.SecurityTokenService));
            uriTemplateTable.KeyValuePairs.Add(new KeyValuePair<UriTemplate, object>(new UriTemplate(navigationService.GetSecurityTokenServiceUriTemplate().TrimEnd('/')), HandlerAction.SecurityTokenService));
            uriTemplateTable.KeyValuePairs.Add(new KeyValuePair<UriTemplate, object>(new UriTemplate(navigationService.GetSecurityTokenServiceUriTemplate() + "{data}"), HandlerAction.SecurityTokenService));
            uriTemplateTable.KeyValuePairs.Add(new KeyValuePair<UriTemplate, object>(new UriTemplate(navigationService.GetUIUriTemplate().TrimEnd('/')), HandlerAction.UI));
            uriTemplateTable.KeyValuePairs.Add(new KeyValuePair<UriTemplate, object>(new UriTemplate(navigationService.GetUIUriTemplate()), HandlerAction.UI));
            uriTemplateTable.KeyValuePairs.Add(new KeyValuePair<UriTemplate, object>(new UriTemplate(navigationService.GetUIUriTemplate() + "{controller}"), HandlerAction.UI));
            uriTemplateTable.KeyValuePairs.Add(new KeyValuePair<UriTemplate, object>(new UriTemplate(navigationService.GetUIUriTemplate() + "{controller}/"), HandlerAction.UI));
            uriTemplateTable.KeyValuePairs.Add(new KeyValuePair<UriTemplate, object>(new UriTemplate(navigationService.GetUIUriTemplate() + "{controller}/{controllerAction}"), HandlerAction.UI));
            uriTemplateTable.KeyValuePairs.Add(new KeyValuePair<UriTemplate, object>(new UriTemplate(navigationService.GetUIUriTemplate() + "{controller}/{controllerAction}/"), HandlerAction.UI));
            uriTemplateTable.KeyValuePairs.Add(new KeyValuePair<UriTemplate, object>(new UriTemplate(navigationService.GetUIUriTemplate() + "{controller}/{controllerAction}/{data}"), HandlerAction.UI));

            // Check whether the current path matches any of these rewrite rules. If not, return.
            UriTemplateMatch uriTemplateMatch = uriTemplateTable.MatchSingle(currentPath);
            if (uriTemplateMatch == null)
            {
                // Before returning, check whether silkveil.net's application handler was called
                // directly. If so, check if SSL is required for silkveil.net, and redirect, if
                // neccessary.
                if (context.Request.Url.GetLeftPart(UriPartial.Path).TrimEnd('/').ToLowerInvariant() ==
                    navigationService.GetApplicationHandlerFactoryPath().TrimEnd('/').ToLowerInvariant())
                {
                    this.Container.Resolve<IProtocolManager>().RedirectIfNeccessary(context.Request);
                }

                // Return to the caller.
                return;
            }

            // Check if SSL is required for silkveil.net, and redirect, if neccessary.
            this.Container.Resolve<IProtocolManager>().RedirectIfNeccessary(context.Request);

            // Otherwise, redirect using the URI match.
            var applicationHandlerFactoryVirtualPath = navigationService.GetApplicationHandlerFactoryVirtualPath();
            switch ((HandlerAction)uriTemplateMatch.Data)
            {
                case HandlerAction.Download:
                    context.RewritePath(string.Format(CultureInfo.InvariantCulture,
                        applicationHandlerFactoryVirtualPath + "?Action={0}&Data={1}", HandlerAction.Download, uriTemplateMatch.BoundVariables["data"]));
                    break;
                case HandlerAction.Redirect:
                    context.RewritePath(string.Format(CultureInfo.InvariantCulture,
                        applicationHandlerFactoryVirtualPath + "?Action={0}&Data={1}", HandlerAction.Redirect, uriTemplateMatch.BoundVariables["data"]));
                    break;
                case HandlerAction.SecurityTokenService:
                    string data = uriTemplateMatch.BoundVariables["data"] ?? "Logon";
                    string wa;
                    switch (data)
                    {
                        case "Logon":
                            wa = WSFederationConstants.Actions.SignIn;
                            break;
                        case "Logoff":
                            wa = WSFederationConstants.Actions.SignOut;
                            break;
                        default:
                            throw new InvalidOperationException(string.Format(CultureInfo.CurrentUICulture,
                                "The data '{0}' is not supported.", data));
                    }
                    var wtrealm = "http://www.silkveil.net/";
                    context.RewritePath(string.Format(CultureInfo.InvariantCulture,
                        applicationHandlerFactoryVirtualPath + "?Action={0}&Data={1}&wa={2}&wtrealm={3}", HandlerAction.SecurityTokenService,
                            data, wa, HttpUtility.UrlEncode(wtrealm)));
                    break;
                case HandlerAction.UI:
                    context.RewritePath(string.Format(CultureInfo.InvariantCulture,
                        applicationHandlerFactoryVirtualPath + "?Action={0}&Controller={1}&ControllerAction={2}&Data={3}", HandlerAction.UI,
                            uriTemplateMatch.BoundVariables["controller"] ?? string.Empty,
                            uriTemplateMatch.BoundVariables["controllerAction"] ?? string.Empty,
                            uriTemplateMatch.BoundVariables["data"] ?? string.Empty));
                    break;
            }
        }

        /// <summary>
        /// Set up the add-ins.
        /// </summary>
        private static void SetupAddIns()
        {
            // Get the add-ins.
            var addIns = _container.ResolveAll<IAddIn>();

            // Initialize the add-ins.
            addIns.ForEach(addIn => addIn.Initialize());
        }

        /// <summary>
        /// Activates the WCF services.
        /// </summary>
        private static void ActivateServices()
        {
            if (!SilkveilConfiguration.Instance.Wcf.ServiceActivation.UseVirtualPathProvider)
            {
                return;
            }

            // Initialize the service activation provider.
            HostingEnvironment.RegisterVirtualPathProvider(new ServiceActivationProvider());
        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements 
        /// <see cref="T:System.Web.IHttpModule" />.
        /// </summary>
        public void Dispose()
        {
            // At the moment, nothing needs to be done here.
        }

        /// <summary>
        /// Gets the microkernel.
        /// </summary>
        /// <value>The microkernel.</value>
        public IContainer Container
        {
            get
            {
                return _container;
            }
        }
    }
}