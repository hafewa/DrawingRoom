namespace ShapesAndInheritance
{
	partial class DrawingRoom
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.startRoom = new System.Windows.Forms.Button();
            this.joinRoom = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.offlineButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // startRoom
            // 
            this.startRoom.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.startRoom.Location = new System.Drawing.Point(138, 12);
            this.startRoom.Name = "startRoom";
            this.startRoom.Size = new System.Drawing.Size(104, 23);
            this.startRoom.TabIndex = 0;
            this.startRoom.Text = "Start New";
            this.startRoom.UseVisualStyleBackColor = true;
            this.startRoom.Click += new System.EventHandler(this.startRoom_Click);
            // 
            // joinRoom
            // 
            this.joinRoom.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.joinRoom.Location = new System.Drawing.Point(12, 12);
            this.joinRoom.Name = "joinRoom";
            this.joinRoom.Size = new System.Drawing.Size(104, 23);
            this.joinRoom.TabIndex = 1;
            this.joinRoom.Text = "Join Existing Room";
            this.joinRoom.UseVisualStyleBackColor = true;
            this.joinRoom.Click += new System.EventHandler(this.joinRoom_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(12, 58);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(104, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // offlineButton
            // 
            this.offlineButton.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.offlineButton.Location = new System.Drawing.Point(138, 58);
            this.offlineButton.Name = "offlineButton";
            this.offlineButton.Size = new System.Drawing.Size(104, 23);
            this.offlineButton.TabIndex = 3;
            this.offlineButton.Text = "Go Offline";
            this.offlineButton.UseVisualStyleBackColor = true;
            this.offlineButton.Click += new System.EventHandler(this.offlineButton_Click);
            // 
            // DrawingRoom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(254, 93);
            this.Controls.Add(this.offlineButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.joinRoom);
            this.Controls.Add(this.startRoom);
            this.Name = "DrawingRoom";
            this.Text = "DrawingRoom";
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button startRoom;
		private System.Windows.Forms.Button joinRoom;
		private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button offlineButton;
	}
}