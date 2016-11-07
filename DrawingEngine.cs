using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net;
using SimpleNetwork;
using System.Reflection;

namespace ShapesAndInheritance
{
    // Performs actions on the windows form
	class DrawingEngine
	{
		// Key numbers
		public static Random rand = new Random();
		private const int maxRandDim = 200;

		// Network
		//public enum Connectivity { Offline, Online };
		UdpCommunicator com;
		private Queue<string> incoming = new Queue<string>();
		private List<string> incomingShapes = new List<string>();
		private List<string> outgoingShapes = new List<string>();
		private string username;
		private int room;
		Dictionary<string, Brush> usernameColor = new Dictionary<string, Brush>();

		// State
		private bool dragging;
		private bool transforming;
		private bool rotating;

		// Connectivity mode
		private Form1.Connectivity mode;
		

		// Characteristics
		private Color fill;
		private Color outline;
		private bool randCol;
		private bool randOut;
		private PointF mouseOffset;

		// Shapes
		private Shape selected;
		private int selectedIndex;
		private List<Shape> offlineShapes;
		private List<Shape> onlineShapes = new List<Shape>();
		

		public DrawingEngine(UdpCommunicator c, Form1.Connectivity connectivity)
		{
			this.selected = null;
			this.offlineShapes = new List<Shape>();
			this.com = c;
			this.mode = connectivity;
		}

        // Draws either the offline or online shapes
		public void Draw(Graphics g, Form1.Connectivity connectivity)
		{
			if (this.mode == Form1.Connectivity.Offline)
			{
				for (int i = 0; i < this.offlineShapes.Count; i++)
					this.offlineShapes[i].Draw(g);
			}
			else
			{
				this.DrawOnline(g);
			}
		}

        // Returns the online shapes list
		public List<Shape> Shapes()
		{
			return this.onlineShapes;
		}

        // Returns the list of shapes in action form (for online use)
		public string AllShapes()
		{
			string all = "";
			for (int i = 0; i < this.onlineShapes.Count; i++)
			{
				all += this.onlineShapes[i].SendShape("Add", i, this.username) + "@";
			}
			return all;
		}

        // Clears the shape list
		public void ClearShapes()
		{
			this.onlineShapes.Clear();
		}

        // Changes the connectivity
		public void EditConnectivity(Form1.Connectivity c)
		{
			this.mode = c;
		}

        // Changes the room
		public void EditRoom(int r)
		{
			this.room = r;
		}

        // Changes the username
		public void EditUsername(string name)
		{
			this.username = name;
		}

        // Returns the username
		public string GetUsername()
		{
			return this.username;
		}

        // Draws the online shapes
		private void DrawOnline(Graphics g)
		{
			if (this.onlineShapes.Count == 0)
				return;

			for (int i = 0; i < this.onlineShapes.Count; i++)
			{
				this.onlineShapes[i].Draw(g);
			}
		}

        // Converts a string into a shape
		public void ConvertShape(string message /*Graphics g*/)
		{		
			//string[] info = incoming.Dequeue().Split('|');
			string[] info = message.Split('|');
            string sender = info[0];
			int drawRoom = int.Parse(info[1]);
			if (drawRoom != this.room)
				return;

            // Selection Outline Color
            Brush identifyer;
            if (sender != this.username)
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

			// Perform given action
			switch (action)
			{
				case "Add":
					this.onlineShapes.Add(shape[0]);
					break;
				case "Transform":
				case "Rotate":
				case "Move":
					this.onlineShapes.Insert(index, shape[0]);
					this.onlineShapes.RemoveAt(index + 1);
                    this.onlineShapes[index].Select(FindUserColor(sender));
					break;
				case "Delete":
					this.onlineShapes.RemoveAt(index);
					break;
				case "Select":
					for (int i = 0; i < this.onlineShapes.Count; i++)
					{
						if (i == index)
							this.onlineShapes[i].Select(identifyer);
						else
							this.onlineShapes[i].Deselect();
					}
					break;
				default:
					break;					
			}
		}

        // Deselects all shapes except current selected one
        public void DeselectAll()
        {
            for (int i = 0; i < this.onlineShapes.Count; i++)
            {
                if (this.selected == null)
                    this.onlineShapes[i].Deselect();
                else if (!this.selected.Equals(this.onlineShapes[i]))
                    this.onlineShapes[i].Deselect();
            }
        }

        // Chekcs if a user joined and adds a new color for them if true
        public void UserJoined(string name)
        {
            for (int i = 0; i < this.usernameColor.Keys.Count; i++)
            {
                if (name == this.usernameColor.ElementAt(i).Key)
                    return;
            }
            this.usernameColor.Add(name, PickRandomBrush());
        }

        // Returns a certain user's Brush color
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

