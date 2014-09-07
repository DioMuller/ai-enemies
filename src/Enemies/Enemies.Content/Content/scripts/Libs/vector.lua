vector = {}

function vector.create(x,y)
	return { X = x, Y = y }
end

function vector.size(v)
	return math.sqrt(v.X * v.X + v.Y * v.Y)
end

function vector.normalize(v)
	result = {}
	siz = size(v)
	result.X = v.X / siz
	result.Y = v.Y / siz

	return result
end

function vector.add(v1, v2)
	result = { } 
	result.X = v1.X - v2.X
	result.Y = v1.Y - v2.Y

	return result
end

function vector.subtract(v1, v2)
	result = { } 
	result.X = v1.X - v2.X
	result.Y = v1.Y - v2.Y

	return result
end

function vector.multiply(v, c)
	result = { } 
	result.X = v.X * c
	result.Y = v.Y * c

	return result
end

return vector