using Commerce.Amazon.Domain.Config;
using System.IO;

namespace Commerce.Amazon.Domain.Helpers
{
    public class HelperFile
    {
        public static string GenerateFullPathScreen(string filename, string userId)
        {
            string uploadTo = Path.Combine(GlobalConfiguration.Setting.FolderComments, userId, filename);
            return uploadTo;
        }

        public static bool CreateDirectoryIfNotExists(string userId)
        {
            bool create = false;
            string directory = Path.Combine(GlobalConfiguration.Setting.FolderComments, userId);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                create = true;
            }
            return create;
        }
    }
}
