namespace SnmpDotNet.Encoding.Types
{
	/// <summary>
	/// Used to specify a value whose range includes only non-negative integers (0..4294967295). 
	/// </summary>
	public class Unsigned32 : TValue
	{
		public Unsigned32(byte[] bytes) : base(SnmpTag.Unsigned32, bytes)
		{
		}
		public Unsigned32(uint value) : this(new Encoder().WriteIntegerUnsigned(value, SnmpTag.Unsigned32).Encode())
		{
		}
		public uint Value => new Decoder(Bytes).ReadUInt32(Tag);
		public static uint MaxValue => uint.MaxValue;
		public static uint MinValue => uint.MinValue;
	}
}
