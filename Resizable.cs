using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ShapesAndInheritance
{
	public class Resizable
	{
        // Selection marker offset
		const int cSet = 5;
        
        // Shape Info
		public enum Type { Rect, Square, Oval, Circle, Triangle };
		protected Type type;
		protected bool up = true;
		protected float angle = 0;
		protected PointF loc;
		protected PointF originalLoc;
		protected SizeF size;
		protected bool firstRotateClick = true;
		protected int selectedMarker = -1;
		protected GraphicsPath p = new GraphicsPath();
		protected Matrix m = new Matrix();
		protected bool selected = false;

        // The bounds of this shape
		public RectangleF Bounds
		{
			get { return new RectangleF(this.loc, this.size); }
			set { }
		}

        // The array of markers of this shape
		public RectangleF[] Markers
		{
			get
			{
				RectangleF[] temporary = new RectangleF[8];
				Rectangle temp = Rectangle.Round(this.Bounds);
				temp.Inflate(2, 2);
				PointF[] cornerP = new PointF[4];
				int s = cSet * 2;
				int offset = 2;
				temporary[0] = new RectangleF(temp.X - cSet + offset, temp.Y - cSet + offset, s, s);
				temporary[1] = new RectangleF(temp.X + temp.Width - cSet - offset, temp.Y - cSet + offset, s, s);
				temporary[2] = new RectangleF(temp.X - cSet + offset, temp.Y + temp.Height - cSet - offset, s, s);
				temporary[3] = new RectangleF(temp.X + temp.Width - cSet - offset, temp.Y + temp.Height - cSet - offset, s, s);
				temporary[4] = new RectangleF(temp.X - cSet + offset + temp.Width / 2 - 2, temp.Y - cSet + offset, s, s);
				temporary[5] = new RectangleF(temp.X - cSet + offset, temp.Y - cSet + offset + temp.Height / 2 - 2, s, s);
				temporary[6] = new RectangleF(temp.X + temp.Width - cSet - offset, temp.Y - cSet + offset + temp.Height / 2 - 2, s, s);
				temporary[7] = new RectangleF(temp.X - cSet + offset + temp.Width / 2 - 2, temp.Y + temp.Height - cSet - offset, s, s);
				return temporary;
			}
			set { }
		}

        // The center of this shape
		public PointF Center
		{
			get
			{
				return new PointF(this.loc.X + this.size.Width / 2,
					this.loc.Y + this.size.Height / 2);
			}
			set { }
		}

        // The rotator of this shape
		public RectangleF Rotator
		{
			get
			{
				return new RectangleF(this.Bounds.X + this.size.Width / 2 - 5.5f, this.Bounds.Y - 25, 10, 10); 
			}
			set { }
		}

        // The radius of this shape
		public float Radius
		{
			get
			{
				return this.Center.X - this.Bounds.X;
			}
			set { }
		}

        // Constructor
		public Resizable(float x, float y, float w, float h)
		{
			this.loc = new PointF(x, y);
			this.size = new SizeF(w, h);
			this.originalLoc = new PointF(this.loc.X + this.size.Width / 2, this.loc.Y - 25);
		}		

        // Determines if a selection marker has been clicked
		public bool SelectionMarkerClicked(float x, float y)
		{			
			for (int i = 0; i < 8; i++)
			{
				if (HitTest(this.Markers[i], x, y))
				{
					this.selectedMarker = i;
					return true;
				}
			}
			return false;
		}

        // Determines if the click is within the shape
		public bool HitTest(RectangleF r, float x, float y)
		{
			GraphicsPath path = new GraphicsPath();
			Matrix matrix = new Matrix();
			matrix.RotateAt(this.angle, this.Center);
			path.AddRectangle(r);
			path.Transform(matrix);
			this.selected = path.IsVisible(x, y);
			return path.IsVisible(x, y);
		}

        // Determines if the rotator has been clicked
		public bool RotatorClicked(float x, float y)
		{
			return HitTest(this.Rotator, x, y);
		}

        // Marker handler
		private void MarkerZeroClicked(PointF p, float x, float y)
		{
			if (MoveLeftSideX(x))
				this.selectedMarker = 1;

			if (MoveTopY(y))
				this.selectedMarker = 2;
		}

        // Marker handler
		private void MarkerOneClicked(PointF p, float x, float y)
		{
			if (MoveRightSideX(x))
				this.selectedMarker = 0;

			if (MoveTopY(y))
				this.selectedMarker = 3;
		}

        // Marker handler
		private void MarkerTwoClicked(PointF p, float x, float y)
		{
			if (MoveLeftSideX(x))
				this.selectedMarker = 3;

			if (MoveBottomY(y))
				this.selectedMarker = 0;
		}

        // Marker handler
		private void MarkerThreeClicked(PointF p, float x, float y)
		{
			if (MoveRightSideX(x))
				this.selectedMarker = 2;

			if (MoveBottomY(y))
				this.selectedMarker = 1;
		}

        // Marker handler
		private void MarkerFourClicked(PointF p, float x, float y)
		{
			if (MoveTopY(y))
				this.selectedMarker = 7;
		}

        // Marker handler
		private void MarkerFiveClicked(PointF p, float x, float y)
		{
			if (MoveLeftSideX(x))
				this.selectedMarker = 6;
		}

        // Marker handler
		private void MarkerSixClicked(PointF p, float x, float y)
		{
			if (MoveRightSideX(x))
				this.selectedMarker = 5;
		}

        // Marker handler
		private void MarkerSevenClicked(PointF p, float x, float y)
		{
			if (MoveBottomY(y))
				this.selectedMarker = 4;
		}

        // Moves the left side of the shape
		private bool MoveLeftSideX(float x)
		{
			float difX = Math.Max(this.Markers[this.selectedMarker].X, x) -
				Math.Min(this.Markers[this.selectedMarker].X, x);
			if (this.Markers[this.selectedMarker].X > x)
			{
				this.loc.X -= difX;
				this.size.Width += difX;
			}
			else if (x > this.Markers[this.selectedMarker].X)
			{
				this.loc.X += difX;
				this.size.Width -= difX;
				if (x >= this.loc.X + this.size.Width)
				{
					this.loc.X += this.size.Width;
					return true;
				}
			}
			return false;
		}

        // Moves the right side of the shape
		private bool MoveRightSideX(float x)
		{
			float difX = Math.Max(this.Markers[this.selectedMarker].X, x) -
				Math.Min(this.Markers[this.selectedMarker].X, x);
			if (this.Markers[this.selectedMarker].X > x)
			{
				this.size.Width -= difX;
				if (x <= this.loc.X)
				{
					this.loc.X -= this.size.Width;
					return true;
				}
			}
			else if (x > this.Markers[this.selectedMarker].X)
			{
				this.size.Width += difX;
			}
			return false;
		}

        // Moves the top of the shape
		private bool MoveTopY(float y)
		{
			float difY = Math.Max(this.Markers[this.selectedMarker].Y, y) -
				Math.Min(this.Markers[this.selectedMarker].Y, y);
			if (this.Markers[this.selectedMarker].Y > y)
			{
				this.loc.Y -= difY;
				this.size.Height += difY;
			}
			else if (y > this.Markers[this.selectedMarker].Y)
			{
				if (y >= this.loc.Y + this.size.Height)
				{
					this.up = !this.up;
					this.loc.Y += this.size.Height;
					return true;
				}
				this.loc.Y += difY;
				this.size.Height -= difY;
			}
			return false;
		}

        // Moves the bottom of the shape
		private bool MoveBottomY(float y)
		{
			float difY = Math.Max(this.Markers[this.selectedMarker].Y, y) -
				Math.Min(this.Markers[this.selectedMarker].Y, y);
			if (this.Markers[this.selectedMarker].Y > y)
			{
				this.size.Height -= difY;
				if (y <= this.loc.Y)
				{
					this.up = !this.up;
					this.loc.Y -= this.size.Height;
					return true;
				}
			}
			else if (y > this.Markers[this.selectedMarker].Y)
			{
				this.size.Height += difY;
			}
			return false;
		}

        // Transforms the shape accordingly with the given point
		public void Transform(PointF pt)
		{
			// Prevents size change while rotated
			if (Math.Abs(this.angle) > 4)
				return;

			if (this.selectedMarker == 0)
			{
				MarkerZeroClicked(pt, pt.X, pt.Y);
			}
			if (this.selectedMarker == 1)
			{
				MarkerOneClicked(pt, pt.X, pt.Y);
			}
			if (this.selectedMarker == 2)
			{
				MarkerTwoClicked(pt, pt.X, pt.Y);
			}
			if (this.selectedMarker == 3)
			{
				MarkerThreeClicked(pt, pt.X, pt.Y);
			}
			if (this.selectedMarker == 4)
			{
				MarkerFourClicked(pt, pt.X, pt.Y);
			}
			if (this.selectedMarker == 5)
			{
				MarkerFiveClicked(pt, pt.X, pt.Y);
			}
			if (this.selectedMarker == 6)
			{
				MarkerSixClicked(pt, pt.X, pt.Y);
			}
			if (this.selectedMarker == 7)
			{
				MarkerSevenClicked(pt, pt.X, pt.Y);
			}
			if (this.size.Height != this.size.Width)
			{
				if (this.type == Type.Circle)
					this.type = Type.Oval;
				if (this.type == Type.Square)
					this.type = Type.Rect;
			}
		}
	}
}
