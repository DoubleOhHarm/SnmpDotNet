namespace SnmpDotNet
{
    //http://www.tcpipguide.com/free/t_SNMPVersion1SNMPv1MessageFormat-2.htm

    public enum SnmpVersion
    {
        V1 = 0,
        V2c = 1,
    }
    //public enum PduType
    //{
    //    GetRequest = 0,
    //    GetNextRequest = 1,
    //    Response = 2,
    //    SetRequest = 3,
    //    TrapV1 = 4,
    //    GetBulkRequest = 5,
    //    InformRequest = 6,
    //    TrapV2 = 7,
    //    Report = 8
    //}

    public enum ErrorStatus
    {
        NoError = 0,
        TooBig = 1,
        NoSuchName = 2,
        BadValue = 3,
        ReadOnly = 4,
        GenErr = 5,
        NoAccess = 6,
        WrongType = 7,
        WrongLength = 8,
        WrongEncoding = 9,
        WrongValue = 10,
        NoCreation = 11,
        InconsistentValue = 12,
        ResourceUnavailable = 13,
        CommitFailed = 14,
        UndoFailed = 15,
        AuthrizationError = 16,
        NotWritable = 17,
        InconsistentName = 18,
    }
}
