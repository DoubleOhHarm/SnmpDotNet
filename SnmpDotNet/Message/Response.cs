using SnmpDotNet.Exceptions;

namespace SnmpDotNet.Message
{
    public class Response
    {
        public Response(SnmpVersion version, string community, uint requestId, ErrorStatus error, uint errorIndex, Dictionary<string, AsnType.TValue> varbinds)
        {
            Version = version;
            Community = community;
            RequestId = requestId;
            Error = error;
            ErrorIndex = errorIndex;
            Varbinds = varbinds;
        }

        public SnmpVersion Version { get; }
        public string Community { get; }
        public uint RequestId { get; }
        public ErrorStatus Error { get; }
        public uint ErrorIndex { get; }

        public Dictionary<string, AsnType.TValue> Varbinds { get; }

        public static Response Decode(byte[] bytes)
        {
            try
            {
                var decoder = new Decoder(bytes);
                var msgDecoder = decoder.ReadSequence();
                var version = (SnmpVersion)msgDecoder.ReadInt32();
                var community = msgDecoder.ReadString();

                var pduDecoder = msgDecoder.ReadSequence(SnmpTag.Response);
                var requestId = pduDecoder.ReadUInt32();

                var error = (ErrorStatus)pduDecoder.ReadInt32();
                var errorIndex = pduDecoder.ReadUInt32();

                var vblDecoder = pduDecoder.ReadSequence();
                var varbinds = new Dictionary<string, AsnType.TValue>();
                while (vblDecoder.HasData)
                {
                    var vbDecoder = vblDecoder.ReadSequence();
                    varbinds.Add(vbDecoder.ReadOid(), new AsnType.TValue(vbDecoder.PeekTag(), vbDecoder.ReadEncodedValue().ToArray()));
                }
                return new Response(version, community, requestId, error, errorIndex, varbinds);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new DecodeFailedException();
            }
        }
    }
}
