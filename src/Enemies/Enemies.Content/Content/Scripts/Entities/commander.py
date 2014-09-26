from Enemies.Entities import BaseEntity
from Enemies.Entities import TypeTag

class ScriptEntity(BaseEntity):
	# Attributes
	current = -1
	total_time = 0
	change_time = 1000
	next_command = "wait"
	dialog = "Starting Drill Exercise!"

	# Enumerator
	def circle(self):
		self.change_time = 30000
		self.next_command = "circles"
		self.dialog = "Run in Circles!"

	def rotate(self):
		self.change_time = 15000
		self.next_command = "rotate"
		self.dialog = "I want to see you rotate!"

	def lookleft(self):
		self.change_time = 5000
		self.next_command = "look_left"
		self.dialog = "Look left!"

	def lookright(self):
		self.change_time = 5000
		self.next_command = "look_right"
		self.dialog = "Look right!"

	def patrol(self):
		self.change_time = 5000
		self.next_command = "patrol"
		self.dialog = "Patrol your area!"

	def origin(self):
		self.change_time = 15000
		self.next_command = "origin"
		self.dialog = "Back to your positions!"

	def star(self):
		self.change_time = 35000
		self.next_command = "star"
		self.dialog = "Star Movement!"

	
	options = { 
		0 : circle,
        1 : rotate,
        2 : lookleft,
        3 : lookright,
        4 : patrol,
        5 : origin,
        6 : star
	}
	option_count = 7

	# Game Cycle
	def Initialize(self):
		self.BroadcastMessage("set commander")
	
	def DoUpdate(self, game_time):
		self.total_time += game_time

		if self.total_time > self.change_time:
			self.options[self.NextValue()](self)
			self.BroadcastMessage(self.next_command)
			#TODO: Talk: dialog
			self.total_time = 0
		
		super(ScriptEntity, self).DoUpdate(game_time)

	def NextValue(self):
		self.current = (self.current+ 1) % self.option_count
		return self.current

	def ReceiveMessage(self, message):
		if message.Text == "commander":
			self.SendMessage(message.Sender, "set commander")