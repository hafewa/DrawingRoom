using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using SimpleNetwork;
using System.Reflection;

namespace ShapesAndInheritance
{
    // Handles the online room information and actions
	public partial class DrawingRoom : Form
	{
        // Random
        public static Random rand = new Random();

        // Room & User Information
		Dictionary<int, Room> rooms = new Dictionary<int, Room>();
		private int chosen;
		private int action;
		private DialogResult result = DialogResult.No;
		string self;
        Dictionary<string, Brush> usernameColor = new Dictionary<string, Brush>();

        // Network
        IPEndPoint ownIp;
        UdpCommunicator com;

		public DrawingRoom(string s, IPEndPoint e, UdpCommunicator c)
		{
			InitializeComponent();

			this.self = s;
            this.ownIp = e;
            this.com = c;
		}

        // Changes the username
		public void EditUsername(string n)
		{
			this.self = n;
		}

        // Returns the Dictionary of rooms
        public Dictionary<int, Room> DictionaryOfRooms()
        {
            return this.rooms;
        }

        // Returns the room selected to join/create
		public int ChosenRoom()
		{
			return this.chosen;
		}

        // Returns the action (join, create, leave)
		public int Action()
		{
			return this.action;
		}

        // Property of the DialogResult returned
		public DialogResult Result
		{
			get { return this.result; }
			set { this.result = value; }
		}

        // Adds a new room to the list of rooms
		public void AddRoom(int n, string s, IPEndPoint ep)
		{
            if (s != this.self)
            {
                this.rooms.Add(n, new Room(n, s, ep));
            }
		}

        // Returns if the a given room has been created
        public bool Contains(int r)
        {
            int[] numbers = this.rooms.Keys.ToArray();
            for (int i = 0; i < numbers.Length; i++)
            {
                if (r == numbers[i])
                    return true;
            }
            return false;
        }

