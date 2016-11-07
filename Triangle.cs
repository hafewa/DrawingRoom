using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ShapesAndInheritance
{
	class Triangle : Shape
	{
		public Triangle(float x, float y, float w, float h, Color c, Color o, int r, Brush i) : base(x, y, w, h, c, o, r, i)
		{
			//PointF[] points = new PointF[3];
			this.type = Type.Triangle;
		}

        // The points of the triangle
		public PointF[] Points
		{
			get
			{
				PointF[] temp = new PointF[3];
				if (this.up)
				{
					temp[0] = new PointF(this.loc.X + (this.size.Width / 2), this.Bounds.Y);
					temp[1] = new PointF(this.loc.X, this.loc.Y + this.size.Height);
					temp[2] = new PointF(this.loc.X + this.size.Width, this.loc.Y + this.size.Height);
					return temp;
				}
				else
				{
					temp[0] = new PointF(this.loc.X, this.loc.Y);
					temp[1] = new PointF(this.loc.X + this.size.Width, this.loc.Y);
					temp[2] = new PointF(this.loc.X + (this.size.Width / 2), this.loc.Y + this.size.Height);
					return temp;
				}
			}
			set
			{
			}
		}

        // Draws the triangle
		public override void DrawShape(Graphics g)
		{
			using (Brush b = new SolidBrush(this.fill))
			{
				g.FillPolygon(b, this.Points);
			}
			using (Pen p = new Pen(this.outline))
			{
				Point point = new Point((int)this.loc.X, (int)this.loc.Y);
				Size size = new Size((int)this.size.Width, (int)this.size.Height);
				g.DrawPolygon(p, this.Points);
			}
		}

        // Changes the location of the triangle
		public override void ChangeLocation(float x, float y, PointF mouseOffset)
		{
			this.loc.X = x - mouseOffset.X;
			this.loc.Y = y - mouseOffset.Y;
		}
	}
}
