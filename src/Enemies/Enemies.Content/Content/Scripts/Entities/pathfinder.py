from Enemies.Entities import BaseEntity
from Enemies.Entities import TypeTag

class ScriptEntity(BaseEntity):
	map = None
	map_info = None
	places_to_walk = None

	#def Initialize(self):
		#Nothing else to do.

	def DoUpdate(self, game_time):
		self.Move(-1,1)
		super(ScriptEntity, self).DoUpdate(game_time)

		self.CheckMapInfo()
		self.FindPath(self.CurrentPosition(), self.TargetPosition())
		self.WalkRoute()

	def CheckMapInfo(self):
		if self.map:
			return
		map_info = self.GetMapLayout()
		self.map = [[map_info.CanGo(x, y) for x in range(map_info.TileCount.X)] for y in range(map_info.TileCount.Y)]

	def CurrentPosition(self):
		position = self.Sprite.Position
		return self.TilePosition(position.X, position.Y)

	def TargetPosition(self):
		target = self.GetNearestTarget()
		position = target.Position
		return self.TilePosition(position.X, position.Y)

	def TilePosition(self, x, y):
		tile_size = self.map_info.TileSize;
		sx = x / tile_size.X
		sy = y / tile_size.Y
		return sx, sy

	def FindPath(self, start, end):
		pass

	def WalkRoute(self):
		pass
