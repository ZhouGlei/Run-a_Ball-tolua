using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public LuaFunction LuaStart;
    public LuaFunction LuaUpdate;
    // Start is called before the first frame update
    void Start()
    {
        string filename = this.gameObject.name;
        LuaController.instance.lua.DoFile(filename + ".lua");
        LuaStart = LuaController.instance.lua.GetFunction(filename + ".Start");
        LuaUpdate = LuaController.instance.lua.GetFunction(filename + ".Update");
        if (LuaStart != null)
        {
            LuaStart.Call(this.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (LuaUpdate!=null)
        {
            LuaUpdate.Call();
        }
    
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 60, 100, 100), "reload"))
        {
            LuaController.instance.lua.DoFile(this.gameObject.name + ".lua");
            
        }
    }
}
