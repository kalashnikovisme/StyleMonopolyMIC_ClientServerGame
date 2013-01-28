using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace ClientNameSpace {
	public class Client {
		public Client() {
			try {
				TcpClient clientSocket = new TcpClient("localhost", 9090); //port is same as in server code.
				StreamWriter streamWriter = new StreamWriter(clientSocket.GetStream());
				streamWriter.WriteLine("Hello from ClientNameSpace");
				streamWriter.Flush();
				streamWriter.Close();
			} catch (Exception ex) {
			}
		}
	}
}