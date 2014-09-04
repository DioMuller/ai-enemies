script = {}

function script.Initialize()
	entity:SetSpritesheet("Sprites/HeroSprite")
end

function script.Update(delta)
	entity:Move(1,1)
end

return script