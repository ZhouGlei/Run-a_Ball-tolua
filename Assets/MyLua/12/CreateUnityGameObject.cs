using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateUnityGameObject : MonoBehaviour
{
    public LuaState lua;
    LuaFunction luaUpdate;
    // Start is called before the first frame update
    void Start()
    {
        lua = new LuaState();
        lua.AddSearchPath(Application.dataPath + "/MyLua/12");
        lua.Start();
        LuaBinder.Bind(lua);
        luaUpdate = lua.GetFunction("Update");
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 60, 100, 100), "LuaCreateGameObject"))
        {
            lua.DoFile("LuaCreateUnityGameObject.lua");
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
