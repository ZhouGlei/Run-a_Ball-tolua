using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuaController : MonoBehaviour
{
    public static LuaState lua;
    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        lua = new LuaState();
        lua.Start();
        lua.AddSearchPath(Application.dataPath + "/Mylua/Lua-unity-Update");
        LuaBinder.Bind(lua);
    }
    void Start()
    {
        
    }

    private void OnDestroy()
    {
        lua.Dispose();
        lua = null;
    }
}
