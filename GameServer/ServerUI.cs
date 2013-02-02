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
			server.ServerStarted += new Server.ServerStartedEventHandler(server_ServerStarted);
			server.ServerMessaged += new Server.ServerMessagedEventHandler(server_ServerMessaged);
			server.ConnectClientsCountChanged += new Server.ConnectedClientsCountChangedEventHandler(server_ConnectClientsCountChanged);
			server.Start(4505);
		}

		private void server_ServerMessaged(string message) {
			if (serverMessageLabel.InvokeRequired) {
				SetLabelTextDelegate labelTextDelegate = new SetLabelTextDelegate(SetLabelTextCallBack);
				serverMessageLabel.Invoke(labelTextDelegate, gameCountLabel, message);
			} else {
				serverMessageLabel.Text = message;
			}
		}

		private void server_ConnectClientsCountChanged(int count) {
			if (gameCountLabel.InvokeRequired) {
				SetLabelTextDelegate labelTextDelegate = new SetLabelTextDelegate(SetLabelTextCallBack);
				gameCountLabel.Invoke(labelTextDelegate, gameCountLabel, "Количество игроков " + count.ToString());
			} else {
				gameCountLabel.Text = "Количество игроков " + count.ToString();
			}
		}

		private void server_ServerStarted() {
			serverInfoLabel.Text = "Сервер запущен";
		}

		private void server_ClientConnected() {
			serverInfoLabel.Text = "Игрок подключился";
		}

		private delegate void SetLabelTextDelegate(ref Label label, string message);
		private void SetLabelTextCallBack(ref Label label, string message) {
			label.Text = message;
		}
	}
}