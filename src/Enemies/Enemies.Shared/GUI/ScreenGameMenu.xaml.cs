using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enemies.GUI
{
	public partial class ScreenGameMenu
	{
        public event OnButtonClickDelegate AddPlayer_Clicked;
        public event OnButtonClickDelegate AddEnemy_Clicked;
        public event OnButtonClickDelegate AddObjective_Clicked;

        public ScreenGameMenu()
        {
            InitializeComponent();
        }

        public void AddPlayer_Click(object sender, EventArgs e)
        {
            if(AddPlayer_Clicked != null)
                AddPlayer_Clicked();
        }

        public void AddEnemy_Click(object sender, EventArgs e)
        {
            if(AddEnemy_Clicked != null)
                AddEnemy_Clicked();
        }

        public void AddObjective_Click(object sender, EventArgs e)
        {
            if(AddObjective_Clicked != null)
                AddObjective_Clicked();
        }
	}
}
