using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace GameServer {
	public class Server {
		private const string CLOSE = "close";

		private const int MAX_PLAYERS = 1;

		public delegate void ServerMessagedEventHandler(string message);
		public event ServerMessagedEventHandler ServerMessaged;

		public delegate void ServerStartedEventHandler();
		public event ServerStartedEventHandler ServerStarted;

		public delegate void ConnectedClientsCountChangedEventHandler(int count);
		public event ConnectedClientsCountChangedEventHandler ConnectClientsCountChanged;

		private Socket Sock;
		private SocketAsyncEventArgs AcceptAsyncArgs;
		private ClientConnection Client;

		public Server() {
			Sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			AcceptAsyncArgs = new SocketAsyncEventArgs();
			AcceptAsyncArgs.Completed += AcceptCompleted;
		}

		private void AcceptCompleted(object sender, SocketAsyncEventArgs e) {
			if (e.SocketError == SocketError.Success) {
				Client = new ClientConnection(e.AcceptSocket);
				Client.ServerMessaged += new ClientConnection.ServerMessagedEventHandler(Client_ServerMessaged);
				ConnectClientsCountChanged(ClientConnection.ClientCount);
				if (ClientConnection.ClientCount > MAX_PLAYERS) {
					System.Windows.Forms.MessageBox.Show("Максимальное количество игроков");
					e.AcceptSocket.Disconnect(true);
					Client.SendAsync("close");
					return;
				}
				ConnectClientsCountChanged(ClientConnection.ClientCount);
			} else {
				return;
			}
			e.AcceptSocket = null;
			SockAcceptAsync(AcceptAsyncArgs);
			Client = new ClientConnection(e.AcceptSocket);
			Client.SendAsync(new GameItems.Player(ClientConnection.ClientCount - 1));
		}

		private void SockAcceptAsync(SocketAsyncEventArgs e) {
			if (Sock.AcceptAsync(e) == false) {
				AcceptCompleted(Sock, e);
			}
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

		private void Client_ServerMessaged(string message) {
			ServerMessaged(message);
		}
	}
}