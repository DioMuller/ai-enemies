script = {}

function script.Initialize()
	script.mapData = entity:GetMapLayout()
	script.objective = entity:GetNearestTarget()

	if script.objective and script.mapData then
		script.path = astar.findPath(script.mapData, entity.Position, script.objective.Position)
	end
end

function script.DoUpdate(delta)
	entities = entity:GetNearbyEnemies()

	if script.objective and script.mapData and not script.path then
		script.path = astar.findPath(script.mapData, entity.Position, script.objective.Position)
		for value in array.foreach(script.path) do
			print("X = ", value.X, "Y = ", value.Y)
		end
	end
end

return script
