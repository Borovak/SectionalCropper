using System;
using SectionalCropper.Models;

namespace SectionalCropper.Controllers
{
    internal static class LoadImagesController
    {
        internal static void Load()
        {
            var inputDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\SectionalCropper\input\";
            DirectoryController.CreateIfMissing(inputDirectory);
            Frame.Frames?.Clear();
            var files = System.IO.Directory.GetFiles(inputDirectory);
            foreach (var file in files)
            {
                Frame.Add(file);
            }
            Frame.CurrentIndex = 0;
        }
    }
}
