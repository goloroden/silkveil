using LightCore;

using PeterBucher.Tools;
using PeterBucher.Web.UI.WebControls;

using silkveil.net.Contracts;
using silkveil.net.Contracts.Application;
using silkveil.net.Contracts.Services;
using silkveil.net.Contracts.Users;
using silkveil.net.Web.Controllers;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace silkveil.net.Web
{
    ///<summary>
    /// Represents the base class for silkveil.net views.
    ///</summary>
    public abstract class ViewBase : Page, IRequiresSessionState
    {
        /// <summary>
        /// Gets or sets the title placeholder.
        /// </summary>
        /// <value>The title placeholder.</value>
        private PlaceHolder TitleInternal
        {
            get;
            set;
        }

        /// <summary>
        /// Gets whether the current view requires administrative privileges.
        /// </summary>
        /// <value><c>true</c> if the view requires administrative privileges; <c>false</c> otherwise.</value>
        protected abstract bool RequiresAdministrativePrivileges
        {
            get;
        }

        /// <summary>
        /// Gets the master page for the current view.
        /// </summary>
        /// <value>The master page.</value>
        protected abstract Stream MasterPage
        {
            get;
        }

        /// <summary>
        /// Contains the navigation service.
        /// </summary>
        private INavigationService _navigationService;

        /// <summary>
        /// Gets or sets the mapping service.
        /// </summary>
        /// <value>The mapping service.</value>
        protected IMappingService MappingService
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the redirect service.
        /// </summary>
        /// <value>The redirect service.</value>
        protected IRedirectService RedirectService
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the currently logged on user.
        /// </summary>
        /// <value>The currently logged on user.</value>
        public IUser CurrentlyLoggedOnUser
        {
            get
            {
                return SessionFacade.GetSessionValue<IUser>(SessionFacadeKey.CurrentlyLoggedOnUser);
            }
        }

        /// <summary>
        /// Gets or sets the title of the view.
        /// </summary>
        /// <value>The title.</value>
        public new string Title
        {
            get
            {
                return (this.TitleInternal.Controls.Count > 0)
                           ? ((LiteralControl)this.TitleInternal.Controls[0]).Text
                           : "";
            }

            set
            {
                this.TitleInternal.Controls.Clear();
                this.TitleInternal.Controls.Add(new LiteralControl(value));
            }
        }

        /// <summary>
        /// Gets the breadcrumb.
        /// </summary>
        /// <value>The breadcrumb.</value>
        public PlaceHolder BreadCrumb
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the menu placeholder.
        /// </summary>
        /// <value>The menu placeholder.</value>
        public PlaceHolder Menu
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the content placeholder.
        /// </summary>
        /// <value>The content placeholder.</value>
        public PlaceHolder Content
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>The container.</value>
        protected IContainer Container
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="View" /> type.
        /// </summary>
        /// <param name="container">The container.</param>
        protected ViewBase(IContainer container)
        {
            this.Container = container;
            this.MappingService = container.Resolve<IMappingService>();
            this.RedirectService = container.Resolve<IRedirectService>();
            this._navigationService = container.Resolve<INavigationService>();

            // Setup the placeholders.
            this.BreadCrumb = new PlaceHolder();
            this.Menu = new PlaceHolder();
            this.Content = new PlaceHolder();
            this.TitleInternal = new PlaceHolder();
        }

        /// <summary>
        /// Redirects the view to the specified MVC url.
        /// Specified controller, default action.
        /// </summary>
        protected void RedirectTo<TController>() where TController : IController
        {
            this.RedirectTo<TController>(c => c.Index());
        }

        /// <summary>
        /// Redirects the view to the specified MVC url.
        /// Specified controller, action and optionally parameters.
        /// </summary>
        /// <param name="urlExpression">The MVC url expression (As anonymous method).</param>
        protected void RedirectTo<TController>(Expression<Action<TController>> urlExpression) where TController : IController
        {
            // Redirects to the specified page and ends the current page`s execution.
            this.Response.Redirect(
                GenerateLinkTo(urlExpression), true);
        }

        /// <summary>
        /// Generates a link from a lambda urlExpression with specified controller.
        /// </summary>
        /// <typeparam name="TController">The controller to use.</typeparam>
        /// <returns>The link that drives to you to the appropriate controller with his default action.</returns>
        protected string GenerateLinkTo<TController>() where TController : IController
        {
            return GenerateLinkTo<TController>(c => c.Index());
        }

        /// <summary>
        /// Generates a link from a lambda urlExpression with specified controller and action method call.
        /// </summary>
        /// <typeparam name="TController">The controller to use.</typeparam>
        /// <param name="urlExpression">The method call on the controller that should be executed.</param>
        /// <returns>The link that drives to you to the appropriate controller action.</returns>
        protected string GenerateLinkTo<TController>(Expression<Action<TController>> urlExpression) where TController : IController
        {
            var methodCall = urlExpression.Body as MethodCallExpression;

            if (methodCall == null)
            {
                throw new ArgumentException("The specified expression does not contain a method call.");
            }

            var arguments = methodCall.Arguments;
            bool argumentsAvailable = arguments.Any();

            string controllerName = typeof(TController).Name.Replace("Controller", String.Empty);
            string actionName = methodCall.Method.Name;

            string linkFormat = string.Concat(this._navigationService.GetUIPath() + "{0}/");
            string linkFormatWithAction = string.Concat(linkFormat, "{1}/");
            string linkFormatWithParameter = string.Concat(linkFormatWithAction, "{2}");

            string link;

            if (actionName == "Index" && !argumentsAvailable)
            {
                link = string.Format(linkFormat, controllerName);
            }
            else if (!argumentsAvailable)
            {
                link = string.Format(linkFormatWithAction, controllerName, actionName);
            }
            else
            {
                object dataParameter = ((ConstantExpression)arguments.First()).Value;

                link = string.Format(linkFormatWithParameter,
                                     controllerName,
                                     actionName,
                                     dataParameter);
            }

            return link;
        }

        /// <summary>
        /// Initializes the view.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected sealed override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // Check whether the current user is allowed to enter the page. If not, redirect
            // to the logon page.
            if (this.RequiresAdministrativePrivileges && !this.CurrentlyLoggedOnUser.IsAdministrator)
            {
                this.Response.Redirect(
                    this._navigationService.GetSecurityTokenServicePath() + "Logon", true);
            }

            // Parse the master page.
            this.ParseMasterPage();

            // Call the template method.
            this.OnInitCore(e);

            // Build up the menu
            XList menu = new XList();
            menu.ActiveCssClass = "active";
            menu.ActiveValue = this.Request.RawUrl;
            menu.ActiveNotClickable = true;
            menu.ValueComparison = ValueComparison.IgnoreRight;

            this.Menu.Controls.Add(menu);

            var items = new List<XListItem>
                            {
                                new XListItem
                                    {
                                        Text = "Dashboard",
                                        NavigateUrl = this.GenerateLinkTo<HomeController>()
                                    },
                                new XListItem
                                    {
                                        Text = "Mappings",
                                        NavigateUrl =
                                            this.GenerateLinkTo<MappingsController>()
                                    },
                                new XListItem
                                    {
                                        Text = "Redirects",
                                        NavigateUrl =
                                            this.GenerateLinkTo<RedirectsController>()
                                    },
                                new XListItem
                                    {
                                        Text = "ResourceController",
                                        NavigateUrl = this.GenerateLinkTo<ResourcesController>(c => c.Index()),
                                        Items = new List<XListItem>()
                                                    {
                                                        new XListItem
                                                            {
                                                                Text = "style.css",
                                                                NavigateUrl =
                                                                    this.GenerateLinkTo<ResourcesController>(
                                                                    c => c.Index("Stylesheet.css"))
                                                            },
                                                        new XListItem
                                                            {
                                                                Text = "MasterPage.html",
                                                                NavigateUrl =
                                                                    this.GenerateLinkTo<ResourcesController>(
                                                                    c => c.Index("MasterPage.html"))
                                                            }
                                                    }
                                    }
                            };

            items.ForEach(item => menu.Items.Add(item));

            menu.DataBind();

            BreadCrumb breadCrumb = new BreadCrumb
                                        {
                                            ShowStartSite = false,
                                            Menu = menu,
                                            PreHtml = "->",
                                            Seperator = "/"
                                        };

            this.BreadCrumb.Controls.Add(breadCrumb);
        }

        /// <summary>
        /// Parses the master page.
        /// </summary>
        private void ParseMasterPage()
        {
            // Read the master page.
            string masterPage;
            using(var streamReader = new StreamReader(this.MasterPage))
            {
                masterPage = streamReader.ReadToEnd();
            }

            // Flag that points whether the form was added or not.
            bool isFormAdded = false;

            // Replaces the desired placeholder for the base path with the base path.
            masterPage = masterPage.
                Replace("{AbsoluteAdminPath}", this._navigationService.GetUIPath()).
                Replace("{AbsoluteSecurityTokenServicePath}", this._navigationService.GetSecurityTokenServicePath());

            // Parse the master page and slice it into controls.
            int position = 0;
            while (position < masterPage.Length)
            {
                // Find the next tag.
                int positionOfNextTag = masterPage.IndexOf("<", position);

                // If no next tag was found, put the rest of the string into a literal control
                // and finish.
                if (positionOfNextTag == -1)
                {
                    this.AddLiteralContent(isFormAdded, masterPage.Substring(position));
                    break;
                }

                // Otherwise, if a tag has been found, put everything between the current position
                // and the tag into a literal control, and finally move to the tag.
                this.AddLiteralContent(isFormAdded, masterPage.Substring(position, positionOfNextTag - position));
                position = positionOfNextTag;

                // Check whether there is an appropriate closing of the tag. If not, put the rest
                // of the string into a literal control and finish.
                int positionOfTagClosing = masterPage.IndexOf(">", position);
                if (positionOfTagClosing == -1)
                {
                    this.AddLiteralContent(isFormAdded, masterPage.Substring(position));
                    break;
                }

                // Check whether the found tag is a silkveil tag. If not, leave it as it is, and
                // go to the next iteration.
                string silkveilTagPrefix = "<silkveil:";
                if ((masterPage.Length < position + silkveilTagPrefix.Length) ||
                    (masterPage.Substring(position, silkveilTagPrefix.Length) != silkveilTagPrefix))
                {
                    // Add the tag to the 
                    this.AddLiteralContent(isFormAdded, masterPage.Substring(position, positionOfTagClosing - position + 1));

                    // Check whether the tag was the body tag. If so, add a form immediately.
                    if (masterPage.Substring(position, positionOfTagClosing - position + 1).ToLowerInvariant().StartsWith("<body"))
                    {
                        this.Controls.Add(new HtmlForm());
                        isFormAdded = true;
                    }

                    // Move forward in the master page.
                    position = positionOfTagClosing + 1;
                    continue;
                }

                // If a silkveil tag was found, replace it by a place holder, depending on the name
                // of the silkveil tag.
                string tagName = masterPage.Substring(
                    position + silkveilTagPrefix.Length, positionOfTagClosing - position - silkveilTagPrefix.Length - 1);
                tagName = tagName.Trim(new char[] { '/', ' ' });
                switch (tagName)
                {
                    case "Title":
                        this.Controls.Add(this.TitleInternal);
                        break;
                    case "BreadCrumb":
                        this.Form.Controls.Add(this.BreadCrumb);
                        break;
                    case "Menu":
                        this.Form.Controls.Add(this.Menu);
                        break;
                    case "Content":
                        this.Form.Controls.Add(this.Content);
                        break;
                    default:
                        throw new ElementNotSupportedException(String.Format(CultureInfo.CurrentUICulture,
                                                                             "The element '{0}' is not supported.", tagName));
                }

                // Move forward.
                position = positionOfTagClosing + 1;
            }
        }

        /// <summary>
        /// Adds literal content to the controls collection of the form (if added yet), otherwise to the page.
        /// </summary>
        /// <param name="isFormAdded">Whether the form is yet added to the page.</param>
        /// <param name="literalContent">The literal content to add.</param>
        private void AddLiteralContent(bool isFormAdded, string literalContent)
        {
            LiteralControl literalContentControl = new LiteralControl(literalContent);

            if (isFormAdded)
            {
                this.Form.Controls.Add(literalContentControl);
            }
            else
            {
                this.Controls.Add(literalContentControl);
            }
        }

        /// <summary>
        /// Loads the view.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected sealed override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Call the template method.
            this.OnLoadCore(e);
        }

        /// <summary>
        /// Prerenders the view.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected sealed override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // Set the form action, if needed.
            if (this.Form != null)
            {
                this.Form.Action = this.Request.RawUrl;
            }

            // Call the template method.
            this.OnPreRenderCore(e);
        }

        /// <summary>
        /// Initializes the view.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected virtual void OnInitCore(EventArgs e)
        {
            // Intentionally left blank.
        }

        /// <summary>
        /// Loads the view.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected virtual void OnLoadCore(EventArgs e)
        {
            // Intentionally left blank.
        }

        /// <summary>
        /// Prerenders the view.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected virtual void OnPreRenderCore(EventArgs e)
        {
            // Intentionally left blank.
        }
    }
}