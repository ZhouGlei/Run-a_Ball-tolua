local GameObject = UnityEngine.GameObject
local PrimitiveType = UnityEngine.PrimitiveType
local Color = UnityEngine.Color
local cube = GameObject.CreatePrimitive(PrimitiveType.Cube)
local light = GameObject('Light')
light:AddComponent(typeof(UnityEngine.Light))
light.transform.position = Vector3(0,1.3,-1)
light:GetComponent(typeof(UnityEngine.Light)).color = Color.red



