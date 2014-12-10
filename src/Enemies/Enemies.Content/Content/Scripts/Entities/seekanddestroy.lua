script = {}

function script.Initialize()
	script.maxSpeed = 2
	script.updateTime = 0
	script.shootTime = 0
	script.panicDistance = 100
end

function script.DoUpdate(delta)
	script.updateTime = script.updateTime - delta
	script.shootTime = script.shootTime - delta

	if script.updateTime <= 0 then
		local enemies = entity:GetNearbyEnemies()
		

		if enemies then
			local targetEntity = array.first(enemies)

			if targetEntity then
				if entity:CanReach(targetEntity.Position.X, targetEntity.Position.Y) then				
					script.target = vector.create(targetEntity.Position.X, targetEntity.Position.Y) 
				end
			end
		end

		local objective = entity:GetNearestTarget()
		if objective then
			script.objective = vector.create(objective.Position.X, objective.Position.Y)
		end

		script.updateTime = 100
	end

	if script.objective then
		local movement = steering.seek(entity.Position, script.objective, script.maxSpeed)
		
		if script.shootTime <= 0 and script.target then
			script.shootTime = 500
			entity:ShootAt(script.target.X, script.target.Y)
		end
		entity:Move(movement.X, movement.Y)
	end
end

return script