using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GameServer {
	public partial class ServerUI : Form {
		public ServerUI() {
			InitializeComponent();
			this.Show();
			Server server = new Server();
			server.ClientConnected += new Server.ClientConnectedEventHandler(server_ClientConnected);
			server.Init();
		}

		private void server_ClientConnected() {
			label1.Text = "Игрок подключился";
		}
	}
}