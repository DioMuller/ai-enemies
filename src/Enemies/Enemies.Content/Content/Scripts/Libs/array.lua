array = {}

function array.foreach(o)
   local e = o:GetEnumerator()
   return function()
      if e:MoveNext() then
        return e.Current
     end
   end
end

function array.first(o)
	local e = o:GetEnumerator()
	if e:MoveNext() then
		return e.Current
	else
		return nil
	end
end

return array