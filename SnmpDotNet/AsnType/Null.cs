namespace SnmpDotNet.AsnType
{
    public class Null : TValue
    {

        public Null(byte[] bytes) : base(SnmpTag.Null, bytes)
        {
        }
        public Null() : this(new Encoder().WriteNull(SnmpTag.Null).Encode())
        {
        }
        public Null(Action action) : this()
        {
            action();
        }
        public object? Value => null;


    }
}
