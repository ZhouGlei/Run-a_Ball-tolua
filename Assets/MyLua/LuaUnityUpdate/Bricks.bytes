local Time =UnityEngine.Time
Bricks={}
Bricks.Start=function(go)
	Bricks.go =go
	Bricks.childs ={}
	for i=0,go.transform.childCount-1 do
		table.insert(Bricks.childs,go.transform:GetChild(i))
	end
end

Bricks.Update =function()
	for i=1,#Bricks.childs do
		Bricks.childs[i]:Rotate(Vector3(45,45,45)*Time.deltaTime)
	end
end

