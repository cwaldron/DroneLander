using System.IO;
using System.Threading.Tasks;

namespace CafeLib.Extensions
{
    public static class StreamExtensions
    {
        /// <summary>
        /// Converts stream to byte array.
        /// </summary>
        /// <param name="stream">stream</param>
        /// <returns>byte array</returns>
        public static byte[] ToByteArray(this Stream stream)
        {
            using (var memoryStream = stream.ToMemoryStream())
            {
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Converts stream to byte array.
        /// </summary>
        /// <param name="stream">stream</param>
        /// <returns>byte array</returns>
        public static async Task<byte[]> ToByteArrayAsync(this Stream stream)
        {
            using (var memoryStream = await stream.ToMemoryStreamAsync())
            {
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Converts a stream to a memory stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>memory stream</returns>
        private static MemoryStream ToMemoryStream(this Stream stream)
        {
            var memoryStream = stream as MemoryStream;
            if (memoryStream != null) return memoryStream;

            using (stream)
            {
                using (memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    return memoryStream;
                }
            }
        }

        /// <summary>
        /// Converts a stream to a memory stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>memory stream</returns>
        private static async Task<MemoryStream> ToMemoryStreamAsync(this Stream stream)
        {
            var memoryStream = stream as MemoryStream;
            if (memoryStream != null) return memoryStream;

            using (stream)
            {
                using (memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    return memoryStream;
                }
            }
        }
    }
}
