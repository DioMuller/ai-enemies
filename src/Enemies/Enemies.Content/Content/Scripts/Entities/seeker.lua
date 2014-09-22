script = {}

function script.Initialize()
	script.maxSpeed = 2
	script.updateTime = 0
end

function script.DoUpdate(delta)
	script.updateTime = script.updateTime - delta

	if script.updateTime <= 0 then
		local targetEntity = entity:GetNearestTarget()
		if targetEntity then
			script.target = vector.create(targetEntity.Position.X, targetEntity.Position.Y) 
		end

		script.updateTime = 120
	end

	if script.target then
		local movement = steering.seek(entity.Position, script.target, script.maxSpeed)
		
		entity:Move(movement.X, movement.Y)
	end
end

return script