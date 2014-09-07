from Enemies.Entities import BaseEntity

class ScriptEntity(BaseEntity):
	def Initialize(self):
		self.SetSpritesheet("Sprites/HeroSprite")
		self.SetCurrentAnimation("stopped_down")
	
	def DoUpdate(self, game_time):
		super(ScriptEntity, self).DoUpdate(game_time)
