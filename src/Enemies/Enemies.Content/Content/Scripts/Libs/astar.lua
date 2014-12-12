-- Aux: Table.Contains
function table.contains(table, element)
  for _, value in pairs(table) do
    if value == element then
      return true
    end
  end
  return false
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

function astar.createNode(position, f, g, h)
	node = {}
	node.position = position
	node.f = function()
			return g() + h
		end
	node.g = function()
		if type == astar.NodeType.Start then return 0 end
		if not parent then return 0 end

		if position.X == parent.Position.X or position.Y == parent.Position.Y then
			return parent.g() + astar.directWeight
		else
			return parent.g() + astar.diagonalWeight
		end
	end
	node.h = 0
	node.parent = nil
	node.type = astar.NodeType.Path

	return node
end

function astar.createGrid(mapData)
	grid = {}

	grid.width = mapData.TileCount.X
	grid.height = mapData.TileCount.Y

	print("G S = ", grid.width, grid.height )

	for i=1, mapData.TileCount.X do
		grid[i] = {}
		for j=1, mapData.TileCount.Y do
			-- Create node base
			grid[i][j] = astar.createNode(position, f, g, h)
			
			if mapData.CanGo(i, j) then
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
	print("Starting AStar")

	-- 2 - Repeat:
	while running do
		-- 2.a - Search Node with the lowest F cost on the OPEN list.
		local currentNode = nil
		local indexNode = -1
		local currentIndex = 0

		for _, node in ipairs ( open ) do
			if currentNode then
				if currentNode.f() > node.f() then
					currentNode = node
					indexNode = currentIndex
				end
			else
				currentNode = node
				indexNode = currentIndex
			end

			currentIndex = currentIndex + 1
		end

		print("Current Node is X = ", currentNode.position.X, "Y = ", currentNode.position.Y)

		-- 2.b - Move this node to the CLOSE list.
		table.insert(close, start)
		table.remove(open, indexNode)

		-- 2.c - For each neighbour node:
		for i = -1, 2 do
			iPos = i + currentNode.position.X

			if iPos >= 0 and iPos < grid.width then
				for j = -1, 2 do
					jPos = j + currentNode.position.Y
					if jPos >= 0 and jPos < grid.height then
						-- 2.c.0 - Calculate Heuristic
						nbNode = nodes[iPos][jPos]
						nbNode.h = heuristic(nbNode, start, target)
						
						local pass = false
						 
						-- 2.c.i - Ignore if it's an obstacle or if it's already on the CLOSE list.
						if nbNode.type == astar.NodeType.Obstacle or table.contains(close, nbNode) then
							pass = true
						end
						-- 2.c.ii - If it's not on OPEN, add to open and set the Node PARENT to CURRENT. Record f, g and h.
						if not pass and not table.contains(open, nbNode ) then
							nbNode.parent = currentNode
							table.insert(open, nbNode)
							pass = true
						end
						-- 2.c.iii - If it's already on OPEN, check if the new path is better. If it is, change the recorded PARENT, f, g and h values.
						if not pass and table.contains( open, nbNode ) then
							local oldF = nbNode.f()
							local oldParent = nbNode.parent

							nbNode.parent = currentNode
							if oldF < nbNode.f() then
								nbNode.parent = oldParent
							end
						end
					end
				end 
			end
		end

		-- 2.d - Stop the process when:
		if running then
			-- 2.d.i - Found! Target Node is on CLOSE.
			if currentNode.type == astar.NodeType.Target then running = false end
			-- 2.d.ii - Not Found! OPEN is empty.
			if table.getn(open) == 0 then running = false end
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
