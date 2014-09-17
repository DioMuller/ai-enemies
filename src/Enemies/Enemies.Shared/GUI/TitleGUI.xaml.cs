using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enemies.GUI
{
    public delegate void OnButtonClickDelegate();

    public partial class TitleGUI
    {
        public OnButtonClickDelegate OnNewGame;
        public OnButtonClickDelegate OnLoadGame;
        public OnButtonClickDelegate OnQuitGame;

        public TitleGUI()
        {
            InitializeComponent();
        }

        public void NewGame_Click(object sender, EventArgs e)
        {
            if (OnNewGame != null) OnNewGame();
        }

        public void LoadGame_Click(object sender, EventArgs e)
        {
            if (OnLoadGame != null) OnLoadGame();
        }

        public void QuitGame_Click(object sender, EventArgs e)
        {
            if (OnQuitGame != null) OnQuitGame();
        }
    }
}
