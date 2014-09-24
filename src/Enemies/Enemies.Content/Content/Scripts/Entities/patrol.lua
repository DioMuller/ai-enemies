script = {}

script.threshold = 1.0

function script.Initialize()
	script.points = { vector.create(200,200), entity.Position, vector.create(400,200),
					  entity.Position, vector.create(200,400), entity.Position, vector.create(400,400) }
	script.current = 1
end

function script.DoUpdate(delta)
	local point = script.points[script.current]
	entity:MoveTo(point.X, point.Y)

	if math.abs(entity.Position.X - point.X) < script.threshold and math.abs(entity.Position.Y - point.Y) < script.threshold then 
		script.current = script.current + 1
		if not script.points[script.current] then script.current = 1 end
	end
end

return script