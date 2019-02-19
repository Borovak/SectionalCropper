using System;
using SectionalCropper.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SectionalCropper.Controllers
{
    internal static class SaveSettingsController
    {
        internal static void Save()
        {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\SectionalCropper\";
            DirectoryController.CreateIfMissing(directory);
            var lst = new List<string>();
            foreach (var frame in Frame.Frames.Where(x => x.IsKey))
            {
                lst.Add(string.Join(",", frame.Index.ToString(), frame.Rectangle.X.ToString(), frame.Rectangle.Y.ToString(), frame.Rectangle.Width.ToString(), frame.Rectangle.Height.ToString()));
            }
            using (var sw = new StreamWriter(directory + "parameters.csv"))
            {
                sw.Write(string.Join("\n", lst));
                sw.Close();
            }
        }
    }
}
