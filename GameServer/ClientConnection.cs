using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using GameItems;
using System.Runtime.Serialization.Json;
using System.IO;

namespace GameServer {
	class ClientConnection {
		public delegate void ServerMessagedEventHandler(string message);
		public event ServerMessagedEventHandler ServerMessaged;

		public static int ClientCount = 0;

		private Socket Sock;
		private SocketAsyncEventArgs SockAsyncEventArgs;
		private byte[] buff;

		public ClientConnection(Socket AcceptedSocket) {
			ClientCount++;
			buff = new byte[1024];
			Sock = AcceptedSocket;
			SockAsyncEventArgs = new SocketAsyncEventArgs();
			SockAsyncEventArgs.Completed += SockAsyncEventArgs_Completed;
			SockAsyncEventArgs.SetBuffer(buff, 0, buff.Length);
			SockReceiveAsync(SockAsyncEventArgs);
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
			if (e.SocketError == SocketError.Success) {
				SockReceiveAsync(SockAsyncEventArgs);
			}
		}

		private void ProcessReceive(SocketAsyncEventArgs e) {
			if (e.SocketError == SocketError.Success) {
				SockReceiveAsync(e);
			} else {
				Console.WriteLine("Dont recieve");
			}
		}

		private void SockReceiveAsync(SocketAsyncEventArgs e) {
			if (Sock.ReceiveAsync(e) == false) {
				if (e.SocketError == SocketError.Success) {
					SockReceiveAsync(e);
				}
			}
		}

		public void SendAsync(string data) {
			byte[] buff = Encoding.UTF8.GetBytes(data);
			SocketAsyncEventArgs e = new SocketAsyncEventArgs();
			e.Completed += SockAsyncEventArgs_Completed;
			e.SetBuffer(buff, 0, buff.Length);
			SendAsync(e);
		}

		public void SendAsync(Player gamePlayer) {
			MemoryStream memStream = new MemoryStream();
			DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(GameItems.Player));
			ser.WriteObject(memStream, gamePlayer);
			byte[] buff = new byte[1024];
			memStream.Read(buff, 0, (int)memStream.Length - 1);
			SocketAsyncEventArgs e = new SocketAsyncEventArgs();
			e.Completed += new EventHandler<SocketAsyncEventArgs>(SockAsyncEventArgs_Completed);
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
