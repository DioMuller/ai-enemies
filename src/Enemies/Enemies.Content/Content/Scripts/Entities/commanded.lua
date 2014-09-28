script = {}

-- Current Action
script.current_action = "wait"
-- Commander Id
script.commander = 0
-- Circle openess. More = more open
script.circle_openess = 1000
-- Rotation speed. More = faster
script.rotation_speed = 0.001
-- Current state time.
script.current_time = 0
-- Threshold for proximity states.
script.threshold = 1.0
-- Current objective point
script.current_objective = nil
-- Patrol path
script.patrol_path = patrol.create()
-- Patrol distance
script.patrol_distance = 100

-- Polygon points
script.polygon_count = 5
-- Polygon path
script.polygon_path = patrol.create()
-- Star path
script.star_path = patrol.create()
-- Polygon/Star radius
script.poly_radius = 100
-- Pentagon/Star points
script.poly_points = {}


function script.Initialize()
	-- Sets origin point
	script.origin = entity.Position

	-- Creates patrol pattern
	patrol.add(script.patrol_path, script.origin)
	patrol.add(script.patrol_path, vector.create(script.origin.X, script.origin.Y - script.patrol_distance))
	patrol.add(script.patrol_path, vector.create(script.origin.X + script.patrol_distance, script.origin.Y - script.patrol_distance))
	patrol.add(script.patrol_path, vector.create(script.origin.X + script.patrol_distance, script.origin.Y))

	-- Creates polygon points
	script.poly_points = polygon.generate(script.polygon_count, script.origin, script.poly_radius)

	-- Creates polygon patrol points
	for i = 1, script.polygon_count do
		patrol.add(script.polygon_path, script.poly_points[i])
	end

	-- Creates Star patrol points
	for i = 1, script.polygon_count, 2 do
		patrol.add(script.star_path, script.poly_points[i])
	end
		for i = 2, script.polygon_count - 1, 2 do
		patrol.add(script.star_path, script.poly_points[i])
	end
end

function script.DoUpdate(delta)
	script.current_time = script.current_time + delta

	case.switch(script.current_action) 
	{
		["wait"] =
			function()
				if script.commander == 0 then script.current_action = "search" end
			end,
		["search"] = 
			function()
				if not script.commander == 0 then script.current_action = "wait" end
				entity:BroadcastMessage("commander", nil)
			end,
		["circles"] =
			function()
				entity:Move(math.sin(script.current_time / script.circle_openess), math.cos(script.current_time / script.circle_openess))
			end,
		["rotate"] =
			function()
				entity:SetRotation(script.current_time * script.rotation_speed)
			end,
		["look_left"] =
			function()
				entity:LookAt(-80,0)
			end,
		["look_right"] =
			function()
				entity:LookAt(80,0)
			end,
		["patrol"] =
			function()
				if not script.current_objective then script.current_objective = patrol.getnext(script.patrol_path) end
				entity:MoveTo(script.current_objective.X, script.current_objective.Y)
				if script.HasArrived() then
					script.current_objective = patrol.getnext(script.patrol_path)
				end
			end,
		["origin"] =
			function()
				if not script.current_objective then script.current_objective = script.origin end
				entity:MoveTo(script.current_objective.X, script.current_objective.Y)
				if script.HasArrived() then
					entity:SetRotation(0)
					script.current_action = "wait"
				end
			end,
		["polygon"] =
			function()
				if not script.current_objective then script.current_objective = patrol.getnext(script.polygon_path) end
				entity:MoveTo(script.current_objective.X, script.current_objective.Y)
				if script.HasArrived() then
					script.current_objective = patrol.getnext(script.polygon_path)
				end
			end,
		["star"] =
			function()
				if not script.current_objective then script.current_objective = patrol.getnext(script.star_path) end
				entity:MoveTo(script.current_objective.X, script.current_objective.Y)
				if script.HasArrived() then
					script.current_objective = patrol.getnext(script.star_path)
				end
			end,
		[case.Default] =
			function()
				print("Unknown Action: " .. script.current_action)
			end,
		[case.Nil] =
			function()
				print("Action is Nil")
			end
	}
end

function script.ReceiveMessage(message)
	if message.Text == "set commander" then
		if script.commander == 0 then
			print("Commander set: " .. message.Sender)
			script.commander = message.Sender
		end
		-- TODO: Something if the player already has a commander?
	elseif message.Sender == script.commander then
		print("Received action: " .. message.Text)
		script.current_objective = nil
		script.current_action = message.Text
		script.current_time = 0
	end
end

-- Script Helper functions
function script.HasArrived()
	return math.abs(entity.Position.X - script.current_objective.X) < script.threshold 
		and math.abs(entity.Position.Y - script.current_objective.Y) < script.threshold
end

return script