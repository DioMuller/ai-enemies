vector = {}

function vector.print(label, v)
	print(label .. " = [ " .. v.X .. ", " .. v.Y .. "];")
end

function vector.create(x,y)
	local vector = {}
	vector.X = x
	vector.Y = y
	return vector --{ X = x, Y = y }
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