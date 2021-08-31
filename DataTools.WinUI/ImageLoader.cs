using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Storage.Streams;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DataTools.WinUI
{
    /// <summary>
    /// Load image for WinUI consumption
    /// </summary>
    public static class ImageLoader
    {

        /// <summary>
        /// Loads an instance of <see cref="ImageSource"/> from a stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="closeStream"></param>
        /// <returns></returns>
        public static async Task<ImageSource> LoadFromStream(System.IO.Stream stream, bool closeStream = true)
        {
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            byte[] buffer = new byte[stream.Length];
            
            stream.Read(buffer, 0, buffer.Length);

            if (closeStream)
            {
                stream.Dispose();
            }

            return await LoadFromBytes(buffer);
        }

        /// <summary>
        /// Loads an instance of <see cref="ImageSource"/> from raw image data.
        /// </summary>
        /// <param name="rawImage">The raw image data to load.</param>
        /// <returns></returns>
        public static async Task<ImageSource> LoadFromBytes(byte[] rawImage)
        {
            byte[] b = rawImage;

            using (var rstream = new InMemoryRandomAccessStream())
            {
                using (var strm = rstream.GetOutputStreamAt(0))
                {
                    DataWriter writer = new DataWriter(strm);
                    writer.WriteBytes(b);
                    await writer.StoreAsync();
                    writer.DetachStream();
                    await rstream.FlushAsync();
                }

                BitmapImage image = new BitmapImage();
                image.SetSource(rstream);

                return image;
            }

        }
    }
}
