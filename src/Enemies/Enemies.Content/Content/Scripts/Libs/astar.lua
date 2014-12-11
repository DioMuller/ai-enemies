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

	-- 1 - Add Start Node to the OPEN List.
	-- 2 - Repeat:
		-- 2.a - Search Node with the lowest F cost on the OPEN list.
		-- 2.b - Move this node to the CLOSE list.
		-- 2.c - For each neighbour node:
			-- 2.c.0 - Calculate Heuristic
			-- 2.c.i - Ignore if it's an obstacle or if it's already on the CLOSE list.
			-- 2.c.ii - If it's not on OPEN, add to open and set the Node PARENT to CURRENT. Record f, g and h.
			-- 2.c.iii - If it's already on OPEN, check if the new path is better. If it is, change the recorded PARENT, f, g and h values.
		-- 2.d - Stop the process when:
			-- 2.d.i - Found! Target Node is on CLOSE.
			-- 2.d.ii - Not Found! OPEN is empty.
	-- 3 - Generate a path using the PARENT nodes.
	result = {}

	-- 4 - Return the result.
	return result
end