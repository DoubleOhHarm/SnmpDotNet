namespace SnmpDotNet.Encoding.Types
{
	/// <summary>
	/// Used to specify a value which represents a count. The range is 0 to 2e64 -1. 
	/// </summary>
	public class Counter64 : TValue
	{

		public Counter64(byte[] bytes) : base(SnmpTag.Counter64, bytes)
		{
		}
		public Counter64(ulong value) : this(new Encoder().WriteIntegerUnsigned(value, SnmpTag.Counter64).Encode())
		{
		}
		public ulong Value => new Decoder(Bytes).ReadUInt64(Tag);
		public static ulong MaxValue => ulong.MaxValue;
		public static ulong MinValue => ulong.MinValue;

	}
}
