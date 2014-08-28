from Microsoft.Xna.Framework.Graphics import Texture2D
from System.IO import Path
from Jv.Games.Xna.Sprites import SpriteSheet
from Enemies.Entities import Entity

class SpriteSheetEntity(Entity):
	def __init__(self, content, spritesheet_path, columns, rows):
		super(SpriteSheetEntity, self).__init__()
		texture = content.Load[Texture2D](spritesheet_path)
		self.sprite_sheet = SpriteSheet(texture, columns, rows)

	def add_animation(self, *args, **kwargs):
		animation = self.sprite_sheet.GetAnimation(*args, **kwargs)
		self.Sprite.Add(animation)
