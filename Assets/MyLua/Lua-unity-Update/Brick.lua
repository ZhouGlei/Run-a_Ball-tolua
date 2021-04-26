local Time =UnityEngine.Time
Brick={}
function Brick:Start(go)
		self.go =go
end

function Brick:Update()

		self.go.transform:Rotate(Vector3(45,45,45)*Time.deltaTime)
end

function Brick:new()
	local o ={}
	setmetatable(o,self)
	self.__index=self
	return o
end

return Brick:new()
