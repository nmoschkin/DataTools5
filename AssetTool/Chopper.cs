using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetTool
{
    public static class Chopper
    {

        public static Bitmap ChopBitmap(string file, int chop)
        {
            int cx, cy;

            var image = Bitmap.FromFile(file);

            cx = image.Width;
            cy = image.Height;

            var bmp = new Bitmap((int)cx, (int)cy - chop);
            var graph = Graphics.FromImage(bmp);

            // uncomment for higher quality output

            graph.InterpolationMode = InterpolationMode.High;
            graph.CompositingQuality = CompositingQuality.HighQuality;
            graph.SmoothingMode = SmoothingMode.AntiAlias;

            System.Drawing.RectangleF rect = new System.Drawing.RectangleF();

            rect.X = 0;
            rect.Y = chop;

            rect.Width = cx;
            rect.Height = cy - chop;

            graph.DrawImage(image, 0, 0, rect, GraphicsUnit.Pixel);
            graph.Dispose();

            return bmp;

        }

        public static void StartChop()
        {
            var frm = new frmChop();

            frm.ShowDialog();

            var dlg = new Microsoft.Win32.OpenFileDialog();

            int chop = frm.ChopAmount;

            dlg.Filter = "All Images Files|*.jpg;*.png;*.bmp";
            dlg.InitialDirectory = Settings.LastBrowseFolder;

            dlg.Multiselect = true;
            dlg.Title = "Chop Images";

            if ((bool)dlg.ShowDialog())
            {
                foreach (var file in dlg.FileNames)
                {
                    var fn = System.IO.Path.GetFileNameWithoutExtension(file);
                    var ext = System.IO.Path.GetExtension(file);

                    fn += "_chopped_" + chop.ToString();

                    var outFile = string.Format("{0}\\{1}{2}", System.IO.Path.GetDirectoryName(file), fn, ext);

                    var bmp = ChopBitmap(file, chop);

                    switch (ext.ToLower())
                    {
                        case ".jpg":

                            bmp.Save(outFile, ImageFormat.Jpeg);
                            break;

                        case ".png":

                            bmp.Save(outFile, ImageFormat.Png);
                            break;

                        case ".bmp":

                            bmp.Save(outFile, ImageFormat.Bmp);
                            break;

                    }

                }
            }

        }


    }
}
