// Hunter Dean - Period 5

// This program allows users to draw shapes on a form, either online
// or offline. Online, they can pick rooms and draw alongside other
// online users.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using SimpleNetwork;
using System.Net;
using System.Net.Sockets;

namespace ShapesAndInheritance
{
    // The user interface and acts as the "game control" for the
    // program
	public partial class Form1 : Form
	{
        // Drawing
		private DrawingEngine smeargle;

        // Network Information
		private UdpCommunicator com = new UdpCommunicator(IPAddress.Parse("224.100.0.2"), 9050);
		private UdpCommunicator soloCom = new UdpCommunicator(IPAddress.Parse("224.100.0.3"), 9051);
		public enum Connectivity { Offline, Online };
		Connectivity mode = Connectivity.Offline;
        IPAddress ip;
        IPEndPoint selfEp;
        bool usernameEntered = false;
		int currentRoom;
		List<string> roomOwners = new List<string>();
		DrawingRoom roomController;

        // Constructor, sets all values to and requests available rooms
		public Form1()
		{
			InitializeComponent();
			KeyPreview = true;
			
			this.com.IncomingMessage += com_IncomingMessage;
			this.soloCom.IncomingMessage += soloCom_IncomingMessage;
            this.ip = GetIp();
            this.selfEp = new IPEndPoint(this.ip, 9050);
            this.roomController = new DrawingRoom("", this.selfEp, this.com);
			this.smeargle = new DrawingEngine(this.com, this.mode);			
			this.toolkit.SelectedIndex = 0;
			this.colorBox.SelectedIndex = 9;
			this.outlineBox.SelectedIndex = 9;
			this.modeBox.SelectedIndex = 0;
			this.RequestingCreatedRooms();
		}

        // Not implemented yet
		void soloCom_IncomingMessage(object sender, IncomingMessageEventArgs e)
		{			
			throw new NotImplementedException();
		}

        // Incoming message handler
		void com_IncomingMessage(object sender, IncomingMessageEventArgs e)
		{
			this.HandleMessage(e.Message, e.EndPoint);

			this.pictureBox1.Invalidate();
		}

        // Gets the IP of the computer being used
        private IPAddress GetIp()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            throw new Exception("IP address not found!");
        }

        // Handles the received message depending on what it contains
		private void HandleMessage(string message, IPEndPoint ep)
		{
			if (message.Contains("X") && !message.Contains("Room info"))
			{
				this.smeargle.ConvertShape(message);
                AddShapeToRoom(message);
			}
            if (message.Contains("Deselected"))
            {
                if (ep != this.selfEp)
                    this.smeargle.DeselectAll();
            }
			if (message.Contains("Requesting all available rooms"))
			{
				SendingAllAvailableRooms(ep);
			}
			if (message.Contains("Room added"))
			{
				AddRoom(message, ep);
			}
			if (message.Contains("All rooms open"))
			{
				ReceivingAllAvailableRooms(message, ep);
			}
			if (message.Contains("Requesting room info"))
			{
                string[] info = message.Split('|');
                if (this.roomController.Contains(int.Parse(info[1])))
				    SendingRoomInfo(message, ep);
			}
			if (message.Contains("Room info for"))
			{
				ReceivingRoomInfo(message, ep);
			}
		}

        // Sends a message to let other users know a room was created
		private void RoomCreated(int r)
		{
			this.com.SendMessage("Room added" + "|" + r.ToString() + "|" + this.smeargle.GetUsername());
		}

        // Adds a room to the roomcontroller
		private void AddRoom(string message, IPEndPoint e)
		{
			string[] info = message.Split('|');
            this.roomController.AddRoom(int.Parse(info[1]), info[2], e);
			this.roomOwners.Add(info[2]);
		}

        // Adds a shape to the room through the roomcontroller
        private void AddShapeToRoom(string message)
        {
            this.roomController.ConvertShape(message);
        }

        // Asks all users what rooms are available (if just logged online)
		private void RequestingCreatedRooms()
		{
			this.com.SendMessage("Requesting all available rooms");
		}

        // Sends the available rooms to the requester
		private void SendingAllAvailableRooms(IPEndPoint ep)
		{
            if (!ep.Equals(this.selfEp))
            {
                this.com.SendMessage("All rooms open" + "|" + this.roomController.RoomsToString(), ep);
            }
		}

