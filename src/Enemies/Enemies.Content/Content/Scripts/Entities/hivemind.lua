script = {}

script.current_target = nil

function script.Initialize()

end

function script.DoUpdate(delta)
	if script.current_target then
		local movement = steering.seek(entity.Position, script.current_target, 1)
		
		entity:Move(movement.X, movement.Y)
	end
end

function script.DoUpdate(delta)

end

function script.ReceiveMessage(message)
	if message.Text == "follow" then
		local newPos = message.Attachment

		if newPos then
			if not script.current_target then 
				script.current_target = newPos
			elseif vector.distance(entity:Position, script.current_target) > vector.distance(entity:Position, newPos) then
				script.current_target = newPos
			end
		end
	end
end

return script