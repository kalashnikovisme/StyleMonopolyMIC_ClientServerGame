using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ClientNameSpace;

namespace MIC_Monopolia {
	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			LoginForm loginForm = new LoginForm();

			Application.Run(loginForm);
			if (loginForm.DialogResult == DialogResult.OK) {
				Client client = new Client();
				client.ClientSocket = loginForm.ClientSocket;
				client.PlayerName = loginForm.PlayerName;
				client.InitConnection();
			}
		}
	}
}