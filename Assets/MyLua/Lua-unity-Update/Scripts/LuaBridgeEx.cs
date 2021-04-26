using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuaBridgeEx : MonoBehaviour
{
    public LuaTable table;
    public LuaFunction luaUpdate;
    public LuaFunction luaStart;
    public LuaFunction luaLateUpdate;
    public LuaFunction luaOnTriggerEnter;
    // Start is called before the first frame update
    void Start()
    {
        string fileNameWithOutExt = this.gameObject.name;
        table = LuaController.lua.DoFile<LuaTable>(fileNameWithOutExt + ".lua");
        luaUpdate =  LuaController.lua.GetFunction(fileNameWithOutExt + ".Update");
        luaStart = LuaController.lua.GetFunction(fileNameWithOutExt + ".Start");
        luaLateUpdate = LuaController.lua.GetFunction(fileNameWithOutExt + ".LateUpdate");
        luaOnTriggerEnter = LuaController.lua.GetFunction(fileNameWithOutExt + ".OnTriggerEnter");
        if(luaStart != null)
        {
            luaStart.Call(table,this.gameObject);
            
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (luaUpdate != null)
        {
            luaUpdate.Call(table);
        }

    }

    private void LateUpdate()
    {
        if(luaLateUpdate != null)
        {
            luaLateUpdate.Call(table, this.transform);
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (luaOnTriggerEnter != null)
        {
            luaOnTriggerEnter.Call(table, other);
        }
    }

}
