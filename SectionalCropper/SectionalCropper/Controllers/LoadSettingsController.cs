using System;
using SectionalCropper.Models;
using System.Collections.Generic;
using System.IO;

namespace SectionalCropper.Controllers
{
    internal static class LoadSettingsController
    {
        internal static void Load()
        {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\SectionalCropper\";
            DirectoryController.CreateIfMissing(directory);
            using (var sr = new StreamReader(directory + "parameters.csv"))
            {
                while(sr.Peek() != -1)
                {
                    var data = sr.ReadLine().Split(',');
                    var frame = Frame.Frames[Convert.ToInt32(data[0])];
                    frame.IsKey = true;
                    frame.SetRectangle(Frame.RectangleVariables.Left, Convert.ToDouble(data[1]));
                    frame.SetRectangle(Frame.RectangleVariables.Top, Convert.ToDouble(data[2]));
                    frame.SetRectangle(Frame.RectangleVariables.Width, Convert.ToDouble(data[3]));
                    frame.SetRectangle(Frame.RectangleVariables.Height, Convert.ToDouble(data[4]));
                }
                sr.Close();
            }
        }
    }
}
