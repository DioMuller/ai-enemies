case = {}

case.Default, case.Nil = {}, function () end -- for uniqueness
function case.switch (i)
  return setmetatable({ i }, {
    __call = function (t, cases)
      local item = #t == 0 and case.Nil or t[1]
      return (cases[item] or cases[case.Default] or case.Nil)(item)
    end
  })
end

return case