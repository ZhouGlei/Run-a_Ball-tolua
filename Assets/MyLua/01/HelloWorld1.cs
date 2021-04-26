using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelloWorld1 : MonoBehaviour
{
    private void Awake()
    {
        LuaState lua = new LuaState();
        lua.Start();
        string hello =
            @"
            print('Hello world')
    ";
        lua.DoString(hello, "HelloWorld1.cs");
        lua.Dispose();
        lua = null;
    }
}
