﻿using System;
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
using MIC_Monopolia;

namespace ClientNameSpace {
	public partial class Client {
		public Socket ClientSocket;
		private byte[] byteData = new byte[1024];
		private Player mainPlayer;
		private List<Player> allPlayers;
		private MainField main;
		public Client() { 
			mainPlayer = new Player();
			main = new MainField(40);			
			main.PlayerChanged += main_PlayerChanged;
		}

		public Client(Player inPlayer) {
			mainPlayer = new Player(inPlayer.Name);
			main = new MainField(40);
			main.PlayerChanged += main_PlayerChanged;
		}

		private void main_PlayerChanged(Player changedPlayer) {
			string jsonToSend = Command.MESSAGE + Command.SEPARATOR + JSON.SerializePlayer(changedPlayer);
			byteData = Encoding.UTF8.GetBytes(jsonToSend);
			ClientSocket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnSend), null);			
		}

		public string PlayerName {
			get {
				return mainPlayer.Name;
			}
			set {
				mainPlayer.Name = value;
			}
		}

		public void InitConnection() {
			string commandToSend = Command.LIST + Command.SEPARATOR;
			byteData = Encoding.UTF8.GetBytes(commandToSend);
			ClientSocket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnSend), null);
			byteData = new byte[1024];
			ClientSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);

			allPlayers = new List<Player>();	
			main.ShowDialog();
		}

		private void btnSend_Click(object sender, EventArgs e) {
			try {
				string json = "message." + JSON.SerializePlayer(mainPlayer);
				byte[] byteData = Encoding.UTF8.GetBytes(json);
				ClientSocket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnSend), null);
			} catch (Exception exc) {
				MessageBox.Show("Unable to send message to the server. Error: " + exc.Message, "SGSclientTCP: " + mainPlayer.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void OnSend(IAsyncResult ar) {
			try {
				ClientSocket.EndSend(ar);
			} catch (ObjectDisposedException) { } catch (Exception ex) {
				MessageBox.Show(ex.Message, "ClientTCP: " + mainPlayer.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void OnReceive(IAsyncResult ar) {
			try {
				ClientSocket.EndReceive(ar);
				string json_Datas = Encoding.UTF8.GetString(byteData);
				string command = JSON.GetCommandFromJSONString(json_Datas);
				Player newPlayer = null;
				int startIndex = json_Datas.IndexOf(Command.SEPARATOR) + 1;
				string jsonToSend = "";
				switch (command) {
					case Command.LOGIN:
						newPlayer = JSON.DeserializeData(json_Datas.Substring(startIndex, json_Datas.LastIndexOf('}') + 1 - startIndex));
						if (main.InvokeRequired) {
							setMainPlayerDelegate setMainPlayer = new setMainPlayerDelegate(setMainPlayerCallBack);
							main.Invoke(setMainPlayer, newPlayer);
						} else {
							main.SetMainPlayer(newPlayer);
						}
						jsonToSend = Command.LIST + Command.SEPARATOR;
						break;
					case Command.LOGOUT:
						/// назначить в игре статус проиграл
						break;
					case Command.MESSAGE:
						jsonToSend = Command.MESSAGE + Command.SEPARATOR + JSON.SerializePlayer(mainPlayer);
						break;
					case Command.LIST:
						//Newtonsoft.Json.Linq.JObject jObj = Newtonsoft.Json.Linq.JObject.Parse(json_Datas.Substring(startIndex, json_Datas.LastIndexOf('}') + 1 - startIndex));
						//Player[] pl = Newtonsoft.Json.JsonConvert.DeserializeObject<Player[]>(jObj[0].ToString());
						//allPlayers.Clear();
						//allPlayers.AddRange(pl);
						break;
				}
				if (command != Command.LIST) {
					byteData = Encoding.UTF8.GetBytes(jsonToSend);
					ClientSocket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnSend), null);
				}
				byteData = new byte[1024];
				ClientSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);
			} catch (ObjectDisposedException) { } catch (Exception ex) {
				MessageBox.Show(ex.Message, "СlientTCP: " + mainPlayer.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private delegate void setMainPlayerDelegate(Player player);
		private void setMainPlayerCallBack(Player player) {
			main.SetMainPlayer(player);
		}

		private void SGSClient_FormClosing(object sender, FormClosingEventArgs e) {
			if (MessageBox.Show("Are you sure you want to leave the game?", "ClientNameSpace: " + mainPlayer.Name, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No) {
				e.Cancel = true;
				return;
			}
			try {
				string commandToSend = Command.LOGOUT + Command.SEPARATOR;
				byte[] bytes = Encoding.UTF8.GetBytes(commandToSend);
				ClientSocket.Send(bytes, 0, bytes.Length, SocketFlags.None);
				ClientSocket.Close();
			} catch (ObjectDisposedException) { } catch (Exception ex) {
				MessageBox.Show(ex.Message, "ClientTCP: " + mainPlayer.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}