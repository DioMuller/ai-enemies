from Enemies.Entities import BaseEntity
from Enemies.Entities import TypeTag
from math import sin, cos

class ScriptEntity(BaseEntity):
	totalTime = 0
	diff = 1000.0

	#def Initialize(self):
		# Nothing else to do.
	
	def DoUpdate(self, game_time):
		self.totalTime += game_time
		self.Move(sin(self.totalTime / self.diff), cos(self.totalTime / self.diff))
		super(ScriptEntity, self).DoUpdate(game_time)