        // Handles a mouse click
        public void MouseDown(string tool, float x, float y)
		{
			SelectionMarker(x, y);
			Rotator(x, y);
			RandomizeColors();
			switch (tool)
			{
				case "Selection Tool":
					this.ChangeSelection(x, y);
					if (this.selected != null)
					{
						//BringToFront();
						this.mouseOffset = new PointF(x - this.selected.Bounds.X,
							y - this.selected.Bounds.Y);
					}
					if (!this.transforming && !this.rotating)
						this.dragging = (this.selected != null);
					break;

				case "Rectangle":
					AddShape(Shape.Type.Rect, x, y);
					break;

				case "Square":
					AddShape(Shape.Type.Square, x, y);
					break;

				case "Oval":
					AddShape(Shape.Type.Oval, x, y);
					break;

				case "Circle":
					AddShape(Shape.Type.Circle, x, y);
					break;

				case "Triangle":
					AddShape(Shape.Type.Triangle, x, y);
					break;

				default:
					break;
			}
		}

		// Adds a shape to the list of shapes
		private void AddShape(Shape.Type type, float x, float y)
		{
			this.selected = null; ;
			switch (type)
			{
				case Shape.Type.Rect:
					Rect r = new Rect(x, y, rand.Next(maxRandDim),
                        rand.Next(maxRandDim), this.fill, this.outline, this.room, Brushes.DarkGray);
					if (this.mode == Form1.Connectivity.Offline)
						this.offlineShapes.Add(r);
					else
						this.com.SendMessage(r.SendShape("Add", this.selectedIndex, this.username));
					break;
				case Shape.Type.Square:
					Square s = new Square(x, y, rand.Next(maxRandDim),
                        this.fill, this.outline, this.room, Brushes.DarkGray);
					if (this.mode == Form1.Connectivity.Offline)
						this.offlineShapes.Add(s);
					else
						this.com.SendMessage(s.SendShape("Add", this.selectedIndex, this.username));
					break;
				case Shape.Type.Oval:
					Oval o = new Oval(x, y, rand.Next(maxRandDim),
                        rand.Next(maxRandDim), this.fill, this.outline, this.room, Brushes.DarkGray);
					if (this.mode == Form1.Connectivity.Offline)
						this.offlineShapes.Add(o);
					else
                        this.com.SendMessage(o.SendShape("Add", this.selectedIndex, this.username));
					break;
				case Shape.Type.Circle:
					Circle c = new Circle(x, y, rand.Next(maxRandDim),
                        this.fill, this.outline, this.room, Brushes.DarkGray);
					if (this.mode == Form1.Connectivity.Offline)
						this.offlineShapes.Add(c);
					else
                        this.com.SendMessage(c.SendShape("Add", this.selectedIndex, this.username));
					break;
				case Shape.Type.Triangle:
					Triangle t = new Triangle(x, y, rand.Next(maxRandDim),
                        rand.Next(maxRandDim), this.fill, this.outline, this.room, Brushes.DarkGray);
					if (this.mode == Form1.Connectivity.Offline)
						this.offlineShapes.Add(t);
					else
                        this.com.SendMessage(t.SendShape("Add", this.selectedIndex, this.username));
					break;
				default:
					break;
			}
		}

        // Clones a shape
		public void Clone()
		{
            if (this.mode == Form1.Connectivity.Offline)
                this.offlineShapes.Add(this.selected.Clone());
            else
                this.com.SendMessage(this.selected.Clone().SendShape("Add", this.selectedIndex, this.username));
		}

        // Handles fill color switches
		public void HandleColors(string tool)
		{
			if (this.selected != null)
				this.selected.ChangeColor(ColorSwitch(tool), "fill");
			this.fill = ColorSwitch(tool);
			if (tool == "Random")
				this.randCol = true;
			else
				this.randCol = false;
		}

        // Handles outline color switches
		public void HandleOutline(string tool)
		{
			if (this.selected != null)
				this.selected.ChangeColor(ColorSwitch(tool), "outline");
			this.outline = ColorSwitch(tool);
			if (tool == "Random")
				this.randOut = true;
			else
				this.randOut = false;
		}

        // Returns the corresponding color or random
		private Color ColorSwitch(string c)
		{
			switch (c)
			{
				case "Transparent":
					return Color.Transparent;
				case "Black":
					return Color.Black;
				case "Blue":
					return Color.Blue;
				case "Red":
					return Color.Red;
				case "Yellow":
					return Color.Yellow;
				case "Orange":
					return Color.Orange;
				case "Green":
					return Color.Green;
				case "Purple":
					return Color.Purple;
				case "White":
					return Color.White;
				default:
					return Color.FromArgb(rand.Next(256), rand.Next(256),
						rand.Next(256));
			}
		}

        // Called after a color wheel use to change to the 
        // selected color
		public void ColorWheel(Color c, string s)
		{
			if (this.selected != null)
				this.selected.ChangeColor(c, s);
			if (s == "outline")
			{
				this.randOut = false;
				this.outline = c;
			}
			else
			{
				this.randCol = false;
				this.fill = c;
			}
		}

        // Randomizes the colors of a new shape if random is selected
		private void RandomizeColors()
		{
			if (this.randCol)
				this.fill = ColorSwitch("");
			if (this.randOut)
				this.outline = ColorSwitch("");
		}

