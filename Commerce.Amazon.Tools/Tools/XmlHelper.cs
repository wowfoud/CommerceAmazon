using Commerce.Amazon.Tools.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Commerce.Amazon.Tools.Tools
{
    public class XmlHelper : IXmlHelper
    {
        private readonly ILoggerManager _loggerManager;

        public XmlHelper(ILoggerManager loggerManager)
        {
            _loggerManager = loggerManager;
        }

        public T Deserialize<T>(string pXmlFilename)
        {
            _loggerManager.LogInfo($"start Deserialize: XmlFilename ==> {pXmlFilename}");

            T returnObject = default;
            if (string.IsNullOrEmpty(pXmlFilename)) return default;

            try
            {
                using (StreamReader xmlStream = new StreamReader(pXmlFilename, new UTF8Encoding(false)))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    var vResult = serializer.Deserialize(xmlStream);
                    returnObject = (T)vResult;
                }
            }
            catch (Exception ex)
            {
                _loggerManager.LogInfo($"Deserialize: Exception ==> {ex.ToString()}");
                throw;
            }
            return returnObject;
        }
        public void Serialize<T>(T value, string filePath)
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                using (TextWriter wr = new StreamWriter(filePath, false, new UTF8Encoding(false)))
                {
                    xs.Serialize(wr, value);
                }
            }
            catch (Exception ex)
            {
                _loggerManager.LogInfo($"Serialize {filePath}: Exception ==> {ex.ToString()}");
                throw;
            }
        }
    }
}
