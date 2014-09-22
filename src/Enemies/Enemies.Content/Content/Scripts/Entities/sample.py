from Enemies.Entities import BaseEntity
from Enemies.Entities import TypeTag

class ScriptEntity(BaseEntity):
	#def Initialize(self):
		#Nothing else to do.
	
	def DoUpdate(self, game_time):
		self.Move(1,1)
		super(ScriptEntity, self).DoUpdate(game_time)