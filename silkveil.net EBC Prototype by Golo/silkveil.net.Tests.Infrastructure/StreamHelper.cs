using System.IO;

namespace silkveil.net.Tests.Infrastructure
{
    public static class StreamHelper
    {
        public static MemoryStream GetStream(int size)
        {
            var stream = new MemoryStream();
            for (int i = 0; i < size; i++)
            {
                stream.WriteByte(0);
            }

            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}