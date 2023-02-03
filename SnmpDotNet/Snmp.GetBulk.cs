using System.Net;
using SnmpDotNet.Encoding.Types;
using SnmpDotNet.Protocol;

namespace SnmpDotNet;

public static partial class Snmp
{
	public static async Task<Dictionary<string, TValue>> GetBulkAsync(
		SnmpVersion version,
		IPEndPoint ipEndPoint,
		string community,
		TimeSpan timeout,
		ushort retries,
		ushort maxRepetitions,
		string oid,
		CancellationToken cancellationToken)
	{
		uint requestId = (uint) Random.Shared.Next();
		GetBulk msg = new GetBulk(version, community, requestId, oid, maxRepetitions);

		byte[] response = await SendAsync(msg.Encode(), ipEndPoint, timeout, retries, cancellationToken).ConfigureAwait(false);

		Response result = Response.Decode(response);
		if (result.RequestId != requestId) throw new Exceptions.UnmatchRequestIdException();
		ThrowIfError(result.SnmpError, result.ErrorIndex);

		return result.Varbinds;
	}

	public static Dictionary<string, TValue> GetBulk(
		SnmpVersion version,
		IPEndPoint ipEndPoint,
		string community,
		TimeSpan timeout,
		ushort retries,
		ushort maxRepetitions,
		string oid,
		CancellationToken cancellationToken)
	{
		uint requestId = (uint) Random.Shared.Next();
		GetBulk msg = new GetBulk(version, community, requestId, oid, maxRepetitions);

		byte[] response = Send(msg.Encode(), ipEndPoint, timeout, retries, cancellationToken);

		Response result = Response.Decode(response);
		if (result.RequestId != requestId) throw new Exceptions.UnmatchRequestIdException();
		ThrowIfError(result.SnmpError, result.ErrorIndex);

		return result.Varbinds;
	}
}