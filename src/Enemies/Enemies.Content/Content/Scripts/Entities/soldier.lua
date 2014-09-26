

script = {}

current_action = "wait"
commander = 0

function script.Initialize()

end

function script.DoUpdate(delta)
	case.switch(current_action) 
	{
		["wait"] =
			function()
				if commander == 0 then current_action = "search" end
			end,
		["search"] = 
			function()
				entity:BroadcastMessage("commander")
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
				print(current_action)
			end,
		["look_right"] =
			function()
				print(current_action)
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
		commander = message.Sender
	elseif message.Sender == commander then
		current_action = message.Text
	end
end

return script