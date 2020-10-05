using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Gesisa.SiiCore.Tools.Tools
{
    public static class FilesHelper
    {
        public static void SerializeXml<T>(T value, string filePath)
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            XmlSerializer xs = new XmlSerializer(typeof(T));
            using (TextWriter wr = new StreamWriter(filePath, false, new UTF8Encoding(false)))
            {
                xs.Serialize(wr, value);
            }
        }

        public static string Copy(string sourceFile, string destFile)
        {
            if (!Directory.Exists(Path.GetDirectoryName(destFile)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(destFile));
            }
            if (File.Exists(destFile))
            {
                //TODO
                //Change to rename *(#)
                File.Delete(destFile);
            }
            File.Copy(sourceFile, destFile);
            return destFile;
        }


        public static bool Move(string sourceFile, ref string destFile)
        {
            try
            {
                string directory = Path.GetDirectoryName(destFile);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                int i = 0;
                while (File.Exists(destFile))
                {
                    var ext = Path.GetExtension(destFile);
                    var path = Path.GetDirectoryName(destFile);
                    var name = Path.GetFileNameWithoutExtension(destFile);
                    if (i != 0)
                    {
                        name = name.Replace($"({i})", $"({++i})") + ext;
                    }
                    else
                    {
                        name += $" - Copy ({++i})" + ext;
                    }
                    destFile = Path.Combine(path, name);
                }
                if (File.Exists(sourceFile) && sourceFile != destFile)
                {
                    File.Move(sourceFile, destFile);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