        // Adds the available rooms to the roomcontroller
        private void ReceivingAllAvailableRooms(string rooms, IPEndPoint ep)
        {
            if (!ep.Equals(this.selfEp))
            {
                this.roomController.ReceivingRooms(rooms, ep);
            }
        }

        // Requests info for a specific room number
		private void RequestingRoomInfo()
		{
            this.com.SendMessage("Requesting room info|" + this.currentRoom, this.roomController.DictionaryOfRooms()[this.currentRoom].IP());
		}

        // Sends the info for a specific room, except if the requester is oneself
		private void SendingRoomInfo(string message, IPEndPoint ep)
		{
            if (!ep.Equals(this.selfEp))
            {
                string[] info = message.Split('|');
                string send = "";
                send = "Room info for&" + info[1]
                    + "&" + this.roomController.RoomInformation(int.Parse(info[1])).ToString();

                this.com.SendMessage(send, ep);
            }
            else if (this.roomController.Contains(this.currentRoom))
            {
                string[] shapes = this.roomController.RoomInformation(this.currentRoom).ToString().Split('@');
                this.smeargle.ClearShapes();
                for (int i = 0; i < shapes.Count() - 1; i++)
                {
                    this.smeargle.ConvertShape(shapes[i]);
                }
            }
		}

        // Translates the room info to the roomcontroller by adding the shapes to the room
		private void ReceivingRoomInfo(string message, IPEndPoint ep)
		{
            if (!ep.Equals(this.selfEp) && !this.roomController.ContainsRoomInfo(this.currentRoom))
            {
                string[] info = message.Split('&');
                string[] shapes = info[2].Split('@');
                this.smeargle.ClearShapes();
                for (int i = 0; i < shapes.Count() - 1; i++)
                {
                    this.smeargle.ConvertShape(shapes[i]);
                }
                this.roomController.AddListOfShapesToRoom(this.currentRoom, this.smeargle.Shapes());
            }
		}

		private void pictureBox1_Paint(object sender, PaintEventArgs e)
		{			
			this.smeargle.Draw(e.Graphics, this.mode);
		}

		private void pictureBox1_Resize(object sender, EventArgs e)
		{
			this.pictureBox1.Invalidate();
		}

