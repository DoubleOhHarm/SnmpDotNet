using SnmpDotNet.Encoding.Types;
using SnmpDotNet.Protocol;

namespace SnmpDotNet;

public partial class Snmp
{
	public static async Task<Dictionary<string, TValue>> GetAsync(
		SnmpVersion version, string ip, ushort port, string community, TimeSpan timeout, ushort retries,
		HashSet<string> oids)
	{
		var requestId = (uint) new Random().Next();
		var msg = new Get(version, community, requestId, oids);
		var response = await Send(msg.Encode(), ip, port, timeout, retries);
		var result = Response.Decode(response);
		if (result.RequestId != requestId) throw new Exceptions.UnmatchRequestIdException();
		ThrowIfError(result.SnmpError, result.ErrorIndex);
		return result.Varbinds;
	}

	public static Task<Dictionary<string, TValue>> GetAsync(
		SnmpVersion version, string ip, ushort port, string community, TimeSpan timeout, ushort retries,
		params string[] oids)
	{
		return GetAsync(version, ip, port, community, timeout, retries, oids.ToHashSet());
	}

	public static Task<Dictionary<string, TValue>> GetAsync(
		SnmpVersion version, string ip, ushort port, string community, ushort timeout, ushort retries,
		HashSet<string> oids)
	{
		return GetAsync(version, ip, port, community, TimeSpan.FromMilliseconds(timeout), retries, oids);
	}

	public static Task<Dictionary<string, TValue>> GetAsync(
		SnmpVersion version, string ip, ushort port, string community, ushort timeout, ushort retries,
		params string[] oids)
	{
		return GetAsync(version, ip, port, community, TimeSpan.FromMilliseconds(timeout), retries, oids.ToHashSet());
	}

	public static async Task<KeyValuePair<string, TValue>> GetOneAsync(
		SnmpVersion version, string ip, ushort port, string community, TimeSpan timeout, ushort retries, string oid)
	{
		return (await GetAsync(version, ip, port, community, timeout, retries, oid)).First();
	}

	public static Task<KeyValuePair<string, TValue>> GetOneAsync(
		SnmpVersion version, string ip, ushort port, string community, ushort timeout, ushort retries, string oid)
	{
		return GetOneAsync(version, ip, port, community, TimeSpan.FromMilliseconds(timeout), retries, oid);
	}
}