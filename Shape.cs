using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ShapesAndInheritance
{
	// "abstract" - you can't actually create a generic shape
	// you have to create a specific kind of shape (like Rect, etc.)
	public abstract class Shape : Resizable
	{
		const int cSet = 4;		
		float lastAngle = 0;
		protected Color fill;
		protected Color outline;
		private int room;
		protected int id;
        protected Brush identifyingColor = Brushes.DarkGray;

		public Shape(float x, float y, float w, float h, Color c, Color o, int r, Brush i/*, int i*/) : base(x, y, w, h)
		{
			this.room = r;
			//this.id = i;
			this.fill = c;
			this.outline = o;
		}

		// "abstract" means "inheriting classes must override this method"
		public virtual void Draw(Graphics g)
		{
			Matrix r = new Matrix();
			r.RotateAt(this.angle, this.Center);
			g.Transform = r;
			DrawShape(g);
			if (this.selected)
				DrawSelectionMarkers(g, Brushes.DarkGray);
		}

        // Draws the selection markers of the shape
		public virtual void DrawSelectionMarkers(Graphics g, Brush b)
		{
			// for now: draw a rectangle surrounding the shape
			Rectangle markers = Rectangle.Round(this.Bounds);

			using (Pen p = new Pen(this.identifyingColor, 4))
			{
				if (this.size.Width <= 2)
					g.FillRectangle(this.identifyingColor, new RectangleF(new PointF(this.loc.X - 3, this.loc.Y),
						new SizeF(5, this.size.Height)));
				if (this.size.Height <= 2)
					g.FillRectangle(this.identifyingColor, new RectangleF(new PointF(this.loc.X, this.loc.Y - 3),
						new SizeF(this.size.Width, 5)));
				g.DrawRectangle(p, markers);

				foreach (RectangleF point in this.Markers)
				{
					g.FillRectangles(Brushes.Black, this.Markers);
				}
				g.FillRectangle(this.identifyingColor, this.Rotator);
			}
		}

		public abstract void DrawShape(Graphics g);

		// Returns true if the given coordinates lie within the bounds of this shape
		public virtual bool Contains(float x, float y)
		{
			return HitTest(this.Bounds, x, y);
		}

        // Changes the coordinates of the shape
		public virtual void ChangeLocation(float x, float y, PointF mouseOffset)
		{
			this.loc.X = x - mouseOffset.X;
			this.loc.Y = y - mouseOffset.Y;
		}

        // Rotates a shape accordingly using the given coordinate
		public void Rotate(float x, float y)
		{
			float newY = this.Center.Y - y;
			float newX = x - this.Center.X;
			float radians = (float)Math.Atan2(newY, newX);
			
			this.angle = (float)(radians * (180 / Math.PI));			
			this.angle = 90 - this.angle;
		
			this.m.RotateAt(this.angle - this.lastAngle, this.Center);
			this.lastAngle = this.angle;
			this.p.Transform(this.m);
		}

        // Changes either the fill or outline color
		public void ChangeColor(Color c, string target)
		{
			if (target.ToLower() == "fill")
				this.fill = c;
			if (target.ToLower() == "outline")
				this.outline = c;
		}

        // Selects a shape with the correct color (user selection)
		public void Select(Brush b)
		{
            this.identifyingColor = b;
			this.selected = true;
		}

        // Deselects a shape
		public void Deselect()
		{
			this.selected = false;
		}

        // Returns an offset clone of this shape
		public Shape Clone()
		{
			Shape s = (Shape)this.MemberwiseClone();
			
			s.loc.X += 15;
			s.loc.Y += 5;
			return s;
		}

        // Returns the shape info as a string
		public string SendShape(string action, int index, string sender)
		{
			ColorConverter read = new ColorConverter();

			return sender + "|" + this.room + "|" + action + "|" + this.type +
                "|" + this.loc.ToString() + "|" + this.size.ToString() + "|" + 
                read.ConvertToString(this.outline) + "|" + read.ConvertToString(this.fill) +
                "|" + this.angle.ToString() + "|" + this.selected.ToString() + "|" +
                this.up.ToString() + "|" + index;
		}

        // Edits the angle, whether it's selected, and if it is up or down
		public void ReadShape(float a, bool s, bool u)
		{
			this.angle = a;
			this.selected = s;
			this.up = u;
		}

		#region Comments
		// Rectangle that encompasses the shape, with no space
		//public RectangleF Bounds
		//{
		//	get { return new RectangleF(this.loc, this.size); }
		//	set
		//	{
		//	}
		//}

		//public RectangleF[] Corners
		//{
		//	get
		//	{
		//		RectangleF[] temporary = new RectangleF[4];
		//		Rectangle temp = Rectangle.Round(this.Bounds);
		//		temp.Inflate(2, 2);
		//		PointF[] cornerP = new PointF[4];
		//		int s = cSet * 2;
		//		int offset = 2;
		//		temporary[0] = new RectangleF(temp.X - cSet + offset, temp.Y - cSet + offset, s, s);
		//		temporary[1] = new RectangleF(temp.X + temp.Width - cSet - offset, temp.Y - cSet + offset, s, s);
		//		temporary[2] = new RectangleF(temp.X - cSet + offset, temp.Y + temp.Height - cSet - offset, s, s);
		//		temporary[3] = new RectangleF(temp.X + temp.Width - cSet - offset, temp.Y + temp.Height - cSet - offset, s, s);
		//		return temporary;
		//	}
		//	set
		//	{				
		//	}
		//}

		//public virtual void DrawSelectionMarkers(Graphics g)
		//{
		//	// for now: draw a rectangle surrounding the shape
		//	Rectangle markers = Rectangle.Round(this.Bounds);
		//	//markers.Inflate(2, 2);
		//	using (Pen p = new Pen(Brushes.DarkGray, 4))
		//	{
		//		g.DrawRectangle(p, markers);
		//			foreach (RectangleF point in this.Corners)
		//			{
		//				g.FillRectangles(Brushes.Black, this.Corners);
		//			}
		//	}
		//}

		//public bool SelectionMarkerClicked(float x, float y)
		//{
		//	for (int i = 0; i < 4; i++)
		//	{
		//		if (this.Corners[i].Contains(x, y))
		//		{
		//			this.selectedCorner = i;
		//			return true;
		//		}
		//	}
		//	return false;
		//}

		//public virtual void Transform(float x, float y)//, int c)
		//{
		//	//this.selectedCorner = c;
		//	//if ((c == 0 || c == 2) && x > this.Corners[c].X + this.size.Width)
		//	//{
				
		//	//}
		//	for (int i = 0; i < 4; i++)
		//	{
		//		if (this.selectedCorner == 0)
		//		{
		//			CornerZeroClicked(x, y, this.selectedCorner);
		//		}
		//		if (this.selectedCorner == 1)
		//		{
		//			CornerOneClicked(x, y, this.selectedCorner);
		//		}
		//		if (this.selectedCorner == 2)
		//		{
		//			CornerTwoClicked(x, y, this.selectedCorner);
		//		}
		//		if (this.selectedCorner == 3)
		//		{
		//			CornerThreeClicked(x, y, this.selectedCorner);
		//		}
		//	}
		//	//if (cornerClicked == 3)
		//	//{
		//	//	difX = x - this.Corners[cornerClicked].X;
				
		//	//}
		//	//this.loc.X -= difX;
		//	//this.loc.Y -= difY;
		//	//this.size.Width += difX;
		//	//this.size.Height += difY;
		//}

		//private void CornerZeroClicked(float x, float y, int c)
		//{
		//	//float this.Corners[c].X = this.Corners[c].X;
		//	//float yCorner = this.Corners[c].Y;
		//	float difX = Math.Max(this.Corners[c].X, x) - Math.Min(this.Corners[c].X, x);
		//	float difY = Math.Max(this.Corners[c].Y, y) - Math.Min(this.Corners[c].Y, y);
		//	//difX = this.Corners[cornerClicked].X - x;
		//	//difY = this.Corners[cornerClicked].Y - y;
		//	if (this.Corners[c].X > x)
		//	{
		//		this.loc.X -= difX;
		//		this.size.Width += difX;	
		//	}
		//	else if (x > this.Corners[c].X)
		//	{
		//		this.loc.X += difX;
		//		this.size.Width -= difX;
		//		if (x >= this.loc.X + this.size.Width)
		//		{
		//			this.loc.X += this.size.Width;
		//			this.selectedCorner++;
		//			return;
		//		}
		//	}
		//	if (this.Corners[c].Y > y)
		//	{
		//		this.loc.Y -= difY;
		//		this.size.Height += difY;
		//	}
		//	else if (y > this.Corners[c].Y)
		//	{
		//		if (y >= this.loc.Y + this.size.Height)
		//		{
		//			this.loc.Y += this.size.Height;
		//			this.selectedCorner += 2;
		//			return;
		//		}
		//		this.loc.Y += difY;
		//		this.size.Height -= difY;
		//	}
		//}

		//private void CornerOneClicked(float x, float y, int c)
		//{
		//	float difX = Math.Max(this.Corners[c].X, x) - Math.Min(this.Corners[c].X, x);
		//	float difY = Math.Max(this.Corners[c].Y, y) - Math.Min(this.Corners[c].Y, y);
		//	//difX = this.Corners[cornerClicked].X - x;
		//	//difY = this.Corners[cornerClicked].Y - y;
		//	if (this.Corners[c].X > x)
		//	{
		//		this.size.Width -= difX;
		//		if (x <= this.loc.X)
		//		{
		//			this.loc.X -= this.size.Width;
		//			this.selectedCorner--;
		//			return;
		//		}
		//		//this.loc.X -= difX;
		//	}
		//	else if (x > this.Corners[c].X)
		//	{
		//		//this.loc.X += difX;
		//		this.size.Width += difX;
		//	}
		//	if (this.Corners[c].Y > y)
		//	{
		//		this.loc.Y -= difY;
		//		this.size.Height += difY;
		//	}
		//	else if (y > this.Corners[c].Y)
		//	{
		//		this.loc.Y += difY;
		//		this.size.Height -= difY;
		//		if (y >= this.loc.Y + this.size.Height)
		//		{
		//			this.loc.Y += this.size.Height;
		//			this.selectedCorner += 2;
		//			return;
		//		}

		//	}
		//}

		//private void CornerTwoClicked(float x, float y, int c)
		//{
		//	float difX = Math.Max(this.Corners[c].X, x) - Math.Min(this.Corners[c].X, x);
		//	float difY = Math.Max(this.Corners[c].Y, y) - Math.Min(this.Corners[c].Y, y);
		//	//difX = this.Corners[cornerClicked].X - x;
		//	//difY = this.Corners[cornerClicked].Y - y;
		//	if (this.Corners[c].X > x)
		//	{
		//		this.loc.X -= difX;
		//		this.size.Width += difX;
		//	}
		//	else if (x > this.Corners[c].X)
		//	{
		//		this.loc.X += difX;
		//		this.size.Width -= difX;
		//		if (x >= this.loc.X + this.size.Width)
		//		{
		//			this.loc.X += this.size.Width;
		//			this.selectedCorner++;
		//			return;
		//		}
		//	}
		//	if (this.Corners[c].Y > y)
		//	{
		//		this.size.Height -= difY;
		//		if (y <= this.loc.Y)
		//		{
		//			this.loc.Y -= this.size.Height;
		//			this.selectedCorner -= 2;
		//			return;
		//		}
		//		//this.loc.Y -= difY;
		//	}
		//	else if (y > this.Corners[c].Y)
		//	{
		//		//this.loc.Y += difY;
		//		this.size.Height += difY;
		//	}
		//}

		//private void CornerThreeClicked(float x, float y, int c)
		//{
		//	float difX = Math.Max(this.Corners[c].X, x) - Math.Min(this.Corners[c].X, x);
		//	float difY = Math.Max(this.Corners[c].Y, y) - Math.Min(this.Corners[c].Y, y);
		//	//difX = this.Corners[cornerClicked].X - x;
		//	//difY = this.Corners[cornerClicked].Y - y;
		//	if (this.Corners[c].X > x)
		//	{
		//		this.size.Width -= difX;
		//		if (x <= this.loc.X)
		//		{
		//			this.loc.X -= this.size.Width;
		//			this.selectedCorner--;
		//			return;
		//		}
		//		//this.loc.X -= difX;
		//	}
		//	else if (x > this.Corners[c].X)
		//	{
		//		//this.loc.X += difX;
		//		this.size.Width += difX;
		//	}
		//	if (this.Corners[c].Y > y)
		//	{
		//		this.size.Height -= difY;
		//		if (y <= this.loc.Y)
		//		{
		//			this.loc.Y -= this.size.Height;
		//			this.selectedCorner -= 2;
		//			return;
		//		}
		//		//this.loc.Y -= difY;

		//	}
		//	else if (y > this.Corners[c].Y)
		//	{
		//		//this.loc.Y += difY;
		//		this.size.Height += difY;
		//	}
		//}

		//public virtual Rect Clone()
		//{
		//	return new Rect(this.Bounds.X, this.Bounds.Y, this.Bounds.Width, this.Bounds.Height, this.fill);
		//}

		//public void Transform(PointF pt)/*, int c) */{ Transform(pt.X, pt.Y); }//, c); }
		#endregion
		public void ChangeLocation(PointF pt, PointF mouseOffset) { ChangeLocation(pt.X, pt.Y, mouseOffset); }

	}
}
