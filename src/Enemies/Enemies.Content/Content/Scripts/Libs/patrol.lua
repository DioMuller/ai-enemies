patrol = {}

function patrol.create()
	self = {}
	self.points = {}
	self.current = 0
	self.size = 0

	return self
end

function patrol.add(self, vector)
	self.size = self.size + 1
	self.points[self.size] = vector

	if self.current == 0 then self.current = 1 end
end

function patrol.getnext(self)
	self.current = self.current + 1
	if self.current > self.size then self.current = 1 end

	return self.points[self.current]
end

return patrol