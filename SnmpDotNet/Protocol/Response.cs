using SnmpDotNet.Encoding;
using SnmpDotNet.Encoding.Types;
using SnmpDotNet.Exceptions;

namespace SnmpDotNet.Protocol
{
	public class Response
	{
		public Response(SnmpVersion version, string community, uint requestId, SnmpError snmpError, uint errorIndex, Dictionary<string, TValue> varbinds)
		{
			Version = version;
			Community = community;
			RequestId = requestId;
			SnmpError = snmpError;
			ErrorIndex = errorIndex;
			Varbinds = varbinds;
		}

		public SnmpVersion Version { get; }
		public string Community { get; }
		public uint RequestId { get; }
		public SnmpError SnmpError { get; }
		public uint ErrorIndex { get; }

		public Dictionary<string, TValue> Varbinds { get; }

		public static Response Decode(byte[] bytes)
		{
			try
			{
				var decoder = new Decoder(bytes);
				var msgDecoder = decoder.ReadSequence();
				var version = (SnmpVersion) msgDecoder.ReadInt32();
				var community = msgDecoder.ReadString();

				var pduDecoder = msgDecoder.ReadSequence(SnmpTag.Response);
				var requestId = pduDecoder.ReadUInt32();

				var error = (SnmpError) pduDecoder.ReadInt32();
				var errorIndex = pduDecoder.ReadUInt32();

				var vblDecoder = pduDecoder.ReadSequence();
				var varbinds = new Dictionary<string, TValue>();
				while (vblDecoder.HasData)
				{
					var vbDecoder = vblDecoder.ReadSequence();
					varbinds.Add(vbDecoder.ReadOid(), new TValue(vbDecoder.PeekTag(), vbDecoder.ReadEncodedValue().ToArray()));
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
