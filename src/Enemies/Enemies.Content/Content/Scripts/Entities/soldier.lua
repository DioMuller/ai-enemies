

script = {}

script.current_action = "wait"
script.commander = 0

function script.Initialize()

end

function script.DoUpdate(delta)
	case.switch(current_action) 
	{
		["wait"] =
			function()
				if script.commander == 0 then script.current_action = "search" end
			end,
		["search"] = 
			function()
				entity:BroadcastMessage("commander", nil)
			end,
		["circles"] =
			function()
				print(current_action)
			end,
		["rotate"] =
			function()
				print(current_action)
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
				print(current_action)
			end,
		["origin"] =
			function()
				print(current_action)
			end,
		["star"] =
			function()
				print(current_action)
			end,
		[case.Default] =
			function()
				print(current_action)
			end,
		[case.Nil] =
			function()
				print("Action is Nil")
			end
	}
end

function script.ReceiveMessage(message)
	if message.Text == "commander" then
		script.commander = message.Sender
	elseif message.Sender == commander then
		script.current_action = message.Text
	end
end

return script