        // Changes the current selected shape
		private void ChangeSelection(float x, float y)
		{
			if (this.mode == Form1.Connectivity.Offline)
			{
				Select(this.offlineShapes, x, y);
			}

			if (this.mode == Form1.Connectivity.Online)
			{
				int pos = Select(this.onlineShapes, x, y);
                           
                if (this.selected != null)
                {
                    this.onlineShapes[pos].Select(Brushes.DarkGray);
                    this.com.SendMessage(this.selected.SendShape("Select", pos, this.username));
                }
                else
                {
                    this.com.SendMessage("Deselected");
                }
			}
		}

        // Selects the shape in the given coordinates
		private int Select(List<Shape> shapes, float x, float y)
		{
			for (int i = shapes.Count - 1; i >= 0; i--)
			{
				Shape s = shapes[i];
				if (s.Contains(x, y))
				{
					// if so, remember which shape is now selected
					this.selectedIndex = i;
					this.selected = s;
					for (int a = 0; a < shapes.Count; a++)
					{
						if (a != selectedIndex)
							shapes[a].Deselect();
					}
					return i;
				}
				if (s.SelectionMarkerClicked(x, y) || s.RotatorClicked(x, y))
				{
					this.selected = s;
					return i;
				}
			}
			this.selected = null;
			return -1;
		}

        // Handles mouse movement
		public void MouseMove(PointF mouseLoc)
		{
			if (this.rotating)
			{
				this.selected.Rotate(mouseLoc.X, mouseLoc.Y);
				if (this.mode == Form1.Connectivity.Online)
                    this.com.SendMessage(this.selected.SendShape("Rotate", this.selectedIndex, this.username));
			}
			if (this.transforming)
			{
				this.selected.Transform(mouseLoc);
				if (this.mode == Form1.Connectivity.Online)
                    this.com.SendMessage(this.selected.SendShape("Transform", this.selectedIndex, this.username));
			}
			if (this.dragging)
			{
				System.Diagnostics.Debug.Assert(this.selected != null);

				this.selected.ChangeLocation(mouseLoc, this.mouseOffset);
				if (this.mode == Form1.Connectivity.Online)
                    this.com.SendMessage(this.selected.SendShape("Move", this.selectedIndex, this.username));
			}
		}

        // Ends actions when the mouse goes up
		public void MouseUp(PointF mouseLoc)
		{
			this.dragging = false;
			this.transforming = false;
			this.rotating = false;
		}

        // Brings shapes to the front
		public void BringToFront()
		{
			if (this.selected == null)
				return;
			for (int i = 0; i < this.offlineShapes.Count; i++)
			{
				if (this.offlineShapes[i] == this.selected)
				{
					this.offlineShapes.RemoveAt(i);
				}
			}
			this.offlineShapes.Insert(this.offlineShapes.Count, this.selected);
		}

        // Sends shapes to the back
		public void SendToBack()
		{
			if (this.selected == null)
				return;
			for (int i = 0; i < this.offlineShapes.Count; i++)
			{
				if (this.offlineShapes[i] == this.selected)
				{
					this.offlineShapes.RemoveAt(i);
				}
			}
			this.offlineShapes.Insert(0, this.selected);
		}

        // Brings shapes forward one
		public void BringForward()
		{
			if (this.selected == null)
				return;
			for (int i = 0; i < this.offlineShapes.Count - 1; i++)
			{
				if (this.offlineShapes[i] == this.selected)
				{
					Shape temp = this.offlineShapes[i + 1];
					this.offlineShapes.RemoveAt(i);
					this.offlineShapes.Insert(i, temp);
					this.offlineShapes.RemoveAt(i + 1);
					this.offlineShapes.Insert(i + 1, this.selected);
					break;
				}
			}
		}

        // Sends shapes backward one
		public void SendBackward()
		{
			if (this.selected == null)
				return;
			for (int i = this.offlineShapes.Count - 1; i > 0; i--)
			{
				if (this.offlineShapes[i] == this.selected)
				{
					Shape temp = this.offlineShapes[i - 1];
					this.offlineShapes.RemoveAt(i);
					this.offlineShapes.Insert(i, temp);
					this.offlineShapes.RemoveAt(i - 1);
					this.offlineShapes.Insert(i - 1, this.selected);
					break;
				}
			}
		}

        // Changes selected to right clicked shape
		public void MouseRightClick(float x, float y)
		{
			this.ChangeSelection(x, y);
		}

        // Deletes a shape
		public void Delete()
		{
			if (this.selected == null)
				return;
			this.offlineShapes.Remove(this.selected);
            this.com.SendMessage(this.selected.SendShape("Delete", this.selectedIndex, this.username));
			this.selected = null;		
		}

        // If a marker has been selected, start transform mode
		public void SelectionMarker(float x, float y)
		{
			if (this.selected != null && this.selected.SelectionMarkerClicked(x, y))
			{
				this.transforming = true;
				this.dragging = false;
				this.rotating = false;
			}
			else
				this.transforming = false;		
		}

        // If rotator has been selected, start rotate mode
		public void Rotator(float x, float y)
		{
			if (this.selected != null && this.selected.RotatorClicked(x, y))
			{
				this.rotating = true;
				this.dragging = false;
				this.transforming = false;
			}
			else
				this.rotating = false;			
		}
	}
}
