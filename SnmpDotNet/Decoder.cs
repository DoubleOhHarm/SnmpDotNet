using SnmpDotNet.Exceptions;
using System.Formats.Asn1;
using System.Text;

namespace SnmpDotNet
{
    internal class Decoder
    {
        private readonly AsnReader _reader;

        public Decoder(AsnReader reader)
        {
            _reader = reader;
        }

        public Decoder(byte[] bytes) : this(new AsnReader(bytes, AsnEncodingRules.BER))
        {
        }
        public Decoder(ReadOnlyMemory<byte> bytes) : this(new AsnReader(bytes, AsnEncodingRules.BER))
        {
        }

        public bool HasData => _reader.HasData;

        public Decoder ReadSequence(SnmpTag? tag = null)
        {
            return new Decoder(_reader.ReadSequence(tag?.GetTag()));
        }
        public byte[] ReadIntegerBytes(SnmpTag? tag = null)
        {
            return _reader.ReadIntegerBytes(tag?.GetTag()).ToArray();
        }
        public int ReadInt32(SnmpTag? tag = null)
        {
            try
            {
                if (_reader.TryReadInt32(out int result, tag?.GetTag())) return result;
                
            }
            catch (AsnContentException e)
            {
                return BitConverter.ToInt32(_reader.PeekContentBytes().ToArray().Reverse().ToArray());
            }
            throw new DecodeFailedException();
        }
        public uint ReadUInt32(SnmpTag? tag = null)
        {
            if (_reader.TryReadUInt32(out uint result, tag?.GetTag())) return result;
            throw new DecodeFailedException();
        }
        public ulong ReadUInt64(SnmpTag? tag = null)
        {
            if (_reader.TryReadUInt64(out ulong result, tag?.GetTag())) return result;
            throw new DecodeFailedException();
        }
        public string ReadString(SnmpTag? tag = null)
        {
            return Encoding.UTF8.GetString(ReadOctetString(tag));
        }

        public byte[] ReadOctetString(SnmpTag? tag = null)
        {
            if (_reader.TryReadPrimitiveOctetString(out ReadOnlyMemory<byte> result, tag?.GetTag())) return result.ToArray();
            throw new DecodeFailedException();
        }

        public string ReadOid(SnmpTag? tag = null)
        {
            return _reader.ReadObjectIdentifier(tag?.GetTag());
        }
        public ReadOnlyMemory<byte> ReadEncodedValue()
        {
            return _reader.ReadEncodedValue();
        }

        public SnmpTag PeekTag()
        {
            return new SnmpTag(_reader.PeekTag());
        }
        public ReadOnlyMemory<byte> PeekContentBytes()
        {
            return _reader.PeekContentBytes();
        }
        public void ReadNull(SnmpTag? tag = null)
        {
            _reader.ReadNull(tag?.GetTag());
        }
    }
}
