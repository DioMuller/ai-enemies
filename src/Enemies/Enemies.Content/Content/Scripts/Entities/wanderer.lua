script = {}

function script.Initialize()
	script.target = vector.create( 1, 1 )
	script.direction = vector.create( 1, 1 )
	script.movement = vector.create( 1, 1 )

	script.jitter= 3.5
	script.distance = 150
	script.radius = 50
	script.maxSpeed = 10
end

function script.DoUpdate(delta)
	local newTarget, newDirection, movement = steering.wander(entity.Position, script.target, script.direction, script.radius, script.distance, script.jitter, delta, script.maxSpeed)
	script.target = newTarget
	script.direction = newDirection

	entity:Move(movement.X, movement.Y)
end

return script