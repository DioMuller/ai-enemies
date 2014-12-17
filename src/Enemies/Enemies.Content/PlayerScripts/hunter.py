from math import pow
from math import sqrt
from Enemies.Entities import BaseEntity
from Enemies.Entities import TypeTag

class AStarNode(object):
    G = None
    H = None
    Parent = None
    def __init__(self, x, y):
        self.X = x
        self.Y = y
    @property
    def F(self):
        return self.G + self.H

class ScriptEntity(BaseEntity):
    state_searching = 1
    state_attacking = 2
    state_reaching = 3

    map = None
    map_info = None
    places_to_walk = None
    next_path_update = 0
    next_shoot_time = 0
    next_state_update = 0
    path_update_delay = 500
    shoot_delay = 300
    state_update_delay = 100
    state = 1
    target = None
    enemy = None

    #def Initialize(self):
        #Nothing else to do.

    def DoUpdate(self, game_time):
        super(ScriptEntity, self).DoUpdate(game_time)

        self.next_path_update -= game_time
        self.next_state_update -= game_time
        self.next_shoot_time -= game_time

        if self.next_state_update <= 0:
            self.next_state_update = self.state_update_delay
            enemies = self.GetShootableEntities()
            self.target = self.GetNearestTarget()


            if enemies.Length > 0 and self.CanReach(enemies[0].Position.X, enemies[0].Position.Y):
                self.state = self.state_attacking
                self.enemy = enemies[0]
            else:
                if self.CanReach(self.target.Position.X, self.target.Position.Y):
                    self.state = self.state_reaching
                else:
                    self.state = self.state_searching

        if self.state == self.state_attacking and self.next_shoot_time < 0:        
            self.ShootAt(self.enemy.Position.X, self.enemy.Position.Y)
            self.next_shoot_time = self.shoot_delay
        else: 
            if self.state == self.state_reaching:
                self.MoveTo(self.target.Position.X,self.target.Position.Y)
            else:
                if self.state == self.state_searching:
                    self.FindPath()
                    self.WalkRoute()

    def LoadMapInfo(self):
        self.map_info = self.GetMapLayout()
        self.map = [[AStarNode(x, y) if self.map_info.CanGo(x, y) else None for x in range(self.map_info.TileCount.X)] for y in range(self.map_info.TileCount.Y)]

    def CurrentPosition(self):
        position = self.BoundingBox.Center
        return self.TilePosition(position.X, position.Y)

    def TargetPosition(self):
        target = self.GetNearestTarget()
        if not target:
            return None
        position = target.Position
        return self.TilePosition(position.X, position.Y)

    def TilePosition(self, x, y):
        tile_size = self.map_info.TileSize;
        sx = x / tile_size.X
        sy = y / tile_size.Y
        return self.map[int(sy)][int(sx)]

    def MapPosition(self, tile):
        tile_size = self.map_info.TileSize;
        return tile.X * tile_size.X + tile_size.X / 2, tile.Y * tile_size.Y + tile_size.Y / 2

    def FindPath(self):
        if self.next_path_update > 0:
            return
        self.LoadMapInfo()
        self.next_path_update = self.path_update_delay
        target = self.TargetPosition()
        if not target:
            return
        initial = self.CurrentPosition()

        initial.G = 0
        initial.H = abs(target.Y - initial.Y) + abs(target.X - initial.X)
        open = [initial]
        closed = []
        while open:
            current = min(open, key=lambda x: x.F)
            open.remove(current)
            closed.append(current)
            if current == target:
                self.places_to_walk = [target]
                while current != initial:
                    current = current.Parent
                    self.places_to_walk = [current] + self.places_to_walk
                return

            for neighbor in self.Neighbors(current):
                if neighbor in closed:
                    continue
                if not neighbor in open:
                    open.append(neighbor)
                cost = 10 if neighbor.X == current.X or neighbor.Y == current.Y else 14
                if neighbor.G == None or neighbor.G > current.G + cost:
                    neighbor.Parent = current
                    neighbor.G = current.G + cost
                    neighbor.H = abs(target.Y - neighbor.Y) + abs(target.X - neighbor.X)

        self.places_to_walk = None

    def WalkRoute(self):
        if not self.places_to_walk:
            return

        nextX, nextY = self.MapPosition(self.places_to_walk[0])
        x, y = self.BoundingBox.Center.X, self.BoundingBox.Center.Y
        self.Move(nextX - x, nextY - y)

        if sqrt(pow(nextX - x, 2) + pow(nextY - y, 2)) < 1:
            self.places_to_walk.pop(0)
            self.next_path_update = 2000
        pass

    def Neighbors(self, node):
        for dy in range(-1, 2):
            for dx in range(-1, 2):
                if bool(dx) == bool(dy):
                    continue
                if node.Y + dy < 0 or node.Y + dy > len(self.map):
                    continue
                if node.X + dx < 0 or node.X + dx > len(self.map[node.Y + dy]):
                    continue
                neighbor = self.map[node.Y + dy][node.X + dx]
                if neighbor:
                    yield neighbor
