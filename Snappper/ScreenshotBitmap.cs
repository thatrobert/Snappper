using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Snappper
{
   class Screenshot
   {
      public static void CopyScreen(MainWindow mw)
      {
         System.Drawing.Rectangle bounds = new System.Drawing.Rectangle(Convert.ToInt32(mw.Left) + 1, Convert.ToInt32(mw.Top) + 1, Convert.ToInt32(mw.Width) - 2, Convert.ToInt32(mw.Height) - 2);
         using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
         {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
               g.CopyFromScreen(bounds.Location, System.Drawing.Point.Empty, bounds.Size);
            }
            Bitmap newBitmap = bitmap;
            if (ClipSize.Current.IsRatioMaintained)
               newBitmap = ResizeImage(bitmap, ClipSize.Current.ClipWidth, ClipSize.Current.ClipHeight);
            if (mw.IsToFile)
            {
               string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Snappper");
               if (!Directory.Exists(folder))
                  Directory.CreateDirectory(folder);
               string path = $"{folder}\\snappper-{DateTime.Now:yyyy-MM-dd-hh-mm-ss-ms}.png";
               newBitmap.Save(path, ImageFormat.Png);
            }
            else
            {
               System.Windows.Forms.Clipboard.SetImage(newBitmap);
            }
         }
      }

      private static Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
      {
         var destRect = new Rectangle(0, 0, width, height);
         var destImage = new Bitmap(width, height);

         destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

         using (var graphics = Graphics.FromImage(destImage))
         {
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using (var wrapMode = new ImageAttributes())
            {
               wrapMode.SetWrapMode(WrapMode.TileFlipXY);
               graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            }
         }

         return destImage;
      }

   }
}
