namespace SnmpDotNet.Encoding.Types
{
	/// <summary>
	/// Used to specify a value whose range may include both positive and negative numbers. Range = -2e31 to 2e31 - 1 
	/// </summary>
	public class Integer32 : TValue
	{

		public Integer32(byte[] bytes) : base(SnmpTag.Integer32, bytes)
		{
		}
		public Integer32(int value) : this(new Encoder().WriteInteger(value, SnmpTag.Integer32).Encode())
		{
		}
		public int Value => new Decoder(Bytes).ReadInt32(Tag);

		public static int MaxValue => int.MaxValue;
		public static int MinValue => int.MinValue;


	}
}
