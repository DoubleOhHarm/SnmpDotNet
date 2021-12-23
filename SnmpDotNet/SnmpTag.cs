using System.Diagnostics.CodeAnalysis;
using System.Formats.Asn1;
using System.Text.Json.Serialization;

namespace SnmpDotNet
{
    //http://intronetworks.cs.luc.edu/current/html/netmgmt1.html#snmp-and-asn-1-encoding
    //https://cdpstudio.com/manual/cdp/snmpio/about-snmp.html
    public struct SnmpTag : IEquatable<SnmpTag>
    {
        [JsonConstructor]
        public SnmpTag(TagClass tagClass, int tagValue)
        {
            TagClass = tagClass;
            TagValue = tagValue;
        }
        public SnmpTag(Asn1Tag tag)
        {
            TagClass = tag.TagClass;
            TagValue = tag.TagValue;
        }

        public TagClass TagClass { get; set; }
        public int TagValue { get; set; }

        public Asn1Tag GetTag() => new Asn1Tag(TagClass, TagValue);

        public static bool operator ==(SnmpTag left, SnmpTag right)
        {
            return left.TagClass == right.TagClass && left.TagValue == right.TagValue;
        }
        public static bool operator !=(SnmpTag left, SnmpTag right)
        {
            return left.TagClass != right.TagClass || left.TagValue != right.TagValue;
        }
        public static bool operator ==(Asn1Tag left, SnmpTag right)
        {
            return left.TagClass == right.TagClass && left.TagValue == right.TagValue;
        }
        public static bool operator !=(Asn1Tag left, SnmpTag right)
        {
            return left.TagClass != right.TagClass || left.TagValue != right.TagValue;
        }

        public bool Equals(SnmpTag other)
        {
            return other.TagValue == TagValue && other.TagClass == TagClass;
        }
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is SnmpTag && Equals((SnmpTag)obj);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(TagClass, TagValue);
        }

        //universal 
        public static readonly SnmpTag Null = new SnmpTag(Asn1Tag.Null);
        public static readonly SnmpTag Integer32 = new SnmpTag(Asn1Tag.Integer);
        public static readonly SnmpTag OctetString = new SnmpTag(Asn1Tag.PrimitiveOctetString);
        public static readonly SnmpTag Oid = new SnmpTag(Asn1Tag.ObjectIdentifier);
        public static readonly SnmpTag Sequence = new SnmpTag(Asn1Tag.Sequence);

        //application-specific
        public static readonly SnmpTag IpAddress = new SnmpTag(TagClass.Application, 0);
        public static readonly SnmpTag Counter32 = new SnmpTag(TagClass.Application, 1);
        public static readonly SnmpTag Gauge32 = new SnmpTag(TagClass.Application, 2);
        public static readonly SnmpTag TimeTicks = new SnmpTag(TagClass.Application, 3);
        public static readonly SnmpTag Opaque = new SnmpTag(TagClass.Application, 4);
        public static readonly SnmpTag Counter64 = new SnmpTag(TagClass.Application, 6);
        public static readonly SnmpTag Unsigned32 = new SnmpTag(TagClass.Application, 7);


        //context-specific 
        public static readonly SnmpTag NoSuchObject = new SnmpTag(TagClass.ContextSpecific, 0);
        public static readonly SnmpTag NoSuchInstance = new SnmpTag(TagClass.ContextSpecific, 1);
        public static readonly SnmpTag EndOfMibView = new SnmpTag(TagClass.ContextSpecific, 2);



        //pdu tag
        public static readonly SnmpTag GetRequest = new SnmpTag(TagClass.ContextSpecific, 0);
        public static readonly SnmpTag GetNextRequest = new SnmpTag(TagClass.ContextSpecific, 1);
        public static readonly SnmpTag GetBulkRequest = new SnmpTag(TagClass.ContextSpecific, 5);
        public static readonly SnmpTag Response = new SnmpTag(TagClass.ContextSpecific, 2);


    }
}
