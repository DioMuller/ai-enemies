mathlib = {}

mathlib.tau = math.pi * 2

function mathlib.randomSign()
    return random()>0.5 and 1 or -1
  end

return mathlib