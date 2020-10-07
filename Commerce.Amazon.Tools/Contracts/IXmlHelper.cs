namespace Commerce.Amazon.Tools.Contracts
{
    public interface IXmlHelper
    {
        T Deserialize<T>(string pXmlFilename);

    }
}
