using System;
using SectionalCropper.Models;

namespace SectionalCropper.Controllers
{
    internal static class LoadController
    {
        internal static void Load()
        {
            var inputDirectory = AppContext.BaseDirectory + @"input\";
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
