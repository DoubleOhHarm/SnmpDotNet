using SnmpDotNet.AsnType;
using System.Net;
using System.Net.Sockets;

namespace SnmpDotNet
{
    public class Snmp
    {
        public static async Task<Dictionary<string, AsnType.TValue>> GetAsync(SnmpVersion version, string ip, ushort port, string community, TimeSpan timeout, ushort retries, HashSet<string> oids)
        {
            var requestId = (uint)new Random().Next();
            var msg = new Message.Get(version, community, requestId, oids);
            var response = await Snmp.Send(msg.Encode(), ip, port, timeout, retries);
            var result = Message.Response.Decode(response);
            if (result.RequestId != requestId) throw new Exceptions.UnmatchRequestIdException();
            ThrowIfError(result.Error, result.ErrorIndex);
            return result.Varbinds;
        }
        public static Task<Dictionary<string, AsnType.TValue>> GetAsync(SnmpVersion version, string ip, ushort port, string community, TimeSpan timeout, ushort retries, params string[] oids)
        {
            return GetAsync(version, ip, port, community, timeout, retries, oids.ToHashSet());
        }

        public static Task<Dictionary<string, AsnType.TValue>> GetAsync(SnmpVersion version, string ip, ushort port, string community, ushort timeout, ushort retries, HashSet<string> oids)
        {
            return GetAsync(version, ip, port, community, TimeSpan.FromMilliseconds(timeout), retries, oids);
        }

        public static Task<Dictionary<string, AsnType.TValue>> GetAsync(SnmpVersion version, string ip, ushort port, string community, ushort timeout, ushort retries, params string[] oids)
        {
            return GetAsync(version, ip, port, community, TimeSpan.FromMilliseconds(timeout), retries, oids.ToHashSet());
        }

        public static async Task<KeyValuePair<string, TValue>> GetOneAsync(SnmpVersion version, string ip, ushort port, string community, TimeSpan timeout, ushort retries, string oid)
        {
            return (await GetAsync(version, ip, port, community, timeout, retries, oid)).First();
        }

        public static Task<KeyValuePair<string, TValue>> GetOneAsync(SnmpVersion version, string ip, ushort port, string community, ushort timeout, ushort retries, string oid)
        {
            return GetOneAsync(version, ip, port, community, TimeSpan.FromMilliseconds(timeout), retries, oid);
        }

        public static async Task<Dictionary<string, AsnType.TValue>> GetNextAsync(SnmpVersion version, string ip, ushort port, string community, TimeSpan timeout, ushort retries, string oid)
        {
            var requestId = (uint)(new Random().Next());
            var msg = new Message.GetNext(version, community, requestId, oid);
            var response = await Snmp.Send(msg.Encode(), ip, port, timeout, retries);
            var result = Message.Response.Decode(response);
            if (result.RequestId != requestId) throw new Exceptions.UnmatchRequestIdException();
            ThrowIfError(result.Error, result.ErrorIndex);
            return result.Varbinds;
        }
        public static Task<Dictionary<string, AsnType.TValue>> GetNextAsync(SnmpVersion version, string ip, ushort port, string community, ushort timeout, ushort retries, string oid)
        {
            return GetNextAsync(version, ip, port, community, TimeSpan.FromMilliseconds(timeout), retries, oid);
        }

        public static async Task<Dictionary<string, AsnType.TValue>> GetBulkAsync(SnmpVersion version, string ip, ushort port, string community, TimeSpan timeout, ushort retries, ushort maxRepetitions, string oid)
        {
            var requestId = (uint)(new Random().Next());
            var msg = new Message.GetBulk(version, community, requestId, oid, maxRepetitions);
            var response = await Snmp.Send(msg.Encode(), ip, port, timeout, retries);
            var result = Message.Response.Decode(response);
            if (result.RequestId != requestId) throw new Exceptions.UnmatchRequestIdException();
            ThrowIfError(result.Error, result.ErrorIndex);
            return result.Varbinds;
        }
        public static Task<Dictionary<string, AsnType.TValue>> GetBulkAsync(SnmpVersion version, string ip, ushort port, string community, ushort timeout, ushort retries, ushort maxRepetitions, string oid)
        {
            return GetBulkAsync(version, ip, port, community, TimeSpan.FromMilliseconds(timeout), retries, maxRepetitions, oid);
        }

