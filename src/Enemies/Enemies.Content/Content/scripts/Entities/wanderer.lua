script = {}

function script.Initialize()
	script.target = vector:create( 0, 0 )
	script.direction = vector:create( 1, 1 )
	script.movement = vector:create( 1, 1 )

	script.jitter= 1.25
	script.distance = 1
	script.radius = 100
	script.maxSpeed = 10
	entity:SetSpritesheet("Sprites/HeroSprite")
end

function script.DoUpdate(delta)
	--script.target, script.direction, script.movement = steering:wander(entity.Position, script.target, script.direction, script.radius, script.distance, script.jitter, delta, script.maxSpeed)

	--entity:Move(movement.X, movement.Y)
end

return script