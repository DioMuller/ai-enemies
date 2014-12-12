using System;
using System.Collections.Generic;
using System.Text;
using IronPython.Modules;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Enemies.Auxiliary
{
    public class AudioPlayer
    {
		private static Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();

		private static string _currentBGM = String.Empty;
		private static Dictionary<string, Song> _bgms = new Dictionary<string, Song>();

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

		public static void PlayBGM(string title)
		{
			if (title != _currentBGM)
			{
				Song song = null;
				_bgms.TryGetValue(title, out song);

				if (song == null)
				{
					song = manager.Load<Song>("BGM/" + title + ".wav");
					_bgms.Add(title, song);
				}

				_currentBGM = title;
				MediaPlayer.Play(song);
			}
		} 
    }
}
