using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallLuaFunc : MonoBehaviour
{
    string s =
            @"
        function luaFunc(num)
            return num+1
        end
        test={}
        test.luaFunc=luaFunc
";
    LuaState lua = null;
    LuaFunction luafunc = null;
    // Start is called before the first frame update
    void Start()
    {
        lua = new LuaState();
        lua.Start();
        // 加载lua字符串脚本
        lua.DoString(s);

        // 获取lua函数
        luafunc = lua.GetFunction("test.luaFunc");
        //执行函数
        if (luafunc != null)
        {
           
            int num =luafunc.Invoke<int, int>(123456);
            Debug.Log(num);
        }
        lua.Dispose();
        luafunc.Dispose();
    }

    
}
