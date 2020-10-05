namespace Gesisa.SiiCore.Tools.Contracts
{
	public interface IXmlHelper
	{
		T Deserialize<T>(string pXmlFilename);

	}
}
