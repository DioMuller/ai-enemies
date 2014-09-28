polygon = {}

function polygon.generate(points, start, radius)
	local poly = {}
	poly[1] = start

	for i = 2, points do
		local x, y = poly[i-1].X, poly[i-1].Y
		poly[i] = vector.create( x + radius * math.cos(2 * math.pi * i / points), y + radius * math.sin(2 * math.pi * i / points) )
	end

	return poly
end

return polygon