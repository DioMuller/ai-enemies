script = {}

function script.Initialize()
	script.mapData = entity:GetMapLayout()
	script.objective = entity:GetNearestTarget()

	if script.objective and script.mapData then
		script.path = pathfinding.findPath(script.mapData, entity.Position, script.objective.Position, 
		function(node, start, target)
			local dx = target.position.X - node.position.X;
            local dy = target.position.Y - node.position.Y;

            return pathfinding.directWeight * (dx * dx + dy * dy);
		end)
	end
end

function script.DoUpdate(delta)
	entities = entity:GetNearbyEnemies()

	if script.objective and script.mapData and not script.path then
		script.path = pathfinding.findPath(script.mapData, entity.Position, script.objective.Position, 
		function(node, start, target)
			local dx = target.position.X - node.position.X;
            local dy = target.position.Y - node.position.Y;

            return pathfinding.directWeight * (dx * dx + dy * dy);
		end)
		for value in array.foreach(script.path) do
			print("X = ", value.X, "Y = ", value.Y)
		end
	end
end

return script
