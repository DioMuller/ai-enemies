script = {}

function script.Initialize()
	script.mapData = entity:GetMapLayout()
	script.objective = entity:GetNearestTarget()

	script.path = astar.findPath(mapData, entity.Position, objective.Position)
end

function script.DoUpdate(delta)
	entities = entity:GetNearbyEnemies()

	for value in array.foreach(entities) do
		-- Do Something
	end

	entity:Move(1,1)
end

return script