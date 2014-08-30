﻿function Initialize()
	LoadSpritesheet("sprites/characters/main", 9, 13)

	AddAnimation("stopped_up", 0, 1, 100, true, 0)
	AddAnimation("stopped_left", 1, 1, 100, true, 0)
	AddAnimation("stopped_down", 2, 1, 100, true, 0)
	AddAnimation("stopped_right", 3, 1, 100, true, 0)

	AddAnimation("walking_up", 0, 8, 100, true, 0)
	AddAnimation("walking_left", 1, 8, 100, true, 0)
	AddAnimation("walking_down", 2, 8, 100, true, 0)
	AddAnimation("walking_right", 3, 8, 100, true, 0)
end

function Update(delta)
end