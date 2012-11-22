namespace silkveil.net.Web.Controls
{
    /// <summary>
    /// Represents an info object for a language.
    /// </summary>
    public class LanguageInfo
    {
        /// <summary>
        /// Gets or sets the language's short name that should be displayed.
        /// </summary>
        /// <value>The short name.</value>
        public string DisplayNameShort
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the language's full name that should be displayed.
        /// </summary>
        /// <value>The full name.</value>
        public string DisplayName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the two letter ISO language name.
        /// </summary>
        /// <value>The language name.</value>
        /// <remarks>Two letter ISO language names are specified as lowercase, e.g. "en".</remarks>
        public string TwoLetterIsoLanguageName
        {
            get;
            set;
        }
    }
}