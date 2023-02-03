using System.Net;
using System.Net.Sockets;
using SnmpDotNet.Protocol;

namespace SnmpDotNet
{
	public partial class Snmp
	{
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

		private static void ThrowIfError(SnmpError status, uint errorIndex)
		{
			switch (status)
			{
				case SnmpError.AuthrizationError:
					throw new Exceptions.AuthrizationErrorException();
				case SnmpError.BadValue:
					throw new Exceptions.BadValueException();
				case SnmpError.CommitFailed:
					throw new Exceptions.CommitFailedException();
				case SnmpError.GenErr:
					throw new Exceptions.GenErrException();
				case SnmpError.InconsistentName:
					throw new Exceptions.InconsistentNameException();
				case SnmpError.InconsistentValue:
					throw new Exceptions.InconsistentValueException();
				case SnmpError.NoAccess:
					throw new Exceptions.NoAccessException();
				case SnmpError.NoCreation:
					throw new Exceptions.NoCreationException();
				case SnmpError.NoSuchName:
					throw new Exceptions.NoSuchNameException();
				case SnmpError.NotWritable:
					throw new Exceptions.NotWritableException();
				case SnmpError.ReadOnly:
					throw new Exceptions.ReadOnlyException();
				case SnmpError.ResourceUnavailable:
					throw new Exceptions.ResourceUnavailableException();
				case SnmpError.TooBig:
					throw new Exceptions.TooBigException();
				case SnmpError.UndoFailed:
					throw new Exceptions.UndoFailedException();
				case SnmpError.WrongEncoding:
					throw new Exceptions.WrongEncodingException();
				case SnmpError.WrongLength:
					throw new Exceptions.WrongLengthException();
				case SnmpError.WrongType:
					throw new Exceptions.WrongTypeException();
				case SnmpError.WrongValue:
					throw new Exceptions.WrongValueException();

			}
		}
	}
}
