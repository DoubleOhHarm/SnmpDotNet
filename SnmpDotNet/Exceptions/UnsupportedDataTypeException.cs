using System.Formats.Asn1;

namespace SnmpDotNet.Exceptions
{
    public class UnsupportedDataTypeException : SnmpException
    {
        public UnsupportedDataTypeException(Asn1Tag tag) : base($"Unsupported data type: {tag}")
        {
        }
    }
}
