using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimpleNetwork;
using System.Net;

namespace ShapesAndInheritance
{
	public partial class JoinRoom : Form
	{
		//List<Room> available = new List<Room>();
		List<string> owners = new List<string>();
		Dictionary<int, Room> boxes = new Dictionary<int, Room>();
		int chosen;
        UdpCommunicator com;

		public JoinRoom(Dictionary<int, Room> r, UdpCommunicator c)
		{
			InitializeComponent();
			//this.available = rooms;
			this.boxes = r;
			this.roomsLabel.Text = RoomsToString();
            this.com = c;
		}

		private string RoomsToString()
		{
			string s = "";
			for (int i = 0; i < this.boxes.Count; i++)
			{
				//s += this.available[i].ToInt() + "-" + this.available[i].Owner.ToString() + " ";
                s += this.boxes.ElementAt(i).Value.ToInt() + "-" + this.boxes.ElementAt(i).Value.Owner + " ";
			}
			return s;
		}

		public int GetRoom()
		{
			return this.chosen;
		}

		private void joinButton_Click(object sender, EventArgs e)
		{
			bool real = false;
			int entered = int.Parse(textBox1.Text);
			if (TryRoom(entered) == false)
			{
				real = false;
			}
			else
			{
				real = true;
			}
			if (real == false)
			{
				joinButton_Click(sender, e);
			}
		}

		private bool TryRoom(int entered)
		{
			int[] keys = this.boxes.Keys.ToArray();
			for (int i = 0; i < keys.Count(); i++)
			{
				if (entered == keys[i])
				{				
					this.chosen = entered;
					return true;
				}
			}
			errorLabel.Text = "Room is not available, please choose another.";
			return false;
		}

		private void Pause()
		{
			for (int i = 0; i < 100000; i++)
			{
			}
		}

        private void refreshButton_Click(object sender, EventArgs e)
        {
            this.com.SendMessage("Requesting all available rooms");
            this.Refresh();
        }
	}
}