        // Returns if the given room has any shapes information it
        public bool ContainsRoomInfo(int r)
        {
            int[] numbers = this.rooms.Keys.ToArray();
            for (int i = 0; i < numbers.Length; i++)
            {
                if (r == numbers[i])
                {
                    if (this.rooms[r].Shapes().Count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        // Adds a shape to the specified room
        public void AddShapeToRoom(int room, Shape s)
		{
                this.rooms[room].AddShape(s);
		}

        // Adds a list of shapes to the specified room
		public void AddListOfShapesToRoom(int room, List<Shape> s)
		{
                this.rooms[room].AddListOfShapes(s);
		}

        // Goes through the process of creating a new room
		private void startRoom_Click(object sender, EventArgs e)
		{
			StartRoom s = new StartRoom(this.rooms);
			DialogResult d = s.ShowDialog();
			if (d == DialogResult.OK)
			{
				this.chosen = s.GetRoom();
				this.rooms.Add(this.chosen, new Room(this.chosen, this.self, this.ownIp));
				this.action = 0;
				this.result = DialogResult.OK;
			}
			if (d == DialogResult.Cancel)
			{
				this.result = DialogResult.No;
			}
		}

        // Goes through the process of joining an already created room
		private void joinRoom_Click(object sender, EventArgs e)
		{
			JoinRoom j = new JoinRoom(this.rooms, this.com);
			DialogResult d = j.ShowDialog();
			if (d == DialogResult.OK)
			{
				this.chosen = j.GetRoom();
				this.action = 1;
				this.result = DialogResult.Yes;
			}
			if (d == DialogResult.Cancel)
			{
				this.result = DialogResult.No;
			}
		}

        // Cancel the room change
		private void cancelButton_Click(object sender, EventArgs e)
		{
			this.result = DialogResult.Cancel;
		}

        // Returns the shape info for a specified room
        public string RoomInformation(int roomNum)
        {
            string s = "";
            List<Shape> shapes = this.rooms[roomNum].Shapes();
            for (int i = 0; i < shapes.Count; i++)
            {
                s += shapes[i].SendShape("Add", i, this.self) + "@";
            }
            return s;
        }

        // Returns all available rooms as a string
        public string RoomsToString()
        {
            string s = "";
            for (int i = 0; i < this.rooms.Count; i++)
            {
                s += this.rooms.ElementAt(i).Value.ToString() + "|";
            }
            return s;
        }

        // Converts the string of rooms and adds them to the dictionary 
        // of rooms for itself
        public void ReceivingRooms(string m, IPEndPoint ep)
        {
            string[] pieces = m.Split('|');
            for (int i = 1; i < pieces.Count() - 1; i++)
            {
                string[] info = pieces[i].Split('$');
                string ip = info[2];
                string[] ipInfo = ip.Split(':');
                IPAddress address = IPAddress.Parse(ipInfo[0]);
                IPEndPoint use = new IPEndPoint(address, 9050);
                this.rooms.Add(int.Parse(info[0]), new Room(int.Parse(info[0]), info[1], use));
            }
        }

        // Converts a new shape and performs the specified action
        // in the correct room
        public void ConvertShape(string message)
        {
            string[] info = message.Split('|');
            string sender = info[0];
            int drawRoom = int.Parse(info[1]);

            // Selection Outline Color
            Brush identifyer;
            if (sender != this.self)
            {
                UserJoined(sender);
                identifyer = FindUserColor(sender);
            }
            else
            {
                identifyer = Brushes.DarkGray;
            }

            string action = info[2];
            string type = info[3];

            // location
            string[] location = info[4].Split('=');
            string[] ex = location[1].Split(',');
            float x = float.Parse(ex[0].ToString());
            string[] why = location[2].Split('}');
            float y = float.Parse(why[0].ToString());

            //size
            string[] size = info[5].Split('=');
            string[] wit = size[1].Split(',');
            float width = float.Parse(wit[0].ToString());
            string[] hi = size[2].Split('}');
            float heigth = float.Parse(hi[0].ToString());

            // Colors
            ColorConverter read = new ColorConverter();
            Color outline = (Color)read.ConvertFromString(info[6]);
            Color fill = (Color)read.ConvertFromString(info[7]);

            // Angle
            float angle = float.Parse(info[8]);

            // Selected
            bool selected = !info[9].Contains("False");

            // Up
            bool up = !info[10].Contains("False");

            // Index
            int index = int.Parse(info[11]);

            // Create shape
            Shape[] shape = new Shape[1];
            switch (type)
            {
                case "Rect":
                    Rect r = new Rect(x, y, width, heigth, fill, outline, drawRoom, identifyer);
                    r.ReadShape(angle, selected, up);
                    shape[0] = r;
                    break;

                case "Square":
                    Square s = new Square(x, y, width, fill, outline, drawRoom, identifyer);
                    s.ReadShape(angle, selected, up);
                    shape[0] = s;
                    break;

                case "Oval":
                    Oval o = new Oval(x, y, width, heigth, fill, outline, drawRoom, identifyer);
                    o.ReadShape(angle, selected, up);
                    shape[0] = o;
                    break;

                case "Circle":
                    Circle c = new Circle(x, y, width, fill, outline, drawRoom, identifyer);
                    c.ReadShape(angle, selected, up);
                    shape[0] = c;
                    break;

                case "Triangle":
                    Triangle t = new Triangle(x, y, width, heigth, fill, outline, drawRoom, identifyer);
                    t.ReadShape(angle, selected, up);
                    shape[0] = t;
                    break;

                default:
                    return;
            }

            switch (action)
            {
                case "Add":
                    this.rooms[drawRoom].AddShape(shape[0]);
                    break;
                case "Transform":
                case "Rotate":
                case "Move":
                    this.rooms[drawRoom].Replace(index, shape[0]);
                    break;
                case "Delete":
                    this.rooms[drawRoom].Delete(index);
                    break;
                case "Select":
                    for (int i = 0; i < this.rooms[drawRoom].Shapes().Count; i++)
                    {
                        if (i == index)
                            this.rooms[drawRoom].Select(i, identifyer);
                        else
                            this.rooms[drawRoom].Deselect(i);
                    }
                    break;
                default:
                    break;
            }

        }

        // Determines if the user is new, if so adds a random color
        // as their selection color
        public void UserJoined(string name)
        {
            for (int i = 0; i < this.usernameColor.Keys.Count; i++)
            {
                if (name == this.usernameColor.ElementAt(i).Key)
                    return;
            }
            this.usernameColor.Add(name, PickRandomBrush());
        }

        // Finds the brush color for a specified username
        public Brush FindUserColor(string user)
        {
            for (int i = 0; i < this.usernameColor.Keys.Count; i++)
            {
                if (user == this.usernameColor.ElementAt(i).Key)
                    return this.usernameColor.ElementAt(i).Value;
            }
            return Brushes.DarkGray;
        }

        // Returns a random Brush color
        private Brush PickRandomBrush()
        {
            Brush result = Brushes.Transparent;
            Type brushesType = typeof(Brushes);
            PropertyInfo[] properties = brushesType.GetProperties();
            int random = rand.Next(properties.Length);
            result = (Brush)properties[random].GetValue(null, null);
            return result;
        }

        // The DialogResult changes to offline signal
        private void offlineButton_Click(object sender, EventArgs e)
        {
            this.result = DialogResult.Abort;
        }
	}
}
