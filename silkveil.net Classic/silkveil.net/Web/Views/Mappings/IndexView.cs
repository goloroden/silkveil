using cherryflavored.net.Contracts.Resources;

using LightCore;

using PeterBucher.Web.UI.WebControls;

using silkveil.net.Web.Controllers;

using System;
using System.IO;
using System.Reflection;

namespace silkveil.net.Web.Views.Mappings
{
    /// <summary>
    /// Represents the index page of the mappings UI.
    /// </summary>
    public class IndexView : ViewBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IndexView"/> type.
        /// </summary>
        /// <param name="container">The container.</param>
        public IndexView(IContainer container)
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
                return true;
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
                    resourceManager.GetResource(Assembly.GetExecutingAssembly(), "silkveil.net.Web.Resources.MasterPage.html");
            }
        }

        /// <summary>
        /// Initializes the view.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnInitCore(EventArgs e)
        {
            this.Title = "silkveil.net | Administration | Mappings";

            XList mappingsList = new XList();
            this.Content.Controls.Add(mappingsList);

            mappingsList.DataTextField = "Name";
            mappingsList.DataNavigateUrlFormatString =
                this.GenerateLinkTo<MappingsController>() + "{0}/";

            mappingsList.DataNavigateUrlFields = new[] { "Name" };

            mappingsList.DataSource = this.MappingService.ReadMappings(this.CurrentlyLoggedOnUser);
            mappingsList.DataBind();
        }
    }
}