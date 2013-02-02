using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace GameServer {
	class ClientConnection {
		public delegate void ServerMessagedEventHandler(string message);
		public event ServerMessagedEventHandler ServerMessaged;

		public static int ClientNumber = 0;

		private Socket Sock;
		private SocketAsyncEventArgs SockAsyncEventArgs;
		private byte[] buff;

		public ClientConnection(Socket AcceptedSocket) {
			ClientNumber++;
			buff = new byte[1024];
			Sock = AcceptedSocket;
			SockAsyncEventArgs = new SocketAsyncEventArgs();
			SockAsyncEventArgs.Completed += SockAsyncEventArgs_Completed;
			SockAsyncEventArgs.SetBuffer(buff, 0, buff.Length);

			ReceiveAsync(SockAsyncEventArgs);
		}

		private void SockAsyncEventArgs_Completed(object sender, SocketAsyncEventArgs e) {
			switch (e.LastOperation) {
				case SocketAsyncOperation.Receive:
					ProcessReceive(e);
					break;
				case SocketAsyncOperation.Send:
					ProcessSend(e);
					break;
			}
		}

		private void ProcessSend(SocketAsyncEventArgs e) {
			if (e.SocketError == SocketError.Success)
				ReceiveAsync(SockAsyncEventArgs);
		}

		private void ProcessReceive(SocketAsyncEventArgs e) {
			if (e.SocketError == SocketError.Success) {
				string str = Encoding.UTF8.GetString(e.Buffer, 0, e.BytesTransferred);
				ServerMessaged("Incoming msg from #{0}: {1}" + ClientNumber.ToString() + str);
				SendAsync("You send " + str);
			}
		}

		private void ReceiveAsync(SocketAsyncEventArgs e) {
			bool willRaiseEvent = Sock.ReceiveAsync(e);
			if (!willRaiseEvent)
				ProcessReceive(e);
		}

		private void SendAsync(string data) {
			byte[] buff = Encoding.UTF8.GetBytes(data);
			SocketAsyncEventArgs e = new SocketAsyncEventArgs();
			e.Completed += SockAsyncEventArgs_Completed;
			e.SetBuffer(buff, 0, buff.Length);
			SendAsync(e);
		}
		private void SendAsync(SocketAsyncEventArgs e) {
			bool willRaiseEvent = Sock.SendAsync(e);
			if (!willRaiseEvent)
				ProcessSend(e);
		}
	}
}
