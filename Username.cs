using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShapesAndInheritance
{
	public partial class Username : Form
	{
		string name;
		public Username()
		{
			InitializeComponent();
		}

		private void doneButton_Click(object sender, EventArgs e)
		{
			this.name = textBox1.Text;
		}

		public string GetName()
		{
			return this.name;
		}


	}
}
