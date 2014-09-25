from Enemies.Entities import BaseEntity
from Enemies.Entities import TypeTag

class ScriptEntity(BaseEntity):
	totaltime = 0

	def Initialize(self):

		self.BroadcastMessage("I'm Alive")
	
	def DoUpdate(self, game_time):
		self.totaltime += game_time

		if self.totaltime > 1000:
			self.BroadcastMessage("I'm Still Alive.")
			self.totaltime = 0
		
		super(ScriptEntity, self).DoUpdate(game_time)