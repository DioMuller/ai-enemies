script = {}

function script.Initialize()
	script.updateTime = 0
end

function script.DoUpdate(delta)
	script.updateTime = script.updateTime - delta

	if script.updateTime <= 0 then
		-- Shooting Targets
		local shootingTargets = entity:GetShootableEntities()
		if shootingTargets then
			local targetShoot = array.first(shootingTargets)
			if targetShoot then
				script.shootingTarget = vector.create(targetShoot.Position.X, targetShoot.Position.Y) 
			end
		end

		local objective = entity:GetNearestTarget()
		if objective then
			script.objective = vector.create(objective.Position.X, objective.Position.Y)
		end

		if script.shootingTarget then
			script.updateTime = 750
			entity:ShootAt(script.shootingTarget.X, script.shootingTarget.Y)
			script.shootingTarget = nil
		end
	end


end

return script