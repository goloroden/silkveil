using cherryflavored.net.Contracts.Resources;

using LightCore;

using Microsoft.IdentityModel.Protocols.WSFederation;
using Microsoft.IdentityModel.Web;

using silkveil.net.Contracts.Application;
using silkveil.net.Contracts.Users;
using silkveil.net.Web;

using System;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Principal;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace silkveil.net.Identity.SecurityTokenService.Views
{
    /// <summary>
    /// Represents the logon view.
    /// </summary>
    public class LogonView : ViewBase
    {
        /// <summary>
        /// Contains the logon text box.
        /// </summary>
        private TextBox _loginTextBox;

        /// <summary>
        /// Contains the password text box.
        /// </summary>
        private TextBox _passwordTextBox;

        /// <summary>
        /// Contains the logon button.
        /// </summary>
        private Button _logonButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogonView" /> type.
        /// </summary>
        /// <param name="container">The container.</param>
        public LogonView(IContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Gets whether the current view requires administrative privileges.
        /// </summary>
        /// <value><c>true</c> if the view requires administrative privileges; <c>false</c> otherwise.</value>
        protected override bool RequiresAdministrativePrivileges
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the master page for the current view.
        /// </summary>
        /// <value>The master page.</value>
        protected override Stream MasterPage
        {
            get
            {
                // Load the master page and return it to the caller.
                var resourceManager = this.Container.Resolve<IResourceManager>();
                return
                    resourceManager.GetResource(Assembly.GetExecutingAssembly(), "silkveil.net.Identity.SecurityTokenService.Resources.MasterPage.html");
            }
        }

        /// <summary>
        /// Initializes the view.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnInitCore(EventArgs e)
        {
            this.Title = "silkveil.net | Logon";

            var backgroundImage = new Image();
            backgroundImage.ImageUrl =
                this.Container.Resolve<INavigationService>().GetApplicationHandlerFactoryPath() + "?Action=SecurityTokenService&Data=Background.jpg";
            this.Content.Controls.Add(backgroundImage);
            this.Content.Controls.Add(new LiteralControl("<br />"));

            var loginLabel = new Label();
            loginLabel.Text = "Login:";
            this.Content.Controls.Add(loginLabel);
            this._loginTextBox = new TextBox();
            this.Content.Controls.Add(this._loginTextBox);
            this.Content.Controls.Add(new LiteralControl("<br />"));

            var passwordLabel = new Label();
            passwordLabel.Text = "Password:";
            this.Content.Controls.Add(passwordLabel);
            this._passwordTextBox = new TextBox();
            this._passwordTextBox.TextMode = TextBoxMode.Password;
            this.Content.Controls.Add(this._passwordTextBox);
            this.Content.Controls.Add(new LiteralControl("<br />"));

            this._logonButton = new Button();
            this._logonButton.Text = "Logon";
            this._logonButton.Click += _logonButton_Click;
            this.Content.Controls.Add(this._logonButton);
        }

        /// <summary>
        /// Prepares the render process.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnPreRenderCore(EventArgs e)
        {
            this._loginTextBox.Focus();
        }

        /// <summary>
        /// Handles the click event on the logon button.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void _logonButton_Click(object sender, EventArgs e)
        {
            // Get the user provider.
            var userProvider = this.Container.Resolve<IUserProvider>();

            // Get the principal object for the current user.
            IPrincipal principal;
            try
            {
                principal = userProvider.Logon(this._loginTextBox.Text, this._passwordTextBox.Text);
            }
            catch (SecurityException)
            {
                return;
            }

            // Create the sign in request.
            var signInRequestMessage =
                (SignInRequestMessage)WSFederationMessage.CreateFromUri(this.Request.Url);

            // Create the security token service.
            var securityTokenService =
                new SecurityTokenService(new SecurityTokenServiceConfiguration());

            // Send the sign request to the security token service and get a sign in response.
            var signInResponseMessage =
                FederatedPassiveSecurityTokenServiceOperations.ProcessSignInRequest(
                    signInRequestMessage, principal, securityTokenService);
            
            // Redirect based on the sign in response.
            FederatedPassiveSecurityTokenServiceOperations.ProcessSignInResponse(
                signInResponseMessage, this.Response);
        }
    }
}