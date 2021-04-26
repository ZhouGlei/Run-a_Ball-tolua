using LuaInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptsFromFile1 : MonoBehaviour
{
    LuaState lua;
    string s = "";
    // Start is called before the first frame update
    void Start()
    {
        lua = new LuaState();
        lua.AddSearchPath(Application.dataPath + "/MyLua/02");
        lua.Start();
        Application.logMessageReceived += Log;
    }

    private void Log(string condition, string stackTrace, LogType type)
    {
        s += condition;
    }

    private void OnGUI()
    {
       
        GUILayout.Label(s);
        if (GUILayout.Button("DoFile"))
        {
            lua.DoFile("ScriptsFromFile1.lua");
        }
        if (GUILayout.Button("Require"))
        {
            lua.Require("ScriptsFromFile1");
        }
    }


}
