namespace Invoicing.Common.Contracts
{
    public interface ISerializable
    {
        void SerializeToFile(string pathFile);
        string SerializeToString();
    }
}