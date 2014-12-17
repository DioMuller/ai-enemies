using Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Enemies.Scripting
{
    public class ScriptEntityDescription
    {
        public string DisplayName { get; set; }
        public string File { get; set; }
        public IScriptEntityFactory Factory { get; set; }
        public bool IsPlayerCreated { get; set; }
        public Color HighlightColor
        {
            get { return IsPlayerCreated ? Color.Aqua : Color.Yellow;  }
        }
    }
}
