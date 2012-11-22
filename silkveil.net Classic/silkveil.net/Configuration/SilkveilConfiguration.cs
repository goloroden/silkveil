using cherryflavored.net.Web.Configuration;

using System.Collections.Generic;
using System.Configuration;

namespace silkveil.net.Configuration
{
    /// <summary>
    /// Represents the current configuration.
    /// </summary>
    public class SilkveilConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SilkveilConfiguration" /> type.
        /// </summary>
        public SilkveilConfiguration()
        {
            this.Rewrites = new List<Rewrite>();
        }

        /// <summary>
        /// Gets the configuration instance.
        /// </summary>
        /// <value>The configuration instance.</value>
        public static SilkveilConfiguration Instance
        {
            get
            {
                XamlConfigSectionHandler configSectionHandler =
                    (XamlConfigSectionHandler)ConfigurationManager.GetSection("SilkveilConfiguration");
                return configSectionHandler.GetInstance<SilkveilConfiguration>();
            }
        }

        /// <summary>
        /// Gets or sets the virtual path to the application handler factory.
        /// </summary>
        /// <value>The virtual path to the application handler factory.</value>
        public string ApplicationHandlerFactoryVirtualPath
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether bug reports should be sent.
        /// </summary>
        /// <value><c>true</c> if bug reports should be sent; <c>false</c> otherwise.</value>
        public bool SendBugReports
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the rewrite settings.
        /// </summary>
        /// <value>The rewrite settings.</value>
        public List<Rewrite> Rewrites
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the WCF settings.
        /// </summary>
        /// <value>The WCF settings.</value>
        public Wcf Wcf
        {
            get;
            set;
        }
    }
}