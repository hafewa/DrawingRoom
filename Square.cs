using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ShapesAndInheritance
{
	class Square : Rect
	{
		// when we initialize an object, have to first initialize the object it's based on
		public Square(float x, float y, float s, Color c, Color o, int r, Brush i) : base(x, y, s, s, c, o, r, i)
		{
			// if I had additional initializing for my Square, I'd put the code here
			this.type = Type.Square;
		}
	}
}
