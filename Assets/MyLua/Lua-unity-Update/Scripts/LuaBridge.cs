using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuaBridge : MonoBehaviour
{
    public LuaFunction luaUpdate;
    public LuaFunction luaStart;
    public LuaFunction luaLateUpdate;
    public LuaFunction luaOnTriggerEnter;
    // Start is called before the first frame update
    void Start()
    {
        string fileNameWithOutExt = this.gameObject.name;
        LuaController.lua.DoFile(fileNameWithOutExt + ".lua");
        luaUpdate =  LuaController.lua.GetFunction(fileNameWithOutExt + ".Update");
        luaStart = LuaController.lua.GetFunction(fileNameWithOutExt + ".Start");
        luaLateUpdate = LuaController.lua.GetFunction(fileNameWithOutExt + ".LateUpdate");
        luaOnTriggerEnter = LuaController.lua.GetFunction(fileNameWithOutExt + ".OnTriggerEnter");
        if(luaStart != null)
        {
            luaStart.Call(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (luaUpdate != null)
        {
            luaUpdate.Call();
        }

    }

    private void LateUpdate()
    {
        if(luaLateUpdate != null)
        {
            luaLateUpdate.Call(this.transform);
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (luaOnTriggerEnter != null)
        {
            luaOnTriggerEnter.Call(other);
        }
    }

}
