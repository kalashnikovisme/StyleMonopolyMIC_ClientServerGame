using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace ClientNameSpace {
	public class Client {
		public delegate void ClientMessagedEventHandler(string message);
		public event ClientMessagedEventHandler ClientMessaged;

		private Socket Sock;
		private SocketAsyncEventArgs SockAsyncArgs;
		private byte[] buff;

		public Client() {
			buff = new byte[1024];
			Sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			SockAsyncArgs = new SocketAsyncEventArgs();
			SockAsyncArgs.Completed += SockAsyncArgs_Completed;
		}

		public void ConnectAsync(string Address, int Port) {
			SockAsyncArgs.RemoteEndPoint = new DnsEndPoint(Address, Port);
			ConnectAsync(SockAsyncArgs);
		}

		private void ConnectAsync(SocketAsyncEventArgs e) {
			bool willRaiseEvent = Sock.ConnectAsync(e);
			if (!willRaiseEvent)
				ProcessConnect(e);
		}

		public void SendAsync(string data) {
			if (Sock.Connected && data.Length > 0) {
				byte[] buff = Encoding.UTF8.GetBytes(data);
				SocketAsyncEventArgs e = new SocketAsyncEventArgs();
				e.SetBuffer(buff, 0, buff.Length);
				e.Completed += SockAsyncArgs_Completed;
				SendAsync(e);
			}
		}

		private void SendAsync(SocketAsyncEventArgs e) {
			bool willRaiseEvent = Sock.SendAsync(e);
			if (!willRaiseEvent) {
				ProcessSend(e);
			}
		}

		private void ReceiveAsync(SocketAsyncEventArgs e) {
			if (Sock.ReceiveAsync(e) == false) {
				ProcessReceive(e);
			}
		}

		void SockAsyncArgs_Completed(object sender, SocketAsyncEventArgs e) {
			switch (e.LastOperation) {
				case SocketAsyncOperation.Connect:
					ProcessConnect(e);
					break;
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
				ReceiveAsync(SockAsyncArgs);
			} else {
				ClientMessaged("Dont send");
			}
		}

		private void ProcessReceive(SocketAsyncEventArgs e) {
			if (e.SocketError == SocketError.Success) {
				string str = Encoding.UTF8.GetString(e.Buffer, 0, e.BytesTransferred);
				ClientMessaged("Receive: {0}" + str);
			} else {
				ClientMessaged("Dont recieve");
			}
		}

		private void ProcessConnect(SocketAsyncEventArgs e) {
			if (e.SocketError == SocketError.Success) {
				SockAsyncArgs.SetBuffer(buff, 0, buff.Length);
			} else {
				ClientMessaged("Dont connect to {0}" + e.RemoteEndPoint.ToString());
			}
		}
	}
}