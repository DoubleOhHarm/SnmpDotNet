using SnmpDotNet.Encoding.Types;
using SnmpDotNet.Protocol;

namespace SnmpDotNet;

public partial class Snmp
{
	public static async Task<Dictionary<string, TValue>> GetBulkAsync(
		SnmpVersion version, string ip, ushort port, string community, TimeSpan timeout, ushort retries,
		ushort maxRepetitions, string oid)
	{
		var requestId = (uint) (new Random().Next());
		var msg = new GetBulk(version, community, requestId, oid, maxRepetitions);
		var response = await Snmp.Send(msg.Encode(), ip, port, timeout, retries);
		var result = Response.Decode(response);
		if (result.RequestId != requestId) throw new Exceptions.UnmatchRequestIdException();
		ThrowIfError(result.SnmpError, result.ErrorIndex);
		return result.Varbinds;
	}

	public static Task<Dictionary<string, TValue>> GetBulkAsync(
		SnmpVersion version, string ip, ushort port, string community, ushort timeout, ushort retries,
		ushort maxRepetitions, string oid)
	{
		return GetBulkAsync(version, ip, port, community, TimeSpan.FromMilliseconds(timeout), retries, maxRepetitions,
			oid);
	}
}