from System import TimeSpan
from Enemies.Entities import IAEntity

class ScriptEntity(IAEntity):
	def __init__(self, content):
		super(ScriptEntity, self).__init__(content)

		self.LoadSpritesheet("sprites/characters/main", 9, 13)

		frame_duration = 100
		
		self.AddAnimation("stopped_up", 0, 1, frame_duration, True, 0)
		self.AddAnimation("stopped_left", 1, 1, frame_duration, True, 0)
		self.AddAnimation("stopped_down", 2, 1, frame_duration, True, 0)
		self.AddAnimation("stopped_right", 3, 1, frame_duration, True, 0)
		
		self.AddAnimation("walking_up", 0, 8, frame_duration, True, 0)
		self.AddAnimation("walking_left", 1, 8, frame_duration, True, 0)
		self.AddAnimation("walking_down", 2, 8, frame_duration, True, 0)
		self.AddAnimation("walking_right", 3, 8, frame_duration, True, 0)
		
		self.AddAnimation("dying", 12, 6, frame_duration, False, 0)
		
		self.SetCurrentAnimation("stopped_down")
	
	def Update(self, game_time):
		super(ScriptEntity, self).Update(game_time)
