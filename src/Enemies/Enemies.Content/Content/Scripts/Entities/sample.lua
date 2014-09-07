script = {}

function script.Initialize()
	entity:SetSpritesheet("Sprites/HeroSprite")
end

function script.DoUpdate(delta)
	entities = entity:GetNearbyEnemies()

	for value in array.foreach(entities) do
		-- Do Something
	end

	entity:Move(1,1)
end

return script