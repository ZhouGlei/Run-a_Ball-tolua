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
    IEnumerator Start()
    {
        while (!LuaController.instance.isUpdateOk)
            yield return new WaitForSeconds(1);
         string fileNameWithOutExt = this.gameObject.name;
        string lua = LuaController.instance.LoadLuaFromFile(fileNameWithOutExt+".bytes");
        LuaController.instance.lua.DoString(lua, "LuaBridge.cs");
        
        //LuaController.instance.lua.DoFile(fileNameWithOutExt + ".lua"); 
        
        luaUpdate =  LuaController.instance.lua.GetFunction(fileNameWithOutExt + ".Update");
        luaStart = LuaController.instance.lua.GetFunction(fileNameWithOutExt + ".Start");
        luaLateUpdate = LuaController.instance.lua.GetFunction(fileNameWithOutExt + ".LateUpdate");
        luaOnTriggerEnter = LuaController.instance.lua.GetFunction(fileNameWithOutExt + ".OnTriggerEnter");
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
