using System.Net;
using SnmpDotNet.Encoding;
using SnmpDotNet.Encoding.Types;
using SnmpDotNet.Protocol;

namespace SnmpDotNet;

public partial class Snmp
{
	public static async Task<Dictionary<string, TValue>> WalkAsync(
		SnmpVersion version,
		IPEndPoint ipEndPoint,
		string community,
		TimeSpan timeout,
		ushort retries,
		ushort maxRepetitions,
		string oid,
		CancellationToken cancellationToken)
	{
		string rootOid = $"{oid}.";
		Dictionary<string, TValue> result = new Dictionary<string, TValue>();

	GetData:
		Dictionary<string, TValue> data = version == SnmpVersion.V1
			? await GetNextAsync(version, ipEndPoint, community, timeout, retries, oid, cancellationToken)
				.ConfigureAwait(false)
			: await GetBulkAsync(version, ipEndPoint, community, timeout, retries, maxRepetitions, oid, cancellationToken)
				.ConfigureAwait(false);

		foreach (KeyValuePair<string, TValue> vb in data)
		{
			if (!vb.Key.StartsWith(rootOid, StringComparison.InvariantCulture)
				|| vb.Value.Tag == SnmpTag.NoSuchInstance
				|| vb.Value.Tag == SnmpTag.NoSuchObject
				|| vb.Value.Tag == SnmpTag.EndOfMibView)
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
}