namespace ShapesAndInheritance
{
	partial class Form1
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
			this.components = new System.ComponentModel.Container();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.toolkit = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.colorBox = new System.Windows.Forms.ComboBox();
			this.outlineLabel = new System.Windows.Forms.Label();
			this.outlineBox = new System.Windows.Forms.ComboBox();
			this.rightClickMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.bringToFront = new System.Windows.Forms.ToolStripMenuItem();
			this.bringForward = new System.Windows.Forms.ToolStripMenuItem();
			this.sendToBack = new System.Windows.Forms.ToolStripMenuItem();
			this.sendBackward = new System.Windows.Forms.ToolStripMenuItem();
			this.delete = new System.Windows.Forms.ToolStripMenuItem();
			this.clone = new System.Windows.Forms.ToolStripMenuItem();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			this.modeLabel = new System.Windows.Forms.Label();
			this.modeBox = new System.Windows.Forms.ComboBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.usernameLabel = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.drawingRoomLabel = new System.Windows.Forms.Label();
			this.currentRoomLabel = new System.Windows.Forms.Label();
			this.changeRoomButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.rightClickMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox1.Location = new System.Drawing.Point(1, 61);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(1220, 514);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
			this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
			this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
			this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
			this.pictureBox1.Resize += new System.EventHandler(this.pictureBox1_Resize);
			// 
			// toolkit
			// 
			this.toolkit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.toolkit.FormattingEnabled = true;
			this.toolkit.Items.AddRange(new object[] {
            "Selection Tool",
            "Rectangle",
            "Square",
            "Oval",
            "Circle",
            "Triangle"});
			this.toolkit.Location = new System.Drawing.Point(49, 3);
			this.toolkit.Name = "toolkit";
			this.toolkit.Size = new System.Drawing.Size(166, 21);
			this.toolkit.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 6);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(31, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Tool:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(235, 6);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(34, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Color:";
			// 
			// colorBox
			// 
			this.colorBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.colorBox.FormattingEnabled = true;
			this.colorBox.Items.AddRange(new object[] {
            "Transparent",
            "Black",
            "Blue",
            "Red",
            "Yellow",
            "Orange",
            "Green",
            "Purple",
            "White",
            "Random",
            "Color Dialog"});
			this.colorBox.Location = new System.Drawing.Point(275, 3);
			this.colorBox.Name = "colorBox";
			this.colorBox.Size = new System.Drawing.Size(151, 21);
			this.colorBox.TabIndex = 4;
			this.colorBox.SelectedIndexChanged += new System.EventHandler(this.colorBox_SelectedIndexChanged);
			// 
			// outlineLabel
			// 
			this.outlineLabel.AutoSize = true;
			this.outlineLabel.Location = new System.Drawing.Point(452, 6);
			this.outlineLabel.Name = "outlineLabel";
			this.outlineLabel.Size = new System.Drawing.Size(43, 13);
			this.outlineLabel.TabIndex = 5;
			this.outlineLabel.Text = "Outline:";
			// 
			// outlineBox
			// 
			this.outlineBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.outlineBox.FormattingEnabled = true;
			this.outlineBox.Items.AddRange(new object[] {
            "Transparent",
            "Black",
            "Blue",
            "Red",
            "Yellow",
            "Orange",
            "Green",
            "Purple",
            "White",
            "Random",
            "Color Dialog"});
			this.outlineBox.Location = new System.Drawing.Point(501, 3);
			this.outlineBox.Name = "outlineBox";
			this.outlineBox.Size = new System.Drawing.Size(152, 21);
			this.outlineBox.TabIndex = 6;
			this.outlineBox.SelectedIndexChanged += new System.EventHandler(this.outlineBox_SelectedIndexChanged);
			// 
			// rightClickMenu
			// 
			this.rightClickMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bringToFront,
            this.bringForward,
            this.sendToBack,
            this.sendBackward,
            this.delete,
            this.clone});
			this.rightClickMenu.Name = "rightClickMenu";
			this.rightClickMenu.Size = new System.Drawing.Size(155, 136);
			// 
			// bringToFront
			// 
			this.bringToFront.Name = "bringToFront";
			this.bringToFront.Size = new System.Drawing.Size(154, 22);
			this.bringToFront.Text = "Bring to front";
			this.bringToFront.Click += new System.EventHandler(this.bringToFront_Click);
			// 
			// bringForward
			// 
			this.bringForward.Name = "bringForward";
			this.bringForward.Size = new System.Drawing.Size(154, 22);
			this.bringForward.Text = "Bring forward";
			this.bringForward.Click += new System.EventHandler(this.bringForward_Click);
			// 
			// sendToBack
			// 
			this.sendToBack.Name = "sendToBack";
			this.sendToBack.Size = new System.Drawing.Size(154, 22);
			this.sendToBack.Text = "Send to back";
			this.sendToBack.Click += new System.EventHandler(this.sendToBack_Click);
			// 
			// sendBackward
			// 
			this.sendBackward.Name = "sendBackward";
			this.sendBackward.Size = new System.Drawing.Size(154, 22);
			this.sendBackward.Text = "Send backward";
			this.sendBackward.Click += new System.EventHandler(this.sendBackward_Click);
			// 
			// delete
			// 
			this.delete.Name = "delete";
			this.delete.Size = new System.Drawing.Size(154, 22);
			this.delete.Text = "Delete";
			this.delete.Click += new System.EventHandler(this.delete_Click);
			// 
			// clone
			// 
			this.clone.Name = "clone";
			this.clone.Size = new System.Drawing.Size(154, 22);
			this.clone.Text = "Clone";
			this.clone.Click += new System.EventHandler(this.clone_Click);
			// 
			// colorDialog1
			// 
			this.colorDialog1.AnyColor = true;
			// 
			// modeLabel
			// 
			this.modeLabel.AutoSize = true;
			this.modeLabel.Location = new System.Drawing.Point(673, 6);
			this.modeLabel.Name = "modeLabel";
			this.modeLabel.Size = new System.Drawing.Size(68, 13);
			this.modeLabel.TabIndex = 7;
			this.modeLabel.Text = "Connectivity:";
			// 
			// modeBox
			// 
			this.modeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.modeBox.FormattingEnabled = true;
			this.modeBox.Items.AddRange(new object[] {
            "Offline",
            "Online"});
			this.modeBox.Location = new System.Drawing.Point(747, 3);
			this.modeBox.Name = "modeBox";
			this.modeBox.Size = new System.Drawing.Size(121, 21);
			this.modeBox.TabIndex = 8;
			this.modeBox.SelectedIndexChanged += new System.EventHandler(this.modeBox_SelectedIndexChanged);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(1045, 35);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(49, 20);
			this.textBox1.TabIndex = 9;
			this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 38);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(58, 13);
			this.label3.TabIndex = 10;
			this.label3.Text = "Username:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(1100, 38);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(56, 13);
			this.label4.TabIndex = 11;
			this.label4.Text = "Received:";
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(1162, 35);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(48, 20);
			this.textBox2.TabIndex = 12;
			// 
			// usernameLabel
			// 
			this.usernameLabel.AutoSize = true;
			this.usernameLabel.Location = new System.Drawing.Point(76, 38);
			this.usernameLabel.Name = "usernameLabel";
			this.usernameLabel.Size = new System.Drawing.Size(0, 13);
			this.usernameLabel.TabIndex = 13;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(214, 38);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(117, 13);
			this.label5.TabIndex = 14;
			this.label5.Text = "Current Drawing Room:";
			// 
			// drawingRoomLabel
			// 
			this.drawingRoomLabel.AutoSize = true;
			this.drawingRoomLabel.Location = new System.Drawing.Point(300, 38);
			this.drawingRoomLabel.Name = "drawingRoomLabel";
			this.drawingRoomLabel.Size = new System.Drawing.Size(0, 13);
			this.drawingRoomLabel.TabIndex = 15;
			// 
			// currentRoomLabel
			// 
			this.currentRoomLabel.AutoSize = true;
			this.currentRoomLabel.Location = new System.Drawing.Point(337, 38);
			this.currentRoomLabel.Name = "currentRoomLabel";
			this.currentRoomLabel.Size = new System.Drawing.Size(37, 13);
			this.currentRoomLabel.TabIndex = 16;
			this.currentRoomLabel.Text = "Offline";
			// 
			// changeRoomButton
			// 
			this.changeRoomButton.Location = new System.Drawing.Point(420, 33);
			this.changeRoomButton.Name = "changeRoomButton";
			this.changeRoomButton.Size = new System.Drawing.Size(84, 23);
			this.changeRoomButton.TabIndex = 17;
			this.changeRoomButton.Text = "Change Room";
			this.changeRoomButton.UseVisualStyleBackColor = true;
			this.changeRoomButton.Click += new System.EventHandler(this.changeRoomButton_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1222, 576);
			this.Controls.Add(this.changeRoomButton);
			this.Controls.Add(this.currentRoomLabel);
			this.Controls.Add(this.drawingRoomLabel);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.usernameLabel);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.modeBox);
			this.Controls.Add(this.modeLabel);
			this.Controls.Add(this.outlineBox);
			this.Controls.Add(this.outlineLabel);
			this.Controls.Add(this.colorBox);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.toolkit);
			this.Controls.Add(this.pictureBox1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
			this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Form1_PreviewKeyDown);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.rightClickMenu.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.ComboBox toolkit;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox colorBox;
		private System.Windows.Forms.Label outlineLabel;
		private System.Windows.Forms.ComboBox outlineBox;
		private System.Windows.Forms.ContextMenuStrip rightClickMenu;
		private System.Windows.Forms.ToolStripMenuItem bringToFront;
		private System.Windows.Forms.ToolStripMenuItem bringForward;
		private System.Windows.Forms.ToolStripMenuItem sendToBack;
		private System.Windows.Forms.ToolStripMenuItem sendBackward;
		private System.Windows.Forms.ToolStripMenuItem delete;
		private System.Windows.Forms.ColorDialog colorDialog1;
		private System.Windows.Forms.ToolStripMenuItem clone;
		private System.Windows.Forms.Label modeLabel;
		private System.Windows.Forms.ComboBox modeBox;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.Label usernameLabel;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label drawingRoomLabel;
		private System.Windows.Forms.Label currentRoomLabel;
		private System.Windows.Forms.Button changeRoomButton;
	}
}

