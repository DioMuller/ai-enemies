script = {}

function script.Initialize()
	script.maxSpeed = 2
	script.updateTime = 0
	script.panicDistance = 100
end

function script.DoUpdate(delta)
	script.updateTime = script.updateTime - delta

	if script.updateTime <= 0 then
		local enemies = entity:GetNearbyEnemies()
		

		if enemies then
			local targetEntity = array.first(enemies)
			if targetEntity then
				script.target = vector.create(targetEntity.Position.X, targetEntity.Position.Y) 
			end
		end

		local objective = entity:GetNearestTarget()
		if objective then
			script.objective = vector.create(objective.Position.X, objective.Position.Y)
		end

		script.updateTime = 120
	end

	if script.target then
		local movement = steering.flee(entity.Position, script.target, script.panicDistance, script.maxSpeed)
		
		if movement.X == 0 and movement.Y == 0  then
			movement = steering.seek(entity.Position, script.objective, script.maxSpeed)
		end
		entity:Move(movement.X, movement.Y)
	end
end

return script