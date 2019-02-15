using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using SectionalCropper.Models;

namespace SectionalCropper.Controllers
{
    internal static class OutputController
    {
        internal static void Output()
        {
            var outputDirectory = AppContext.BaseDirectory + @"output\";
            DirectoryController.CreateIfMissing(outputDirectory);
            foreach (var frame in Frame.Frames)
            {
                var bmpInput = new Bitmap(frame.ImageSource);
                bmpInput = bmpInput.Clone(new Rectangle(Convert.ToInt32(frame.Rectangle.Left), Convert.ToInt32(frame.Rectangle.Top), Convert.ToInt32(frame.Rectangle.Width), Convert.ToInt32(frame.Rectangle.Height)), PixelFormat.DontCare);
                var brush = new SolidBrush(Color.Fuchsia);
                var scale = Math.Min(Convert.ToDouble(OutputParameters.Width) / Convert.ToDouble(bmpInput.Width), Convert.ToDouble(OutputParameters.Height) / Convert.ToDouble(bmpInput.Height));
                var bmpOutput = new Bitmap(OutputParameters.Width, OutputParameters.Height);
                var graph = Graphics.FromImage(bmpOutput);
                graph.InterpolationMode = InterpolationMode.High;
                graph.CompositingQuality = CompositingQuality.HighQuality;
                graph.SmoothingMode = SmoothingMode.AntiAlias;
                var scaleWidth = (int)(bmpInput.Width * scale);
                var scaleHeight = (int)(bmpInput.Height * scale);
                graph.FillRectangle(brush, new RectangleF(0, 0, OutputParameters.Width, OutputParameters.Height));
                graph.DrawImage(bmpInput, (OutputParameters.Width - scaleWidth) / 2, (OutputParameters.Height - scaleHeight) / 2, scaleWidth, scaleHeight);
                bmpOutput.Save(outputDirectory + $"{frame.Index}.png", ImageFormat.Png);
            }
        }
    }
}
