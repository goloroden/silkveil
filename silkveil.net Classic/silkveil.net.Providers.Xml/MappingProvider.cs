using cherryflavored.net;
using cherryflavored.net.Contracts.Resources;
using cherryflavored.net.ExtensionMethods.System;
using cherryflavored.net.ExtensionMethods.System.IO;

using LightCore;

using silkveil.net.Contracts.Constraints;
using silkveil.net.Contracts.Mappings;
using silkveil.net.Contracts.Providers;
using silkveil.net.Contracts.Users;
using silkveil.net.Providers.Mappings;

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;
using System.Xml.Linq;

namespace silkveil.net.Providers.Xml
{
    /// <summary>
    /// Represents a mapping provider that works on an XML file.
    /// </summary>
    public class MappingProvider : MappingProviderBase<XElement>
    {
        /// <summary>
        /// Contains the XML namespace for the mappings configuration file.
        /// </summary>
        private readonly XNamespace _mappingNamespace = XNamespace.Get("http://schemas.silkveil.net/mappings/");

        /// <summary>
        /// Contains the path to the configuration file including the file name.
        /// </summary>
        private string _configurationFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingProvider" /> type.
        /// </summary>
        /// <param name="container">The container.</param>
        public MappingProvider(IContainer container)
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
                // Check whether the configuration file exists.
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
                Path.Combine(HttpContext.Current.Server.MapPath(configurationData["Path"]), "Mappings.xml");
        }

        /// <summary>
        /// Initializes the configuration.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        protected override void RunFirstTimeSetup(IProviderConfigurationData configurationData)
        {
            // Create the configuration file.
            using (var userStream =
                this.Container.Resolve<IResourceManager>().GetResource(
                    Assembly.GetExecutingAssembly(), "silkveil.net.Providers.Xml.Mappings.xml"))
            {
                using (var fileStream = new FileStream(this._configurationFile, FileMode.CreateNew, FileAccess.Write))
                {
                    userStream.CopyTo(fileStream);
                }
            }
        }

        /// <summary>
        /// Gets the mapping data context.
        /// </summary>
        /// <returns>The mapping data context.</returns>
        protected override XElement GetMappingDataContext()
        {
            return XElement.Load(this._configurationFile);
        }

        /// <summary>
        /// Saves the mapping data context.
        /// </summary>
        /// <returns>The mapping data context.</returns>
        protected override void SaveMappingDataContext(XElement mappingDataContext)
        {
            mappingDataContext.Save(this._configurationFile);
        }

        /// <summary>
        /// Disposes the mapping data context.
        /// </summary>
        /// <returns>The mapping data context.</returns>
        protected override void DisposeMappingDataContext(XElement mappingDataContext)
        {
        }

        /// <summary>
        /// Reads all mappings.
        /// </summary>
        /// <param name="mappingDataContext">The mapping data context.</param>
        /// <returns>The mappings.</returns>
        protected override IEnumerable<IMapping> ReadMappings(XElement mappingDataContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads all mappings for the specified user.
        /// </summary>
        /// <param name="mappingDataContext">The mapping data context.</param>
        /// <param name="user">The user.</param>
        /// <returns>The mappings.</returns>
        protected override IEnumerable<IMapping> ReadMappings(XElement mappingDataContext, IUser user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads the mapping with the specified ID.
        /// </summary>
        /// <param name="mappingDataContext">The mapping data context.</param>
        /// <param name="id">The ID.</param>
        /// <returns>The mapping.</returns>
        protected override IMapping ReadMapping(XElement mappingDataContext, Guid id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads the mapping with the specified name.
        /// </summary>
        /// <param name="mappingDataContext">The mapping data context.</param>
        /// <param name="name">The name.</param>
        /// <returns>The mapping.</returns>
        protected override IMapping ReadMappingByName(XElement mappingDataContext, string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates the specified mapping and returns the ID for the download.
        /// </summary>
        /// <param name="mappingDataContext">The mapping data context.</param>
        /// <param name="mapping">The mapping.</param>
        /// <returns>The mapping.</returns>
        protected override IMapping CreateMapping(XElement mappingDataContext, IMapping mapping)
        {
            // Create the new user.
            XElement mappingAsXml = this.ToXml(mapping);

            // Add the new mapping to the file.
            mappingDataContext.Add(mappingAsXml);

            // Return the mapping to the caller.
            return this.ToMapping(mappingAsXml);
        }

        /// <summary>
        /// Deletes the specified mapping.
        /// </summary>
        /// <param name="mappingDataContext">The mapping data context.</param>
        /// <param name="mapping">The mapping.</param>
        protected override void DeleteMapping(XElement mappingDataContext, IMapping mapping)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the specified mapping.
        /// </summary>
        /// <param name="mappingDataContext">The mapping data context.</param>
        /// <param name="mapping">The mapping.</param>
        /// <returns>The mapping.</returns>
        protected override IMapping UpdateMapping(XElement mappingDataContext, IMapping mapping)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts the specified <see cref="XElement"/> to an <see cref="IMapping"/>.
        /// </summary>
        /// <param name="mappingAsXml">The mapping as <see cref="XElement"/>.</param>
        /// <returns>The mapping as <see cref="IMapping"/>.</returns>
        private IMapping ToMapping(XElement mappingAsXml)
        {
            XElement source = mappingAsXml.Element(_mappingNamespace + "source");
            XElement target = mappingAsXml.Element(_mappingNamespace + "target");

            Guid id = source.Attribute("id").Value.ToOrDefault<Guid>();
            string name = source.Attribute("name").Value;

            Protocol protocol = target.Attribute("protocol").Value.ToOrDefault<Protocol>();
            Uri uri = target.Attribute("uri").Value.ToOrDefault<Uri>();
            string mimeType = target.Attribute("mimeType").Value;
            bool forceDownload = target.Attribute("forceDownload").Value.ToOrDefault<bool>();
            string fileName = target.Attribute("fileName").Value;

            var mapping = this.Container.Resolve<IMapping>();
            mapping.Id = id;
            mapping.Name = name;
            mapping.Protocol = protocol;
            mapping.Uri = uri;
            mapping.MimeType = mimeType;
            mapping.ForceDownload = forceDownload;
            mapping.FileName = fileName;

            return mapping;
        }

        /// <summary>
        /// Converts the specified <see cref="IMapping" /> to an <see cref="XElement"/>.
        /// </summary>
        /// <param name="mapping">The mapping as <see cref="IMapping"/>.</param>
        /// <returns>The mapping as <see cref="XElement"/>.</returns>
        private XElement ToXml(IMapping mapping)
        {
            XElement mappingAsXml =
                new XElement(_mappingNamespace + "mapping",
                    new XElement(_mappingNamespace + "source",
                        new XAttribute("name", mapping.Name),
                        new XAttribute("id", mapping.Id),
                        new XElement(_mappingNamespace + "authentication")),
                    new XElement(_mappingNamespace + "target",
                        new XAttribute("protocol", mapping.Protocol),
                        new XAttribute("uri", mapping.Uri),
                        new XAttribute("mimeType", mapping.MimeType),
                        new XAttribute("forceDownload", mapping.ForceDownload),
                        new XAttribute("fileName", mapping.FileName),
                        new XElement(_mappingNamespace + "authentication")),
                    new XElement(_mappingNamespace + "constraints"));

            foreach (IConstraint constraint in mapping.Constraints)
            {
                switch (constraint.GetType().FullName)
                {
                    case "":
                        break;
                }
            }

            return mappingAsXml;
        }
    }
}