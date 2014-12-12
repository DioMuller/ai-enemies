-- Aux: Table.Contains
function table.contains(table, element)
  for _, value in pairs(table) do
    if value == element then
      return true
    end
  end
  return false
end

function table.to_string (tt, indent, done)
  done = done or {}
  indent = indent or 0
  if type(tt) == "table" then
    for key, value in pairs (tt) do
      io.write(string.rep (" ", indent)) -- indent it
      if type (value) == "table" and not done [value] then
        done [value] = true
        io.write(string.format("[%s] => table\n", tostring (key)));
        io.write(string.rep (" ", indent+4)) -- indent it
        io.write("(\n");
        table.to_string (value, indent + 7, done)
        io.write(string.rep (" ", indent+4)) -- indent it
        io.write(")\n");
      else
        io.write(string.format("[%s] => %s\n",
            tostring (key), tostring(value)))
      end
    end
  else
    io.write(tt .. "\n")
  end
end

astar = {}

astar.directWeight = 10
astar.diagonalWeight = 14

astar.NodeType =
{
	Start = 'X',
    Path = '-',
    Obstacle = '#',
	End = 'O'
}

function astar.createNode(position)
	node = {}
	
	node.h = 0
	node.parent = nil
	node.type = astar.NodeType.Path
	node.position = position
	node.g = 0
	node.f = 0

	return node
end

function astar.createGrid(mapData)
	grid = {}

	grid.width = mapData.TileCount.X
	grid.height = mapData.TileCount.Y

	local i,j = 0,0

	for i=1, mapData.TileCount.X do
		grid[i] = {}
		for j=1, mapData.TileCount.Y do
			-- Create node base
			grid[i][j] = astar.createNode({X = i - 1, Y = j - 1})
			
			if mapData:CanGo(i - 1, j - 1) then
				grid[i][j].type = astar.NodeType.Path
			else
				grid[i][j].type = astar.NodeType.Obstacle
			end
		end
	end

	

	return grid
end

function astar.findPath(mapData, position, objective, heuristic)
	-- Position and Objective Quad Calculation
	local startPos = mapData:GetQuadrantOf(position.X, position.Y)
	local targetPos = mapData:GetQuadrantOf(objective.X, objective.Y)

	-- 0 - Initialization
	print("Starting Pos = ", startPos.X, startPos.Y )
	local nodes = astar.createGrid(mapData)
	local start = nodes[startPos.X][startPos.Y]
	local target = nodes[targetPos.X][targetPos.Y]

	start.type = astar.NodeType.Start
	target.type = astar.NodeType.End

	local close = {}
	local open = {}
	local running = true

	-- 1 - Add Start Node to the OPEN List.
	table.insert(open, start)

	-- 2 - Repeat:
	while running do
		-- 2.a - Search Node with the lowest F cost on the OPEN list.
		local currentNode = nil

		for key, node in ipairs ( open ) do
			if currentNode then
				if currentNode.f > node.f then
					currentNode = node
				end
			else
				currentNode = node
			end
		end

		--print("Current Node is X = ", currentNode.position.X, "Y = ", currentNode.position.Y)

		if currentNode then
			-- 2.b - Move this node to the CLOSE list.
			table.insert(close, currentNode)

			i=1
			while i <= #open do
				if open[i] == nbNode then
					table.remove(open, i)
				else
					i = i + 1
				end
			end

			-- 2.c - For each neighbour node:
			for i = -1, 1 do
				iPos = i + currentNode.position.X

				if iPos >= 1 and iPos <= grid.width then
					for j = -1, 1 do
						jPos = j + currentNode.position.Y
						if jPos >= 1 and jPos <= grid.height then
							nbNode = nodes[iPos][jPos]

							-- 2.c.0 - Calculate Values
							if nbNode then
								nbNode.h = heuristic(nbNode, start, target)
								local g = 0
								local f = 0

								if nbNode.position.X == currentNode.position.X or nbNode.position.Y == currentNode.position.Y then
									g = currentNode.g + astar.directWeight
								else
									g = currentNode.g + astar.diagonalWeight
								end

								f = g + nbNode.h
						
								local pass = false
						 
								-- 2.c.i - Ignore if it's an obstacle or if it's already on the CLOSE list.
								if nbNode.type == astar.NodeType.Obstacle or table.contains(close, nbNode) then
									pass = true
									--print("Node ", nbNode.position.X, ",", nbNode.position.Y, " already on CLOSE")
								end
								-- 2.c.ii - If it's not on OPEN, add to open and set the Node PARENT to CURRENT. Record f, g and h.
								if not pass and not table.contains(open, nbNode ) then
									nbNode.parent = currentNode
									nbNode.g = g
									nbNode.f = f
									table.insert(open, nbNode)
									pass = true
								end
								-- 2.c.iii - If it's already on OPEN, check if the new path is better. If it is, change the recorded PARENT, f, g and h values.
								if not pass and table.contains( open, nbNode ) then
									if f < nbNode.f then
										nbNode.parent = currentNode
										nbNode.g = g
										nbNode.f = f
									end
								end
							end
						end
					end 
				else
					running = false
				end
			end
		end

		-- 2.d - Stop the process when:
		if running then
			-- 2.d.i - Found! Target Node is on CLOSE.
			if currentNode.type == astar.NodeType.Target then running = false end
			-- 2.d.ii - Not Found! OPEN is empty.
			if #open == 0 then running = false end
		end

	end

	-- 3 - Generate a path using the PARENT nodes.
	result = {}

	while currentNode and not currentNode.type == astar.NodeType.Start do
		table.insert(result, currentNode)
		currentNode = currentNode.parent
	end

	-- 4 - Return the result.
	return result
end

return astar
