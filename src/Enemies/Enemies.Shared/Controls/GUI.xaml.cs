using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enemies.Controls
{
	public partial class GUI
	{
		public GUI ()
		{
			InitializeComponent ();
		}

        public void AddPlayer_Click(object sender, EventArgs e)
        {

        }

        public void AddEnemy_Click(object sender, EventArgs e)
        {

        }

        public void AddObjective_Click(object sender, EventArgs e)
        {
            AddObjective.RotationY++;
        }
	}
}
