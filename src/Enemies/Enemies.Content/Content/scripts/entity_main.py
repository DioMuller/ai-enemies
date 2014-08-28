from System import TimeSpan
from spritesheet_entity import SpriteSheetEntity

class ScriptEntity(SpriteSheetEntity):
	def __init__(self, content):
		super(ScriptEntity,self).__init__(content, "sprites/characters/main", 9, 13)
		
		frame_duration = TimeSpan.FromMilliseconds(100)
		
		self.add_animation("stopped_up", 0, 1, frame_duration)
		self.add_animation("stopped_left", 1, 1, frame_duration)
		self.add_animation("stopped_down", 2, 1, frame_duration)
		self.add_animation("stopped_right", 3, 1, frame_duration)

		self.add_animation("walking_up", 0, 8, frame_duration, skipFrames = 1)
		self.add_animation("walking_left", 1, 8, frame_duration, skipFrames = 1)
		self.add_animation("walking_down", 2, 8, frame_duration, skipFrames = 1)
		self.add_animation("walking_right", 3, 8, frame_duration, skipFrames = 1)

		self.add_animation("dying", 12, 6, frame_duration, repeat = False)

		self.Sprite.PlayAnimation("stopped_down")
	
	def Update(self, game_time):
		super(ScriptEntity, self).Update(game_time)
