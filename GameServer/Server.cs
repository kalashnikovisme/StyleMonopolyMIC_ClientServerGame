using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace GameServer {
	public class Server {
		public delegate void ClientConnectedEventHandler();
		public event ClientConnectedEventHandler ClientConnected;
		public Server() {
			
		}

		public void Init() {
			TcpListener tcpListener = new TcpListener(9090);
			tcpListener.Start();
			Socket socket4Client = tcpListener.AcceptSocket();
			try {
				if (socket4Client.Connected) {
					ClientConnected();
					NetworkStream networkStream = new NetworkStream(socket4Client);
					StreamReader streamReader = new StreamReader(networkStream);
					string line = streamReader.ReadLine();
				}
				socket4Client.Close();
			} catch {
			}
		}
	}
}