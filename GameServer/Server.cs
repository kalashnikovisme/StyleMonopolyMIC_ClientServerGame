using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace GameServer {
	public class Server {
		private const int MAX_PLAYERS = 2;

		public delegate void ServerMessagedEventHandler(string message);
		public event ServerMessagedEventHandler ServerMessaged;

		public delegate void ServerStartedEventHandler();
		public event ServerStartedEventHandler ServerStarted;

		public delegate void ConnectedClientsCountChangedEventHandler(int count);
		public event ConnectedClientsCountChangedEventHandler ConnectClientsCountChanged;

		private Socket Sock;
		private SocketAsyncEventArgs AcceptAsyncArgs;

		public Server() {
			Sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			AcceptAsyncArgs = new SocketAsyncEventArgs();
			AcceptAsyncArgs.Completed += AcceptCompleted;
		}

		private void AcceptCompleted(object sender, SocketAsyncEventArgs e) {
			if (e.SocketError == SocketError.Success) {
				ClientConnection Client = new ClientConnection(e.AcceptSocket);
				if (ClientConnection.ClientNumber > MAX_PLAYERS) {
					System.Windows.Forms.MessageBox.Show("Максимальное количество игроков");
					e.AcceptSocket.Disconnect(true);
					return;
				}
				Client.ServerMessaged += new ClientConnection.ServerMessagedEventHandler(Client_ServerMessaged);
				ConnectClientsCountChanged(ClientConnection.ClientNumber);
			}
			e.AcceptSocket = null;
			SockAcceptAsync(AcceptAsyncArgs);
		}

		private void Client_ServerMessaged(string message) {
			//ServerMessaged(message);
		}

		private void SockAcceptAsync(SocketAsyncEventArgs e) {
			bool willRaiseEvent = Sock.AcceptAsync(e);
			if (!willRaiseEvent)
				AcceptCompleted(Sock, e);
		}

		public void Start(int Port) {
			Sock.Bind(new IPEndPoint(IPAddress.Any, Port));
			Sock.Listen(50);
			SockAcceptAsync(AcceptAsyncArgs);
			ServerStarted();
		}

		public void Stop() {
			Sock.Close();
		}
	}
}