using Enemies.Scripting;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace Enemies.GUI
{
    public partial class ScreenGameMain
    {
        public event OnButtonClickDelegate PlayPause_Clicked;
        public event GeneratePageDelegate AddEntity_Page;
        public event OnButtonClickDelegate BuildMode_Clicked;

        public ScreenGameMain()
        {
            InitializeComponent();
        }

        public void PlayPause_Click(object sender, EventArgs e)
        {
            if (PlayPause_Clicked != null)
                PlayPause_Clicked();
        }

        public void AddEntity_Click(object sender, EventArgs e)
        {
            if (AddEntity_Page != null)
            {
                var page = AddEntity_Page();
                Navigation.PushAsync(page);
            }   
        }

        public void BuildMode_Click(object sender, EventArgs e)
        {
            if (BuildMode_Clicked != null)
                BuildMode_Clicked();
        }
	}
}