        public static async Task<Dictionary<string, AsnType.TValue>> GetSubtreeAsync(SnmpVersion version, string ip, ushort port, string community, TimeSpan timeout, ushort retries, ushort maxRepetitions, string oid)
        {
            var rootOid = $"{oid}.";
            var result = new Dictionary<string, AsnType.TValue>();

        GetData:
            var requestId = (uint)(new Random().Next());
            var data = version == SnmpVersion.V1 ? await Snmp.GetNextAsync(version, ip, port, community, timeout, retries, oid) : await Snmp.GetBulkAsync(version, ip, port, community, timeout, retries, maxRepetitions, oid);

            foreach (var vb in data)
            {
                if (!vb.Key.StartsWith(rootOid) || vb.Value.Tag == SnmpTag.NoSuchInstance || vb.Value.Tag == SnmpTag.NoSuchObject || vb.Value.Tag == SnmpTag.EndOfMibView)
                {
                    goto Done;
                }
                result.TryAdd(vb.Key, vb.Value);
                oid = vb.Key;
            }
            goto GetData;

        Done:
            return result;
        }
        public static Task<Dictionary<string, AsnType.TValue>> GetSubtreeAsync(SnmpVersion version, string ip, ushort port, string community, ushort timeout, ushort retries, ushort maxRepetitions, string oid)
        {
            return GetSubtreeAsync(version, ip, port, community, TimeSpan.FromMilliseconds(timeout), retries, maxRepetitions, oid);
        }

        public static async Task GetSubtreeAsync(SnmpVersion version, string ip, ushort port, string community, TimeSpan timeout, ushort retries, ushort maxRepetitions, string oid, Func<string, AsnType.TValue, Task> task)
        {
            var rootOid = $"{oid}.";

        GetData:
            var requestId = (uint)(new Random().Next());
            var data = version == SnmpVersion.V1 ? await Snmp.GetNextAsync(version, ip, port, community, timeout, retries, oid) : await Snmp.GetBulkAsync(version, ip, port, community, timeout, retries, maxRepetitions, oid);

            foreach (var vb in data)
            {
                if (!vb.Key.StartsWith(rootOid) || vb.Value.Tag == SnmpTag.NoSuchInstance || vb.Value.Tag == SnmpTag.NoSuchObject || vb.Value.Tag == SnmpTag.EndOfMibView)
                {
                    return;
                }
                _ = task(vb.Key, vb.Value);
                oid = vb.Key;
            }
            goto GetData;
        }
        public static Task GetSubtreeAsync(SnmpVersion version, string ip, ushort port, string community, ushort timeout, ushort retries, ushort maxRepetitions, string oid, Func<string, AsnType.TValue, Task> task)
        {
            return GetSubtreeAsync(version, ip, port, community, TimeSpan.FromMilliseconds(timeout), retries, maxRepetitions, oid, task);
        }

        private static async Task<byte[]> Send(byte[] bytes, string ip, ushort port, TimeSpan timeout, ushort retries)
        {
            UdpClient? udp = null;
            try
            {
                udp = new();
                for (int i = 0; i <= retries; i++)
                {
                    try
                    {
                        await udp.SendAsync(bytes, new IPEndPoint(IPAddress.Parse(ip), port), new CancellationTokenSource(timeout).Token);
                        UdpReceiveResult result = await udp.ReceiveAsync(new CancellationTokenSource(timeout).Token);
                        return result.Buffer;
                    }
                    catch (OperationCanceledException)
                    {
                        if (i == retries) throw new Exceptions.SnmpTimeoutException();
                        else continue;
                    }
                }
            }
            finally
            {
                if (udp != null)
                {
                    udp.Close();
                    udp.Dispose();
                }
            }
            return new byte[] { };
        }

        private static void ThrowIfError(ErrorStatus status, uint errorIndex)
        {
            switch (status)
            {
                case ErrorStatus.AuthrizationError:
                    throw new Exceptions.AuthrizationErrorException();
                case ErrorStatus.BadValue:
                    throw new Exceptions.BadValueException();
                case ErrorStatus.CommitFailed:
                    throw new Exceptions.CommitFailedException();
                case ErrorStatus.GenErr:
                    throw new Exceptions.GenErrException();
                case ErrorStatus.InconsistentName:
                    throw new Exceptions.InconsistentNameException();
                case ErrorStatus.InconsistentValue:
                    throw new Exceptions.InconsistentValueException();
                case ErrorStatus.NoAccess:
                    throw new Exceptions.NoAccessException();
                case ErrorStatus.NoCreation:
                    throw new Exceptions.NoCreationException();
                case ErrorStatus.NoSuchName:
                    throw new Exceptions.NoSuchNameException();
                case ErrorStatus.NotWritable:
                    throw new Exceptions.NotWritableException();
                case ErrorStatus.ReadOnly:
                    throw new Exceptions.ReadOnlyException();
                case ErrorStatus.ResourceUnavailable:
                    throw new Exceptions.ResourceUnavailableException();
                case ErrorStatus.TooBig:
                    throw new Exceptions.TooBigException();
                case ErrorStatus.UndoFailed:
                    throw new Exceptions.UndoFailedException();
                case ErrorStatus.WrongEncoding:
                    throw new Exceptions.WrongEncodingException();
                case ErrorStatus.WrongLength:
                    throw new Exceptions.WrongLengthException();
                case ErrorStatus.WrongType:
                    throw new Exceptions.WrongTypeException();
                case ErrorStatus.WrongValue:
                    throw new Exceptions.WrongValueException();

            }
        }

    }
}