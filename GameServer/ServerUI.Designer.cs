namespace GameServer {
	partial class ServerUI {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Position = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.serverInfoLabel = new System.Windows.Forms.Label();
			this.gameCountLabel = new System.Windows.Forms.Label();
			this.serverMessageLabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.SuspendLayout();
			// 
			// dataGridView1
			// 
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
			this.Name,
			this.Position});
			this.dataGridView1.Location = new System.Drawing.Point(15, 19);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.Size = new System.Drawing.Size(396, 332);
			this.dataGridView1.TabIndex = 0;
			// 
			// Name
			// 
			this.Name.HeaderText = "Name";
			this.Name.Name = "Name";
			// 
			// Position
			// 
			this.Position.HeaderText = "Position";
			this.Position.Name = "Position";
			// 
			// serverInfoLabel
			// 
			this.serverInfoLabel.AutoSize = true;
			this.serverInfoLabel.Location = new System.Drawing.Point(12, 354);
			this.serverInfoLabel.Name = "serverInfoLabel";
			this.serverInfoLabel.Size = new System.Drawing.Size(70, 13);
			this.serverInfoLabel.TabIndex = 1;
			this.serverInfoLabel.Text = "Игроков нет";
			// 
			// gameCountLabel
			// 
			this.gameCountLabel.AutoSize = true;
			this.gameCountLabel.Location = new System.Drawing.Point(175, 354);
			this.gameCountLabel.Name = "gameCountLabel";
			this.gameCountLabel.Size = new System.Drawing.Size(70, 13);
			this.gameCountLabel.TabIndex = 2;
			this.gameCountLabel.Text = "Игроков нет";
			// 
			// serverMessageLabel
			// 
			this.serverMessageLabel.AutoSize = true;
			this.serverMessageLabel.Location = new System.Drawing.Point(341, 354);
			this.serverMessageLabel.Name = "serverMessageLabel";
			this.serverMessageLabel.Size = new System.Drawing.Size(70, 13);
			this.serverMessageLabel.TabIndex = 3;
			this.serverMessageLabel.Text = "Игроков нет";
			// 
			// ServerUI
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(426, 386);
			this.Controls.Add(this.serverMessageLabel);
			this.Controls.Add(this.gameCountLabel);
			this.Controls.Add(this.serverInfoLabel);
			this.Controls.Add(this.dataGridView1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.DataGridViewTextBoxColumn Name;
		private System.Windows.Forms.DataGridViewTextBoxColumn Position;
		private System.Windows.Forms.Label serverInfoLabel;
		private System.Windows.Forms.Label gameCountLabel;
		private System.Windows.Forms.Label serverMessageLabel;
	}
}

