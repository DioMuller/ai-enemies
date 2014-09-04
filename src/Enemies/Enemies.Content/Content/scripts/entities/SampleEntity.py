from Enemies.Entities import BaseEntity

class ScriptEntity(BaseEntity):
	def Initialize(self):
		self.SetSpritesheet("Sprites/HeroSprite")		
		self.SetCurrentAnimation("stopped_down")
	
	def Update(self, game_time):
		super(ScriptEntity, self).Update(game_time)
