tags = {}

tags.enemy = 0
tags.player = 1
tags.objective = 2
tags.none = 3

function tags.getTag(tag)
	if tag == tags.enemy then return "enemy"
	elseif tag == tags.player then return "player"
	elseif tag == tags.objective then return "objective"
	else return "none" end
end

return tags