using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using PeterBucher.Web;

namespace silkveil.net.Web.Controls
{
    /// <summary>
    /// Represents a WebControl that displays links for language switching
    /// </summary>
    public class LanguageSwitcher : WebControl
    {
        private List<LanguageInfo> _languageInfo;

        /// <summary>
        /// The main TagKey for the Control
        /// </summary>
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Ul;
            }
        }

        /// <summary>
        /// Gets or sets the current language
        /// Use the setter for highlighting with help of the ActiveCssClass property
        /// </summary>
        public CultureInfo CurrentLanguage
        {
            get;
            set;
        }

        /// <summary>
        /// The CssClass that applies if a TwoLetterISOLanguageName matches the CurrentLanguage value
        /// </summary>
        public string ActiveCssClass
        {
            get;
            set;
        }

        /// <summary>
        /// The QueryString parameter name to append to the links
        /// </summary>
        public string QueryStringParameterName
        {
            get;
            set;
        }

        /// <summary>
        /// LanguageInfo List exposed as InnerProperty for the Controls markup
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Concrete type needed since ASP.NET is not able otherwise to access the list.")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public List<LanguageInfo> LanguageInfo
        {
            get
            {
                if (_languageInfo == null)
                {
                    _languageInfo = new List<LanguageInfo>();
                }

                return _languageInfo;
            }
        }

        /// <summary>
        /// Executes when the Control loads
        /// Creates the language links from the list
        /// </summary>
        /// <param name="e">The EventArgs</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Set the default value for querystring param
            if (this.QueryStringParameterName == null)
            {
                this.QueryStringParameterName = "lng";
            }

            // Create language links
            foreach (LanguageInfo info in _languageInfo)
            {
                this.Controls.Add(CreateLanguageItem(info));
            }
        }

        /// <summary>
        /// Creates an LanguageLink from a LanguageInfo
        /// </summary>
        /// <param name="info">The LanguageInfo</param>
        /// <returns></returns>
        private HtmlGenericControl CreateLanguageItem(LanguageInfo info)
        {
            HtmlGenericControl listItem = new HtmlGenericControl("li");
            HtmlAnchor anchor = new HtmlAnchor();
            listItem.Controls.Add(anchor);

            anchor.InnerText = info.DisplayNameShort;
            anchor.Title = info.DisplayName;
            anchor.HRef = CreateLanguageLink(info.TwoLetterIsoLanguageName);
            if (info.TwoLetterIsoLanguageName == this.CurrentLanguage.TwoLetterISOLanguageName)
            {
                anchor.Attributes.Add("class", this.ActiveCssClass ?? "active");
            }

            return listItem;
        }

        /// <summary>
        /// Creates a language link
        /// </summary>
        /// <param name="language">The language</param>
        /// <returns>A Link with the current Url, the GET-params when there and the language param</returns>
        private string CreateLanguageLink(string language)
        {
            string queryString = WebTools.GetAllQueryStringParams(String.Empty, this.QueryStringParameterName);
            string format = string.Concat(this.Page.Request.Url.AbsolutePath, "?{0}");

            return string.Format(CultureInfo.InvariantCulture,
                format, string.Concat(this.QueryStringParameterName, "=", language, queryString));
        }
    }
}