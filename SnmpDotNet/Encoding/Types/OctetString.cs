namespace SnmpDotNet.Encoding.Types
{
	/// <summary>
	/// Used to specify octets of binary or textual information. While SMIv1 doesn't limit the number of octets, SMIv2 specifies a limit of 65535 octets. A size may be specified which can be fixed, varying, or multiple ranges.  
	/// </summary>
	public class OctetString : TValue
	{
		public OctetString(byte[] bytes) : base(SnmpTag.OctetString, bytes)
		{
		}
		public OctetString(string value) : this(new Encoder().WriteOctetString(value, SnmpTag.OctetString).Encode())
		{
		}
		public string Value => new Decoder(Bytes).ReadString(Tag);
		public string PhysAddressValue => BitConverter.ToString(new Decoder(Bytes).ReadOctetString(Tag));


	}
}
