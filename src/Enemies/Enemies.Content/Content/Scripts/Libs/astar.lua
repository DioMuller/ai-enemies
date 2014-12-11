astar = {}

NodeType =
{
	Start = 'X',
    Path = '-',
    Obstacle = '#',
	End = 'O'
}

function createNode(position, f, g, h)
	return
	{
		position = position,
		f = 0,
		g = 0,
		h = 0,
		parent = nil,
		type = NodeType.Path
	}
end

function createGrid(mapData)
	grid = {}

	grid.width = mapData.TileCount.X
	grid.height = mapData.TileCount.Y

	for i=1, mapData.TileCount.X do
		grid[i] = {}
		for j=1, mapData.TileCount.Y do
			-- Create node base
			grid[i][j] = createNode(position, f, g, h)
			
			if mapData.CanGo(i, j) then
				grid[i][j].type = NodeType.Path
			else
				grid[i][j].type = NodeType.Obstacle
			end
		end
	end

	return grid
end

function astar.findPath(mapData, position, objective)
	-- Position and Objective Quad Calculation
	local startPos = createNode(mapData:GetQuadrantOf(position.X, position.Y))
	local targetPos = mapData:GetQuadrantOf(objective.X, objective.Y)

	-- 0 - Initialization
	nodes = createGrid(mapData)
	start = nodes[startPos.X][startPos.Y]
	target = nodes[targetPos.X][targetPos.Y]

	start.type = NodeType.Start
	target.type = NodeType.End

	local close = {}
	local open = {}
	local running = true

	-- 1 - Add Start Node to the OPEN List.
	table.insert(open, start)
	-- 2 - Repeat:
	while running do
		-- 2.a - Search Node with the lowest F cost on the OPEN list.
		local currentNode = nil
		local indexNode = -1
		local currentIndex = 0

		for _, node in ipairs ( open ) do
			if currentNode then
				if currentNode.f > node.f then
					currentNode = node
					indexNode = currentIndex
				end
			else
				currentNode = node
				indexNode = currentIndex
			end

			currentIndex = currentIndex + 1
		end
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
						-- 2.c.i - Ignore if it's an obstacle or if it's already on the CLOSE list.
						-- 2.c.ii - If it's not on OPEN, add to open and set the Node PARENT to CURRENT. Record f, g and h.
						-- 2.c.iii - If it's already on OPEN, check if the new path is better. If it is, change the recorded PARENT, f, g and h values.
				end 
			end
		end

		-- 2.d - Stop the process when:
		if running then
			-- 2.d.i - Found! Target Node is on CLOSE.
			if currentNode.type == NodeType.Target then running = false end
			-- 2.d.ii - Not Found! OPEN is empty.
			if table.getn(open) == 0 then running = false end
		end

	end

	-- 3 - Generate a path using the PARENT nodes.
	result = {}

	while currentNode and not currentNode.type == NodeType.Start do
		table.insert(result, currentNode)
		currentNode = currentNode.parent
	end

	-- 4 - Return the result.
	return result
end