		private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Right)
			{
				rightClickMenu.Show(MousePosition);
				this.smeargle.MouseRightClick(e.X, e.Y);
			}
			else
				this.smeargle.MouseDown((string)this.toolkit.SelectedItem, e.X, e.Y);

			this.pictureBox1.Invalidate();
		}

		private void Form1_MouseMove(object sender, MouseEventArgs e)
		{
			this.smeargle.MouseMove(new PointF(e.X, e.Y));
			this.pictureBox1.Invalidate();
		}

		private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
		{
			this.smeargle.MouseUp(new PointF(e.X, e.Y));
			this.pictureBox1.Invalidate();
		}

		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			//this.smeargle.KeyDown(e.KeyValue);
		}

		private void Form1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
		}

		private void colorBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			string item = (string)this.colorBox.SelectedItem;
			if (item == "Color Dialog")
			{
				colorDialog1.ShowDialog();
				this.smeargle.ColorWheel(colorDialog1.Color, "fill");
			}
			else
				this.smeargle.HandleColors((string)this.colorBox.SelectedItem);
		}

		private void outlineBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			string item = (string)this.outlineBox.SelectedItem;
			if (item == "Color Dialog")
			{
				colorDialog1.ShowDialog();
				this.smeargle.ColorWheel(colorDialog1.Color, "outline");
			}
			else
				this.smeargle.HandleOutline((string)this.outlineBox.SelectedItem);
		}

		private void bringToFront_Click(object sender, EventArgs e)
		{
			this.smeargle.BringToFront();
		}

		private void bringForward_Click(object sender, EventArgs e)
		{
			this.smeargle.BringForward();
		}

		private void delete_Click(object sender, EventArgs e)
		{
			this.smeargle.Delete();
		}

		private void sendToBack_Click(object sender, EventArgs e)
		{
			this.smeargle.SendToBack();
		}

		private void sendBackward_Click(object sender, EventArgs e)
		{
			this.smeargle.SendBackward();
		}

		public Color ColorWheel(string s)
		{
			return colorDialog1.Color;
		}

		private void clone_Click(object sender, EventArgs e)
		{
			this.smeargle.Clone();
		}

		protected override bool ProcessDialogKey(Keys keyData)
		{
			Debug.WriteLine("Key Pressed" + keyData);

			return base.ProcessDialogKey(keyData);
		}

        // Changes between offline and online. First time online creates a username
		private void modeBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch (modeBox.SelectedItem.ToString())
			{
				case "Offline":
					this.mode = Connectivity.Offline;
					this.smeargle.EditConnectivity(this.mode);
					break;
				case "Online":
					if (this.mode == Connectivity.Online)
						return;
					if (!this.usernameEntered)
					{
						Username u = new Username();
						DialogResult r = u.ShowDialog();
						if (r == DialogResult.OK)
						{
							this.smeargle.EditUsername(u.GetName());
							this.roomController.EditUsername(this.smeargle.GetUsername());
							usernameLabel.Text = this.smeargle.GetUsername();
						}
						else if (r == DialogResult.Cancel)
							return;
						this.usernameEntered = true;
					}
					
                    this.roomController.Result = DialogResult.No;
                    if (RoomSelector() == 1)
                    {
                        this.mode = Connectivity.Online;
                        this.smeargle.EditConnectivity(this.mode);
                        this.modeBox.SelectedIndex = 1;
                    }
                    else
                    {
                        this.mode = Connectivity.Offline;
                        this.modeBox.SelectedIndex = 0;
                    }
					break;
				default:
					break;
			}
		}

        // Completes the actual changing or selecting of rooms by using 
        // the ChooseRoom() method to access the requested action
		private int RoomSelector()
		{
			int[] roomInfo = ChooseRoom();
			int room = roomInfo[0];
			int action = roomInfo[1];
			if (room == -1)
			{
                return 1;
			}
            if (room == -42)
            {
                this.currentRoomLabel.Text = "Offline";
                if (this.smeargle.Shapes().Count > 0)
                {
                    this.roomController.AddListOfShapesToRoom(this.currentRoom, this.smeargle.Shapes());
                }
                this.mode = Connectivity.Offline;
                this.smeargle.ClearShapes();
                this.currentRoom = -1;
                this.smeargle.EditConnectivity(Connectivity.Offline);
                modeBox.SelectedIndex = 0;
                return -1;
            }
			if (action == 0)
			{
				this.currentRoom = room;
				this.smeargle.EditRoom(this.currentRoom);
				RoomCreated(this.currentRoom);
				this.smeargle.ClearShapes();
                currentRoomLabel.Text = currentRoom.ToString();
                return 1;
			}
			if (action == 1)
			{
				this.currentRoom = room;
				this.smeargle.EditRoom(this.currentRoom);
                this.smeargle.ClearShapes();
				RequestingRoomInfo();
                this.pictureBox1.Invalidate();
                currentRoomLabel.Text = currentRoom.ToString();
                return 1;
			}
			currentRoomLabel.Text = currentRoom.ToString();
            return -1;
		}

        // Opens a series of forms to request info from the user about room selection
		private int[] ChooseRoom()
		{
			while (roomController.Result == DialogResult.No)
			{
				DialogResult r = roomController.ShowDialog();
				r = roomController.Result;
				if (r == DialogResult.OK)
				{
					return new int[] { roomController.ChosenRoom(), roomController.Action() };
				}
				else if (r == DialogResult.Yes)
				{
					return new int[] { roomController.ChosenRoom(), roomController.Action() };
				}
				if (r == DialogResult.Cancel)
				{
                    if (this.mode == Connectivity.Offline)
                        return new int[] { -1, -1 };
                    else
                        return new int[] { -1, 1 };
				}
                if (r == DialogResult.Abort)
                {
                    return new int[] { -42, -1 };
                }
			}
			return new int[] { -1, -1 };
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			this.smeargle.EditUsername(textBox1.Text);
		}

        // Allows the user to change rooms if online
		private void changeRoomButton_Click(object sender, EventArgs e)
		{
            if (this.mode == Connectivity.Online)
            {
                roomController.Result = DialogResult.No;
                RoomSelector();
            }
		}
	}
}
