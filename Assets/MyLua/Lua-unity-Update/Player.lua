local Input = UnityEngine.Input
local LayerMask = UnityEngine.LayerMask
local Destory = UnityEngine.Object.Destroy

Player={}
Player.Start=function(go)
	Player.gameObject= go
	Player.rd=go:GetComponent('Rigidbody')
	Player.scoreText = UnityEngine.GameObject.Find('ScoreText'):GetComponent('Text')
	Player.winText = UnityEngine.GameObject.Find('WinText'):GetComponent('Text')
	Player.score = 0
	Player.scoreText.text = "Score:"..Player.score
end
Player.Update=function()
	local h = Input.GetAxis('Horizontal')
	local v = Input.GetAxis('Vertical')
	local v3 = Vector3(h,0,v)
	Player.rd:AddForce(v3)
end

Player.OnTriggerEnter=function(other)
	if other.gameObject.layer == LayerMask.NameToLayer("Brick") then
		Destory(other.gameObject)
		Player.score =Player.score+1
		Player.scoreText.text = "Score:"..Player.score
		--Player.scoreText.text = "µÃ·Ö£º"..Player.score
		if Player.score >=9 then
		Player.winText.text = "You Win!!"
		end

	end
end

