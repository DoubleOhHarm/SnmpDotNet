using System.Net;

namespace SnmpDotNet.AsnType
{
    /// <summary>
    /// This type is used to specify an IPv4 address as a string of 4 octets.
    /// </summary>
    public class IpAddress : TValue
    {

        public IpAddress(byte[] bytes) : base(SnmpTag.IpAddress, bytes)
        {
        }
        public IpAddress(IPAddress value) : this(new Encoder().WriteIntegerBytes(value.GetAddressBytes(), SnmpTag.IpAddress).Encode())
        {
        }
        public IpAddress(string ip) : this(IPAddress.Parse(ip)) { }
        public IPAddress Value => new IPAddress(new Decoder(Bytes).ReadIntegerBytes(Tag));



        //public IpAddress(byte[] valueBytes) : base(SnmpTag.Integer32, valueBytes)
        //{
        //    if (valueBytes.Length > 4) throw new Exceptions.InvalidIpAddressException();
        //}
        //public IpAddress(IPAddress value) : this(value.GetAddressBytes())
        //{
        //}
        //public IpAddress(string ip) : this(IPAddress.Parse(ip)) { }
        //public IPAddress Value => new IPAddress(ValueBytes);

        //public override byte[] Encode() => new Encoder().WriteOctetString(ValueBytes, Tag).Encode();


    }
}
