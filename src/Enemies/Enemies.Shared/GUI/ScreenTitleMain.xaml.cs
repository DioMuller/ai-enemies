﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enemies.GUI
{
    public partial class ScreenTitleMain
    {
        public OnButtonClickDelegate OnNewGame;
		public OnButtonClickDelegate OnSandbox;
        public OnButtonClickDelegate OnLoadGame;
        public OnButtonClickDelegate OnQuitGame;

        public ScreenTitleMain()
        {
            InitializeComponent();
        }

        public void NewGame_Click(object sender, EventArgs e)
        {
            if (OnNewGame != null) OnNewGame();
        }

		public void Sandbox_Click(object sender, EventArgs e)
		{
			if (OnSandbox != null) OnSandbox();
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
