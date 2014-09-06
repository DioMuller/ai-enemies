script = {}

function script.Initialize()
	entity:SetSpritesheet("Sprites/HeroSprite")
end

function script.DoUpdate(delta)
	entities = entity:GetNearbyEnemies()

	for value in lualib.foreach(entities) do
		-- Do Something
		print(string.format("Tag: ", value.Tag))
	end

	entity:Move(1,1)
end

return script