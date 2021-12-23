namespace SnmpDotNet.Message
{
    internal class GetNext
    {
        public GetNext(SnmpVersion version, string community, uint requestId, string oid)
        {
            Version = version;
            Community = community;
            RequestId = requestId;
            Oid = oid;
        }

        public SnmpVersion Version { get; }
        public string Community { get; }
        public uint RequestId { get; }
        public string Oid { get; }

        public byte[] Encode()
        {
            return new Encoder()
            .PushSequence()
            .WriteInteger((ushort)Version)
            .WriteOctetString(Community)

            //start Pdu
            .PushSequence(SnmpTag.GetNextRequest)
            .WriteInteger(RequestId)
            .WriteInteger((ushort)ErrorStatus.NoError)
            .WriteInteger(0)

            //start varbindlist
            .PushSequence()
            .PushSequence()
            .WriteOid(Oid)
            .WriteNull()
            .PopSequence()
            .PopSequence()
            //end varbindlist

            .PopSequence(SnmpTag.GetNextRequest)
            //end Pdu

            .PopSequence()
             .Encode();

        }
    }
}
