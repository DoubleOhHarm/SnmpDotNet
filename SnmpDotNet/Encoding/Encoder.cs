using System.Formats.Asn1;
using System.Numerics;

namespace SnmpDotNet.Encoding
{
	internal class Encoder
	{
		private readonly AsnWriter _writer;
		public Encoder()
		{
			_writer = new AsnWriter(AsnEncodingRules.BER);
		}

		//public Encoder WriteTValue(TValue value)
		//{
		//    return WriteContentBytes(value.Tag, value.ValueBytes); ;
		//}
		//public Encoder WriteContentBytes(SnmpTag tag, byte[] value)
		//{
		//    _writer.tag
		//    if (new[] { SnmpTag.OctetString, SnmpTag.IpAddress }.Contains(tag))
		//        return WriteOctetString(value, tag);
		//    if (new[] { SnmpTag.Gauge32, SnmpTag.Counter32, SnmpTag.TimeTicks, SnmpTag.Unsigned32, SnmpTag.Counter64 }.Contains(tag))
		//        return WriteIntegerUnsigned(value, tag);
		//    if (tag == SnmpTag.Integer32)
		//        return WriteInteger(value, tag);
		//    if (tag == SnmpTag.Oid)
		//        return WriteEncodedValue(BerConverter.Encode("to", tag.TagClass + tag.TagValue, value));
		//    return this;
		//}
		public Encoder WriteEncodedValue(byte[] value)
		{
			_writer.WriteEncodedValue(value);
			return this;
		}
		public Encoder PushSequence(SnmpTag? tag = null)
		{
			_writer.PushSequence(tag?.GetTag());
			return this;
		}
		public Encoder PopSequence(SnmpTag? tag = null)
		{
			_writer.PopSequence(tag?.GetTag());
			return this;
		}
		public Encoder WriteIntegerBytes(byte[] value, SnmpTag? tag = null)
		{
			_writer.WriteInteger(value, tag?.GetTag());
			return this;
		}
		public Encoder WriteInteger(long value, SnmpTag? tag = null)
		{
			_writer.WriteInteger(new BigInteger(value).ToByteArray().Reverse().ToArray(), tag?.GetTag());
			return this;
		}

		public Encoder WriteIntegerUnsigned(ulong value, SnmpTag? tag = null)
		{
			_writer.WriteIntegerUnsigned(new BigInteger(value).ToByteArray().Reverse().ToArray(), tag?.GetTag());
			return this;
		}

		public Encoder WriteOctetString(string value, SnmpTag? tag = null)
		{

			return WriteOctetString(System.Text.Encoding.UTF8.GetBytes(value));
		}
		public Encoder WriteOctetString(byte[] value, SnmpTag? tag = null)
		{
			_writer.WriteOctetString(value, tag?.GetTag());
			return this;
		}
		public Encoder WriteNull(SnmpTag? tag = null)
		{
			_writer.WriteNull(tag?.GetTag());
			return this;
		}
		public Encoder WriteOid(string value, SnmpTag? tag = null)
		{
			_writer.WriteObjectIdentifier(value, tag?.GetTag());
			return this;
		}
		public byte[] Encode() => _writer.Encode();
	}
}
