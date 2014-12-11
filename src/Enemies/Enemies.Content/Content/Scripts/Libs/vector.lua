vector = {}

function vector.print(label, v)
	print(label, v.X, v.Y)
end

function vector.create(x,y)
	vec = {}
	vec.X = x
	vec.Y = y

	return vec
end

function vector.size(v)
	if not v then return 0 end
	return math.sqrt(v.X * v.X + v.Y * v.Y)
end

function vector.normalize(v)
	if not v then return nil end
	result = {}
	siz = vector.size(v)
	result.X = v.X / siz
	result.Y = v.Y / siz

	return result
end

function vector.add(v1, v2)
	if not v1 or not v2 then return nil end

	result = { } 
	result.X = v1.X - v2.X
	result.Y = v1.Y - v2.Y

	return result
end

function vector.subtract(v1, v2)
	if not v1 or not v2 then return nil end

	result = { } 
	result.X = v1.X - v2.X
	result.Y = v1.Y - v2.Y

	return result
end

function vector.multiply(v, c)
	if not v or not c then return nil end
	result = { } 
	result.X = v.X * c
	result.Y = v.Y * c

	return result
end

function vector.square_distance(v1, v2)
	if not v1 or not v2 then return 0 end
	return ((v1.X - v2.X) * (v1.X - v2.X)) + ((v1.Y - v2.Y) * (v1.Y - v2.Y))
end

function vector.distance(v1, v2)
	if not v1 or not v2 then return 0 end
	return math.sqrt( vector.square_distance(v1, v2) )
end

return vector