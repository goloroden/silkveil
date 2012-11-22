using cherryflavored.net;
using cherryflavored.net.ExtensionMethods.System.IO;

using LightCore;

using silkveil.net.Contracts;
using silkveil.net.Contracts.Application;
using silkveil.net.Contracts.Mappings;

using System;
using System.IO;
using System.Security;
using System.Globalization;
using System.Web.Hosting;

namespace silkveil.net.DataSources
{
    /// <summary>
    /// Represents a data source on the local file system.
    /// </summary>
    public class FileDataSource : DataSourceBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileDataSource"/> type.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="mapping">The mapping.</param>
        public FileDataSource(IContainer container, IMapping mapping)
            : base(container)
        {
            mapping = Enforce.NotNull(mapping, () => mapping);

            // Set the mapping.
            this.Mapping = mapping;
        }

        /// <summary>
        /// Saves the data source to the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        protected override void SaveCore(Stream stream)
        {
            stream = Enforce.NotNull(stream, () => stream);

            // Copy the input stream to the output stream.
            try
            {
                string absolutepath = this.Mapping.Uri.AbsolutePath;

                string rootSign = "~";
                if (!absolutepath.StartsWith(rootSign))
                {
                    absolutepath = string.Concat(rootSign, absolutepath);
                }

                string mapped = HostingEnvironment.MapPath((absolutepath));

                var response = this.Container.Resolve<IRuntimeContext>().HttpResponse;

                using (FileStream inputStream =
                    new FileStream(mapped, FileMode.Open))
                {
                    response.AddHeader("content-length", inputStream.Length.ToString(CultureInfo.InvariantCulture));
                    inputStream.CopyTo(stream,
                        () => response.IsClientConnected,
                        () => this.OnDownloadFinished(new DownloadFinishedEventArgs { Mapping = this.Mapping, State = DownloadFinishedState.Succeeded }),
                        () => this.OnDownloadFinished(new DownloadFinishedEventArgs { Mapping = this.Mapping, State = DownloadFinishedState.Canceled }));
                }
            }
            catch (FileNotFoundException ex)
            {
                throw new SilkveilException(
                    String.Format(CultureInfo.CurrentCulture,
                        "The data source '{0}' could not be read.", this.Mapping.Uri), ex);
            }
            catch (DirectoryNotFoundException ex)
            {
                throw new SilkveilException(
                    String.Format(CultureInfo.CurrentCulture,
                        "The data source '{0}' could not be read.", this.Mapping.Uri), ex);
            }
            catch (PathTooLongException ex)
            {
                throw new SilkveilException(
                    String.Format(CultureInfo.CurrentCulture,
                        "The data source '{0}' could not be read.", this.Mapping.Uri), ex);
            }
            catch (IOException ex)
            {
                throw new SilkveilException(
                    String.Format(CultureInfo.CurrentCulture,
                        "The data source '{0}' could not be read.", this.Mapping.Uri), ex);
            }
            catch (SecurityException ex)
            {
                throw new SilkveilException(
                    String.Format(CultureInfo.CurrentCulture,
                        "The data source '{0}' could not be read.", this.Mapping.Uri), ex);
            }
        }
    }
}