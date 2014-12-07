using System;
using System.Collections.Generic;
using System.Text;
using IronPython.Modules;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Enemies.Auxiliary
{
    public class AudioPlayer
    {
		private static Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();
	    private static ContentManager manager;

	    public static void Initialize(ContentManager mgr)
	    {
		    manager = mgr;
	    }

	    public static void PlaySound(string sound)
	    {
		    if (manager == null) return;
		    if (!sounds.ContainsKey(sound))
		    {
			    SoundEffect se = manager.Load<SoundEffect>("Sounds/" + sound + ".wav");
				sounds.Add(sound, se);
		    }

			if (sounds.ContainsKey(sound))
		    {
			    sounds[sound].Play();
		    }
	    } 
    }
}
