using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ShapesAndInheritance
{
	class Circle : Oval
	{
		public Circle(float x, float y, float s, Color c, Color o, int r, Brush i) : base(x, y, s, s, c, o, r, i)
		{
			this.type = Type.Circle;
		}
	}
}
