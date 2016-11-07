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
	public partial class StartRoom : Form
	{
		private List<int> availableRooms = new List<int>();
		private int chosenRoom;
		private Dictionary<int, Room> available = new Dictionary<int, Room>();

		public StartRoom(Dictionary<int, Room> r)
		{
			InitializeComponent();
			if (r != null)
			{
                int[] nums = r.Keys.ToArray();
				for (int i = 0; i < nums.Length; i++)
				{
					this.availableRooms.Add(nums[i]);
				}
			}
			this.available = r;
		}

		public int GetRoom()
		{
			return this.chosenRoom;
		}

		private void createButton_Click(object sender, EventArgs e)
		{
			bool notAccepted = true;
			bool taken = false;
			while (notAccepted)
			{
                if (Int64.Parse(textBox1.Text) > Int32.MaxValue)
                {
                    labelForErrors.Text = "That number is too large, please select another.";
                    notAccepted = true;
                }
				if (this.available != null)
				{
					int[] keys = this.available.Keys.ToArray();
					for (int i = 0; i < keys.Count(); i++)
					{
						if (int.Parse(textBox1.Text) == keys[i])
						{
                            labelForErrors.Text = "That room number is taken, please select another.";
							taken = true;
						}
					}
				}
                if (!taken)
                {
                    this.chosenRoom = int.Parse(textBox1.Text);
                    notAccepted = false;
                }
			}
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			this.Hide();
		}

        private void StartRoom_Load(object sender, EventArgs e)
        {

        }
	}
}
