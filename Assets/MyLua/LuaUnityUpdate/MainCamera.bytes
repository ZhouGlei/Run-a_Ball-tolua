local GameObject = UnityEngine.GameObject
MainCamera={}
MainCamera.Start=function(camera)
	MainCamera.player = camera.transform.root:Find('Player')
	MainCamera.camera=camera.transform
	MainCamera.offset = camera.transform.position - MainCamera.player.position
end

MainCamera.LateUpdate=function()
	MainCamera.camera.position= MainCamera.offset + MainCamera.player.position
end
