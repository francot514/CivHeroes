using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace CardManager
{
    public static class ImageResizer
    {

        public static void SaveImage(string imagePath, string savedName,int width = 0, int height = 0)
        {
            Image originalImage = Image.FromFile(imagePath);
            string filePath = AppDomain.CurrentDomain.BaseDirectory + savedName;

            if (width > 0 && height > 0)
            {
                var myCallback =
                new Image.GetThumbnailImageAbort(ThumbnailCallback);
                var imageToSave = originalImage.GetThumbnailImage
                    (width, height, myCallback, IntPtr.Zero);
                imageToSave.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else
            {
                originalImage.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }

        public static void SaveImage(Image image, string savedName, int width = 0, int height = 0)
        {
            Image originalImage = image;
            string filePath = AppDomain.CurrentDomain.BaseDirectory + savedName;

            if (width > 0 && height > 0)
            {
                var myCallback =
                new Image.GetThumbnailImageAbort(ThumbnailCallback);
                Image imageToSave = originalImage.GetThumbnailImage
                    (width, height, myCallback, IntPtr.Zero);
                imageToSave.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else
            {
                originalImage.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }

        private static bool ThumbnailCallback() { return false; }

        public static byte[] ImageToBinary(string imagePath)
        {
            var fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            var buffer = new byte[fileStream.Length];
            fileStream.Read(buffer, 0, (int)fileStream.Length);
            fileStream.Close();
            return buffer;
        }
        public static string OpenFileWindow(string title, string startpath, string filefilter)
        {
            var dialog = new OpenFileDialog();
            dialog.InitialDirectory = startpath;
            dialog.Title = title;
            dialog.Filter = filefilter;
            dialog.Multiselect = true;
            if ((dialog.ShowDialog() == DialogResult.OK))
            {
                return dialog.FileName;
            }
            return null;
        }

    }
}
