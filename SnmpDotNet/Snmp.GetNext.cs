using SnmpDotNet.Encoding;
using SnmpDotNet.Encoding.Types;
using SnmpDotNet.Protocol;

namespace SnmpDotNet;

public partial class Snmp
{
	public static async Task<Dictionary<string, TValue>> GetNextAsync(
		SnmpVersion version, string ip, ushort port, string community, TimeSpan timeout, ushort retries, string oid)
	{
		var requestId = (uint) (new Random().Next());
		var msg = new GetNext(version, community, requestId, oid);
		var response = await Snmp.Send(msg.Encode(), ip, port, timeout, retries);
		var result = Response.Decode(response);
		if (result.RequestId != requestId) throw new Exceptions.UnmatchRequestIdException();
		ThrowIfError(result.SnmpError, result.ErrorIndex);
		return result.Varbinds;
	}

	public static Task<Dictionary<string, TValue>> GetNextAsync(
		SnmpVersion version, string ip, ushort port, string community, ushort timeout, ushort retries, string oid)
	{
		return GetNextAsync(version, ip, port, community, TimeSpan.FromMilliseconds(timeout), retries, oid);
	}

	public static async Task<Dictionary<string, TValue>> GetSubtreeAsync(SnmpVersion version, string ip, ushort port, string community, TimeSpan timeout, ushort retries, ushort maxRepetitions, string oid)
	{
		var rootOid = $"{oid}.";
		var result = new Dictionary<string, TValue>();

	GetData:
		var requestId = (uint) (new Random().Next());
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
	public static Task<Dictionary<string, TValue>> GetSubtreeAsync(SnmpVersion version, string ip, ushort port, string community, ushort timeout, ushort retries, ushort maxRepetitions, string oid)
	{
		return GetSubtreeAsync(version, ip, port, community, TimeSpan.FromMilliseconds(timeout), retries, maxRepetitions, oid);
	}

	public static async Task GetSubtreeAsync(SnmpVersion version, string ip, ushort port, string community, TimeSpan timeout, ushort retries, ushort maxRepetitions, string oid, Func<string, TValue, Task> task)
	{
		var rootOid = $"{oid}.";

	GetData:
		var requestId = (uint) (new Random().Next());
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
	public static Task GetSubtreeAsync(SnmpVersion version, string ip, ushort port, string community, ushort timeout, ushort retries, ushort maxRepetitions, string oid, Func<string, TValue, Task> task)
	{
		return GetSubtreeAsync(version, ip, port, community, TimeSpan.FromMilliseconds(timeout), retries, maxRepetitions, oid, task);
	}
}