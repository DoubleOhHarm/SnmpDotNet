namespace SnmpDotNet.AsnType
{
    /// <summary>
    /// Used to specify the elapsed time between two events, in units of hundredth of a second. Range is 0 to 2e32 - 1. 
    /// </summary>
    public class TimeTicks : TValue
    {
        public TimeTicks(byte[] bytes) : base(SnmpTag.TimeTicks, bytes)
        {
        }
        public TimeTicks(uint value) : this(new Encoder().WriteIntegerUnsigned(value, SnmpTag.TimeTicks).Encode())
        {
        }
        public uint Value => new Decoder(Bytes).ReadUInt32(Tag);
        public static uint MaxValue => uint.MaxValue;
        public static uint MinValue => uint.MinValue;


    }
}
