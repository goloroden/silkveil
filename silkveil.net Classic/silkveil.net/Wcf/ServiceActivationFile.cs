using System.IO;
using System.Web.Hosting;

namespace silkveil.net.Wcf
{
    /// <summary>
    /// Represents a virtual .svc file.
    /// </summary>
    public class ServiceActivationFile : VirtualFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceActivationFile" /> type.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        public ServiceActivationFile(string virtualPath) : base(virtualPath)
        {
        }

        /// <summary>
        /// Returns a read-only stream to the virtual resource.
        /// </summary>
        /// <returns>A read-only stream to the virtual file.</returns>
        public override Stream Open()
        {
            // Create a stream.
            Stream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);

            // Write to the stream.
            writer.Write("<%@ ServiceHost Language=\"C#\" Debug=\"false\" Service=\"silkveil.net.Wcf.Service\" %>");
            writer.Flush();

            // Reset the stream's pointer to its beginning and return the stream to the caller.
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}
