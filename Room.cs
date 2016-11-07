using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Drawing;

namespace ShapesAndInheritance
{
    // A Room object holds information regarding a room
    // that users can draw in
	public class Room
	{
        // Room Information
		protected List<Shape> shapes = new List<Shape>();
		int number;
		string owner;

        // Network
        IPEndPoint ownerAddress;

		public Room(int n, string o, IPEndPoint e)
		{
			this.number = n;
			this.owner = o;
            this.ownerAddress = e;
		}

        // Room creator
		public string Owner
		{
			get
			{
				return this.owner;
			}
			set
			{
				this.owner = value;
			}
		}

        // EP of the Room creator
        public IPEndPoint IP()
        {
            return this.ownerAddress;
        }

        // Replaces a shape with the given one
        public void Replace(int index, Shape s)
        {
            this.shapes.Insert(index, s);
            this.shapes.RemoveAt(index + 1);
        }

        // Deletes a shape at the given index
        public void Delete(int index)
        {
            this.shapes.RemoveAt(index);
        }

        // Selects the specified shape
        public void Select(int index, Brush b)
        {
            this.shapes[index].Select(b);
        }

        // Deselects the specified shape
        public void Deselect(int index)
        {
            this.shapes[index].Deselect();
        }

        // Returns the shapes in the room
        public List<Shape> Shapes()
        {
            return this.shapes;
        }

        // Adds a shape to the room
		public void AddShape(Shape s)
		{
			this.shapes.Add(s);
		}

        // Adds a list of shapes to the room
		public void AddListOfShapes(List<Shape> s)
		{
			for (int i = 0; i < s.Count; i++)
			{
				this.shapes.Add(s[i]);
			}
		}

        // Deletes all shapes in the room
		public void ClearShapes()
		{
			this.shapes.Clear();
		}

        // Returns the number of the room
		public int ToInt()
		{
			return this.number;
		}

        // Returns the room info as a string
        public override string ToString()
        {
            return this.number + "$" + this.owner + "$" + this.ownerAddress;
        }
	}
}
