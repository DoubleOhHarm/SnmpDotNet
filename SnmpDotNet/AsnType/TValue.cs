using SnmpDotNet.Exceptions;

namespace SnmpDotNet.AsnType
{
    public class TValue
    {
        public TValue(SnmpTag tag, byte[] bytes)
        {
            Tag = tag;
            Bytes = bytes;
        }

        public SnmpTag Tag { get; private set; }
        public byte[] Bytes { get; private set; }

        //public static TValue Decode(ReadOnlyMemory<byte> ibytes)
        //{
        //    var decoder = new Decoder(ibytes);
        //    var tag = decoder.PeekTag();
        //    var bytes = decoder.ReadEncodedValue().ToArray();
        //    return new TValue(tag, bytes);
        //}

        public Counter32 ToCounter32() => new Counter32(Bytes);
        public Counter64 ToCounter64() => new Counter64(Bytes);
        public Gauge32 ToGauge32() => new Gauge32(Bytes);
        public Integer32 ToInteger32() => new Integer32(Bytes);
        public IpAddress ToIpAddress() => new IpAddress(Bytes);
        public Null ToNull() => new Null(Bytes);
        public OctetString ToOctetString() => new OctetString(Bytes);
        public Oid ToOid() => new Oid(Bytes);
        public TimeTicks ToTimeTicks() => new TimeTicks(Bytes);
        public Unsigned32 ToUnsigned32() => new Unsigned32(Bytes);

        public object? GetValue()
        {
            if (Tag == SnmpTag.Counter32) return ToCounter32().Value;
            if (Tag == SnmpTag.Counter64) return ToCounter64().Value;
            if (Tag == SnmpTag.Gauge32) return ToGauge32().Value;
            if (Tag == SnmpTag.Integer32) return ToInteger32().Value;
            if (Tag == SnmpTag.IpAddress) return ToIpAddress().Value;
            if (Tag == SnmpTag.Null) return ToNull().Value;
            if (Tag == SnmpTag.OctetString) return ToOctetString().Value;
            if (Tag == SnmpTag.Oid) return ToOid().Value;
            if (Tag == SnmpTag.TimeTicks) return ToTimeTicks().Value;
            if (Tag == SnmpTag.Unsigned32) return ToUnsigned32().Value;
            if (Tag == SnmpTag.NoSuchInstance) throw new NoSuchInstanceException();


            return Bytes;
        }

        public override string? ToString()
        {
            return $"{this.GetValue()}";
        }
    }
}
