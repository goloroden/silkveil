using System.Runtime.Serialization;

namespace silkveil.net.Contracts.Mappings
{
    /// <summary>
    /// Represents the protocols that can be used for downloads.
    /// </summary>
    [DataContract]
    public enum Protocol
    {
        /// <summary>
        /// The file:// protocol.
        /// </summary>
        [EnumMember]
        File,

        /// <summary>
        /// The http:// protocol.
        /// </summary>
        [EnumMember]
        Http,

        /// <summary>
        /// The https:// protocol.
        /// </summary>
        [EnumMember]
        Https,

        /// <summary>
        /// The ftp:// protocol
        /// </summary>
        [EnumMember]
        Ftp
    }
}