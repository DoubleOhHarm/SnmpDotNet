namespace SnmpDotNet.Encoding.Types
{
	/// <summary>
	/// Used to identify a type that has an assigned object identifier value 
	/// </summary>
	public class Oid : TValue
	{
		public Oid(byte[] bytes) : base(SnmpTag.Oid, bytes)
		{
		}
		public Oid(string value) : this(new Encoder().WriteOid(value, SnmpTag.Oid).Encode())
		{
		}
		public string Value => new Decoder(Bytes).ReadOid(Tag);

	}
}
