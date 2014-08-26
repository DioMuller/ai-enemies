from System import TimeSpan
from Microsoft.Xna.Framework.Graphics import Texture2D
from Jv.Games.Xna.Sprites import SpriteSheet
from Enemies.Entities import Entity

class ScriptEntity(Entity):
	def __init__(self, content):
		super(Entity,self).__init__()
		texture = content.Load[Texture2D]("sprites/characters/main")
		sprite_sheet = SpriteSheet(texture, 9, 13)

		def load_animation(*args, **kwargs):
			animation = sprite_sheet.GetAnimation(*args, **kwargs)
			self.Sprite.Add(animation)
		
		walk_frame_duration = TimeSpan.FromMilliseconds(100)
		
		load_animation("stopped_up", 0, 1, walk_frame_duration)
		load_animation("stopped_left", 1, 1, walk_frame_duration)
		load_animation("stopped_down", 2, 1, walk_frame_duration)
		load_animation("stopped_right", 3, 1, walk_frame_duration)

		load_animation("walking_up", 0, 8, walk_frame_duration, skipFrames = 1)
		load_animation("walking_left", 1, 8, walk_frame_duration, skipFrames = 1)
		load_animation("walking_down", 2, 8, walk_frame_duration, skipFrames = 1)
		load_animation("walking_right", 3, 8, walk_frame_duration, skipFrames = 1)

		load_animation("dying", 12, 6, walk_frame_duration, repeat = False)

		self.Sprite.PlayAnimation("stopped_down")
	
	def Update(self, game_time):
		super(ScriptEntity, self).Update(game_time)
