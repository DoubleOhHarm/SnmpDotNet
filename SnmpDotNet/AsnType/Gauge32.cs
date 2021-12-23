namespace SnmpDotNet.AsnType
{
    /// <summary>
    /// It represents a non-negative integer which may increase or decrease, but which holds at the maximum or minimum value specified in the range when the actual value goes over or below the range, respectively.
    /// </summary>
    public class Gauge32 : TValue
    {
        public Gauge32(byte[] bytes) : base(SnmpTag.Gauge32, bytes)
        {
        }
        public Gauge32(uint value) : this(new Encoder().WriteIntegerUnsigned(value, SnmpTag.Gauge32).Encode())
        {
        }
        public uint Value => new Decoder(Bytes).ReadUInt32(Tag);
        public static uint MaxValue => uint.MaxValue;
        public static uint MinValue => uint.MinValue;

    }
}
