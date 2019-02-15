using System;

namespace SectionalCropper.Controllers
{
    internal static class DirectoryController
    {
        internal static bool CreateIfMissing(string directoryPath)
        {
            if (System.IO.Directory.Exists(directoryPath)) return true;
            try
            {
                System.IO.Directory.CreateDirectory(directoryPath);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
