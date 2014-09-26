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

function script.Initialize()
	script.origin = entity.Position
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
				print(script.current_action)
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
		["star"] =
			function()
				print(script.current_action)
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
	if script.commander == 0 and message.Text == "set commander" then
		print("Commander set: " .. message.Sender)
		script.commander = message.Sender
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