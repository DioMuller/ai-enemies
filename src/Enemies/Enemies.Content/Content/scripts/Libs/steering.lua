steering = {}

-- Seek Steering Behavior
function steering.seek(agent, target, maxSpeed)
	local velocity = vector.subtract(target, agent.position)
	velocity = vector.normalize(velocity)
	return vector.multiply(velocity, maxSpeed)
end

-- Flee Steering Behavior
function steering.flee(agent, target, panicDistance, maxSpeed)
	local desiredVelocity = vector.subtract(agent.position, target)
	local distance = vector.size(desiredVelocity)

	if( distance < panicDistance ) then
		desiredVelocity = vector.normalize(desiredVelocity)
		desiredVelocity = vector.multiply(desiredVelocity, maxSpeed)

		return desiredVelocity
	end

	return vector.create( 0, 0 )
end

-- Wander Steering Behavior
function steering.wander(agent, target, direction, radius, distance, jitter, deltaTime, maxSpeed)
	local jitterTimeSlice = jitter * deltaTime * 9
	local temp = vector.create( (math.random(-1000.0,1000.0) / 1000.0) * jitterTimeSlice, (math.random(-1000.0,1000.0) / 1000.0) * jitterTimeSlice  )

	local newTarget = vector.add(target, temp)
	newTarget = vector.normalize(newTarget)
	newTarget = vector.multiply(newTarget, radius)

	local position = vector.add(agent, vector.multiply(direction, distance))
	position = vector.add(position, newTarget)

	local newDirection = vector.subtract(position, agent)
	newDirection = vector.normalize(newDirection)

	local movement = vector.multiply(newDirection, maxSpeed)
	return newTarget, newDirection, movement
end

return steering