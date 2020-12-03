using Commerce.Amazon.Domain.Config;
using System.IO;

namespace Commerce.Amazon.Domain.Helpers
{
    public class HelperFile
    {
        public static string GenerateFullPathScreen(string filename, string userId)
        {
            //string uploadTo = Path.Combine(GlobalConfiguration.Setting.FolderComments, userId, filename);
            string uploadTo = Path.Combine("/images/screen", userId, Path.GetFileName(filename));
            return uploadTo;
        }

        public static string GeneratePathScreen(string filename, string userId)
        {
            //string uploadTo = Path.Combine(GlobalConfiguration.Setting.FolderComments, userId, filename);
            string uploadTo = Path.Combine("wwwroot/images/screen", userId, Path.GetFileName(filename));
            return uploadTo;
        }

        public static string GetDirectoryScreen(string userId)
        {
            //string directory = Path.Combine(GlobalConfiguration.Setting.FolderComments, userId);
            string directory = Path.Combine("wwwroot/images/screen", userId);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            return directory;
        }

        public static bool CreateDirectoryIfNotExists(string userId, out string directory)
        {
            bool create = false;
            //string directory = Path.Combine(GlobalConfiguration.Setting.FolderComments, userId);
            directory = Path.Combine("wwwroot/images/screen", userId);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                create = true;
            }
            return create;
        }
    }
}
