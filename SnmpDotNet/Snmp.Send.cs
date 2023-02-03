using System.Net;
using System.Net.Sockets;
using SnmpDotNet.Protocol;

namespace SnmpDotNet
{
	public static partial class Snmp
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="bytes"></param>
		/// <param name="ipEndPoint"></param>
		/// <param name="timeout"></param>
		/// <param name="retries"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		/// <exception cref="Exceptions.SnmpTimeoutException"></exception>
		private static async Task<byte[]> SendAsync(byte[] bytes, IPEndPoint ipEndPoint, TimeSpan timeout, ushort retries, CancellationToken cancellationToken)
		{
			if (bytes == null) throw new ArgumentNullException(nameof(bytes));
			if (ipEndPoint == null) throw new ArgumentNullException(nameof(ipEndPoint));

			using CancellationTokenSource linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
			using UdpClient udpClient = new UdpClient();

			for (int i = 0; i <= retries; i++)
			{
				try
				{
					linkedTokenSource.CancelAfter(timeout);
					await udpClient
						.SendAsync(bytes, ipEndPoint, linkedTokenSource.Token)
						.ConfigureAwait(false);

					linkedTokenSource.CancelAfter(timeout);
					UdpReceiveResult result = await udpClient
						.ReceiveAsync(linkedTokenSource.Token)
						.ConfigureAwait(false);

					return result.Buffer;
				}
				catch (OperationCanceledException)
				{
					if (i == retries) throw new Exceptions.SnmpTimeoutException();
				}
			}

			return Array.Empty<byte>();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="bytes"></param>
		/// <param name="ipEndPoint"></param>
		/// <param name="timeout"></param>
		/// <param name="retries"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		/// <exception cref="Exceptions.SnmpTimeoutException"></exception>
		private static byte[] Send(byte[] bytes, IPEndPoint ipEndPoint, TimeSpan timeout, ushort retries, CancellationToken cancellationToken)
		{
			if (bytes == null) throw new ArgumentNullException(nameof(bytes));
			if (ipEndPoint == null) throw new ArgumentNullException(nameof(ipEndPoint));

			using CancellationTokenSource linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
			using UdpClient udpClient = new UdpClient();

			//dirty-hack to cancel non-async udpClient methods
			// ReSharper disable once AccessToDisposedClosure
			using CancellationTokenRegistration cancellationTokenRegistration = cancellationToken.Register(udpClient.Close);

			for (int i = 0; i <= retries; i++)
			{
				try
				{
					linkedTokenSource.CancelAfter(timeout);
					udpClient.Send(bytes, ipEndPoint);

					linkedTokenSource.CancelAfter(timeout);
					IPEndPoint? remoteEp = null;
					byte[] result = udpClient.Receive(ref remoteEp);

					return result;
				}
				catch (OperationCanceledException)
				{
					if (i == retries) throw new Exceptions.SnmpTimeoutException();
				}
			}

			return Array.Empty<byte>();
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
