namespace SnmpDotNet.Message
{
    internal class GetBulk
    {
        public GetBulk(SnmpVersion version, string community, uint requestId, string oid, ushort maxRepetitions)
        {
            Version = version;
            Community = community;
            RequestId = requestId;
            Oid = oid;
            MaxRepetitions = maxRepetitions;
        }

        public SnmpVersion Version { get; }
        public string Community { get; }
        public uint RequestId { get; }
        public string Oid { get; }

        public ushort MaxRepetitions { get; }

        public byte[] Encode()
        {
            return new Encoder()
            .PushSequence()
            .WriteInteger((ushort)Version)
            .WriteOctetString(Community)

            //start Pdu
            .PushSequence(SnmpTag.GetBulkRequest)
            .WriteInteger(RequestId)
            .WriteInteger(0)
            .WriteInteger(MaxRepetitions)

            //start varbindlist
            .PushSequence()
            .PushSequence()
            .WriteOid(Oid)
            .WriteNull()
            .PopSequence()
            .PopSequence()
            //end varbindlist

            .PopSequence(SnmpTag.GetBulkRequest)
            //end Pdu

            .PopSequence()
            .Encode();
        }
    }
}
