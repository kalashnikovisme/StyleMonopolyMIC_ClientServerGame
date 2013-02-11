using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Json;
using GameItems;
using System.IO;

namespace ClientNameSpace {
	public partial class Client {
		public Socket clientSocket;
		private byte[] byteData = new byte[1024];
		private Player mainPlayer;
		private List<Player> allPlayers;

		public Client() { 
			/// Инициализировать MainForm
			initClient();
		}

		public Client(Player inPlayer) {
			/// Инициализировать MainForm
			mainPlayer = new Player(inPlayer.Name);
			initClient();
		}

		private void initClient() {
			string commandToSend = Command.LIST + Command.SEPARATOR;
			byteData = Encoding.UTF8.GetBytes(commandToSend);
			clientSocket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnSend), null);
			byteData = new byte[1024];
			clientSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);
		}

		private void btnSend_Click(object sender, EventArgs e) {
			try {
				string json = "message." + JSON.SerializePlayer(mainPlayer);
				byte[] byteData = Encoding.UTF8.GetBytes(json);
				clientSocket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnSend), null);
			} catch (Exception exc) {
				MessageBox.Show("Unable to send message to the server. Error: " + exc.Message, "SGSclientTCP: " + mainPlayer.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void OnSend(IAsyncResult ar) {
			try {
				clientSocket.EndSend(ar);
			} catch (ObjectDisposedException) { } catch (Exception ex) {
				MessageBox.Show(ex.Message, "SGSclientTCP: " + mainPlayer.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void OnReceive(IAsyncResult ar) {
			try {
				clientSocket.EndReceive(ar);
				string json_Datas = Encoding.UTF8.GetString(byteData);
				string command = JSON.GetCommandFromJSONString(json_Datas);
				switch (command) {
					case Command.LOGIN:
						/// Добавить в список игроков под нужным индексом
						break;
					case Command.LOGOUT:
						/// назначить в игре статус проиграл
						break;
					case Command.MESSAGE:
						/// назначить соответствующему пользователю соответствующие баллы
						break;
					case Command.LIST:
						/// добавить остальных игроков в игру
						break;
				}
				byteData = new byte[1024];
				clientSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);
			} catch (ObjectDisposedException) { } catch (Exception ex) {
				MessageBox.Show(ex.Message, "СlientTCP: " + mainPlayer.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void SGSClient_FormClosing(object sender, FormClosingEventArgs e) {
			if (MessageBox.Show("Are you sure you want to leave the game?", "ClientNameSpace: " + mainPlayer.Name, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No) {
				e.Cancel = true;
				return;
			}

			try {
				string commandToSend = Command.LOGOUT + Command.SEPARATOR;

				byte[] bytes = Encoding.UTF8.GetBytes(commandToSend);
				clientSocket.Send(bytes, 0, bytes.Length, SocketFlags.None);
				clientSocket.Close();
			} catch (ObjectDisposedException) { } catch (Exception ex) {
				MessageBox.Show(ex.Message, "ClientTCP: " + mainPlayer.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}