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

	vector.print("Position", entity.Position )
	vector.print("Target", script.target )
	vector.print("Direction", script.direction )
	vector.print("Movement", script.movement )
end

function script.DoUpdate(delta)
	local newTarget, newDirection, movement = steering:wander(entity.Position, script.target, script.direction, script.radius, script.distance, script.jitter, delta, script.maxSpeed)
	script.target = newTarget
	script.direction = newDirection

	--entity:Move(movement.X, movement.Y)
end

return script