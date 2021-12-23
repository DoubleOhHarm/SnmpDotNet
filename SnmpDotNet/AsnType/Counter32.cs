namespace SnmpDotNet.AsnType
{
    /// <summary>
    /// Used to specify a value which represents a count. The range is 0 to 4294967295. 
    /// </summary>
    public class Counter32 : TValue
    {
        public Counter32(byte[] bytes) : base(SnmpTag.Counter32, bytes)
        {
        }
        public Counter32(uint value) : this(new Encoder().WriteIntegerUnsigned(value, SnmpTag.Counter32).Encode())
        {
        }
        public uint Value => new Decoder(Bytes).ReadUInt32(Tag);
        public static uint MaxValue => uint.MaxValue;
        public static uint MinValue => uint.MinValue;


    }
}
