using cherryflavored.net;

using LightCore;

using silkveil.net.Constraints;
using silkveil.net.Contracts;
using silkveil.net.Contracts.AddIns;
using silkveil.net.Contracts.Mappings;

using System;
using System.Globalization;

namespace silkveil.net.DataSources
{
    /// <summary>
    /// Represents the factory for data sources.
    /// </summary>
    public static class DataSourceFactory
    {
        /// <summary>
        /// Gets the data source for the specified mapping.
        /// </summary>
        /// <param name="mapping">The requested mapping.</param>
        /// <param name="container">The container.</param>
        /// <returns>The data source.</returns>
        public static IDataSource GetMappedDataSource(IMapping mapping, IContainer container)
        {
            mapping = Enforce.NotNull(mapping, () => mapping);

            // Depending on the protocol of the mapping, create the appropriate data source. If the
            // protocol is not supported, throw an exception.
            IDataSource dataSource;
            switch (mapping.Protocol)
            {
                case Protocol.File:
                    dataSource = new FileDataSource(container, mapping);
                    break;
                case Protocol.Http:
                    dataSource = new HttpDataSource(container, mapping);
                    break;
                case Protocol.Https:
                    dataSource = new HttpsDataSource(container, mapping);
                    break;
                case Protocol.Ftp:
                    dataSource = new FtpDataSource(container, mapping);
                    break;
                default:
                    throw new ProtocolNotSupportedException(
                        String.Format(CultureInfo.CurrentCulture,
                            "The protocol '{0}' is not supported.", mapping.Protocol));
            }

            // Attach the constraints to the data source.
            foreach (ConstraintBase constraint in mapping.Constraints)
            {
                dataSource.DownloadStarting += constraint.Validate;
            }

            // Attach the add-ins to the data source.
            foreach (var addin in container.ResolveAll<IAddIn>())
            {
                // Wire up simple events.
                dataSource.DownloadStarting += addin.OnDownloadStarting;
                dataSource.DownloadFinished += addin.OnDownloadFinished;

                // Some events require heavy data load. Wire them only if the event is implemented
                // within the add-in.
                if(addin.ImplementsOnDownloadVerifying)
                {
                    dataSource.DownloadVerifying += addin.OnDownloadVerifying;
                }
            }

            // Return the data source to the caller.
            return dataSource;
        }
    }
}