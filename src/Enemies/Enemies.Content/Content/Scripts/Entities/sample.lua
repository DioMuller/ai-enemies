script = {}

function script.Initialize()
	if entity.Tag == tags.player then
		entity:SetSpritesheet("Sprites/Human")
	else
		entity:SetSpritesheet("Sprites/Zombie")
	end
end

function script.DoUpdate(delta)
	entities = entity:GetNearbyEnemies()

	for value in array.foreach(entities) do
		-- Do Something
	end

	entity:Move(1,1)
end

return script