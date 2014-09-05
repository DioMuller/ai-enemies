script = {}

function script.Initialize()
	entity:SetSpritesheet("Sprites/HeroSprite")
end

function script.Update(delta)
	entities = entity:GetNeighbours()

	for value in lualib.foreach(entities) do
		-- Do Something
	end

	entity:Move(1,1)
end

return script