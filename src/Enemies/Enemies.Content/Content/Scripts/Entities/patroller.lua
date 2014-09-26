script = {}

script.threshold = 1.0
script.current = vector.create(0,0)
script.patrol = patrol.create()

function script.Initialize()
	patrol.add(script.patrol, vector.create(200,200))
	patrol.add(script.patrol, entity.Position)
	patrol.add(script.patrol, vector.create(400,200))
	patrol.add(script.patrol, entity.Position)
	patrol.add(script.patrol, vector.create(200,400))
	patrol.add(script.patrol, entity.Position)
	patrol.add(script.patrol,vector.create(400,400))

	script.current = patrol.getnext(script.patrol)
end

function script.DoUpdate(delta)
		entity:MoveTo(script.current.X, script.current.Y)

		if math.abs(entity.Position.X - script.current.X) < script.threshold and math.abs(entity.Position.Y - script.current.Y) < script.threshold then 
			script.current = patrol.getnext(script.patrol)
		end
end

return script