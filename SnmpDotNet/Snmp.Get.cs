using System.Net;
using SnmpDotNet.Encoding.Types;
using SnmpDotNet.Protocol;

namespace SnmpDotNet;

public partial class Snmp
{
	public static async Task<Dictionary<string, TValue>> GetAsync(
		SnmpVersion version,
		IPEndPoint ipEndPoint,
		string community,
		TimeSpan timeout,
		ushort retries,
		HashSet<string> variables,
		CancellationToken cancellationToken)
	{
		uint requestId = (uint) Random.Shared.Next();
		Get msg = new Get(version, community, requestId, variables);

		byte[] response = await SendAsync(msg.Encode(), ipEndPoint, timeout, retries, cancellationToken).ConfigureAwait(false);

		Response result = Response.Decode(response);
		if (result.RequestId != requestId) throw new Exceptions.UnmatchRequestIdException();
		ThrowIfError(result.SnmpError, result.ErrorIndex);

		return result.Varbinds;
	}
}