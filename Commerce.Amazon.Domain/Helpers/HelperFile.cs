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
    }
}
