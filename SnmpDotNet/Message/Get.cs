namespace SnmpDotNet.Message
{
    internal class Get
    {
        public Get(SnmpVersion version, string community, uint requestId, HashSet<string> oids)
        {
            Version = version;
            Community = community;
            RequestId = requestId;
            Oids = oids;
        }

        public SnmpVersion Version { get; }
        public string Community { get; }
        public uint RequestId { get; }
        public HashSet<string> Oids { get; }

        public byte[] Encode()
        {
            var encoder = new Encoder();
            encoder.PushSequence();
            encoder.WriteInteger((ushort)Version);
            encoder.WriteOctetString(Community);

            //start Pdu
            encoder.PushSequence(SnmpTag.GetRequest);
            encoder.WriteInteger(RequestId);
            encoder.WriteInteger((ushort)ErrorStatus.NoError);
            encoder.WriteInteger(0);

            //start varbindlist
            encoder.PushSequence();

            encoder.WriteEncodedValue(Oids.SelectMany(oid => new Encoder().PushSequence().WriteOid(oid).WriteNull().PopSequence().Encode()).ToArray());

            encoder.PopSequence();
            //end varbindlist

            encoder.PopSequence(SnmpTag.GetRequest);
            //end Pdu

            encoder.PopSequence();
            return encoder.Encode();
        }
    }
}
