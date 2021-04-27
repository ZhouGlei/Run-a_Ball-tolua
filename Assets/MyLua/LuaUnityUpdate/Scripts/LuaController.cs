using LuaInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LuaController : MonoBehaviour
{
    public static LuaController instance;
    public  LuaState lua;

    private string serverPath = "http://192.168.0.103:6688/LuaToAB"; // 服务器地址
    public  bool isUpdateOk = false; // 判断是否下载并加载完毕
    // Start is called before the first frame update
    private IEnumerator Start()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        yield return new WaitForEndOfFrame();
        lua = new LuaState();
        lua.Start();
        lua.AddSearchPath(Application.dataPath + "/Mylua/Lua-unity-Update");
        LuaBinder.Bind(lua);

        //step 1:移动资源
        MoveDirectory(Application.streamingAssetsPath, Application.persistentDataPath);
        print(Application.persistentDataPath);
        //下载资源
        ABInfoList serverVer = null;
        ABInfoList localVer = null;

        WWW wwwSer = new WWW(serverPath + "/filelist.txt");
        StartCoroutine(Download(wwwSer, w3Ser =>
        {
            if(w3Ser.error == null)
            {
                // 将服务器文件转回ABInfoList
                serverVer = JsonUtility.FromJson<ABInfoList>(w3Ser.text);
                // 读取本地文件
                WWW wwwLocal = new WWW("file:///" + Application.persistentDataPath + "/filelist.txt");
                StartCoroutine(Download(wwwLocal, w3Local =>
                {
                    if (w3Local.error == null)
                    {
                        localVer = JsonUtility.FromJson<ABInfoList>(w3Local.text);

                        var localLua = localVer.ABInfos.Find(x => x.abName == "Lua.ab");
                        var serverLua = serverVer.ABInfos.Find(x => x.abName == "Lua.ab");
                        if(localLua == null || serverLua == null)
                        {
                            Debug.LogError("版本文件下载错误----LuaController.cs");
                            return;
                        }
                        // 本地版本小于服务器版本说明需要更新
                        if (localLua.version < serverLua.version)
                        {
                            string url = serverPath + "/Lua.ab";
                            WWW www = new WWW(url);
                            StartCoroutine(Download(www, w3 =>
                            {
                                if (w3.error == null) 
                                {
                                    byte[] stream = w3.bytes;
                                    // FileMode.Create表示有则覆盖，没则创建
                                    FileStream fs = new FileStream(Application.persistentDataPath + "/Lua.ab",FileMode.Create);

                                    BinaryWriter bw = new BinaryWriter(fs);

                                    // 写文件
                                    bw.Write(stream);
                                    bw.Close();
                                    fs.Close();

                                    // 保存新的本地filelist文件
                                    SaveNewList(serverVer, Application.persistentDataPath + "/filelist.txt");
                                    // 标记更新完毕
                                    isUpdateOk = true;
                                }
                                else
                                {
                                    Debug.LogError("更新服务器资源失败----LuaController.cs");
                                }
                            }));
                        }
                        else
                        {
                            Debug.Log("客户端已经是最新版本了----LuaController.cs");
                            isUpdateOk = true;
                        }
                    }
                    else
                    {
                        Debug.LogError("本地文件下载错误----LuaController.cs");
                    }
                }));
            }
            else
            {
                Debug.LogError("服务器文件下载错误----LuaController.cs");
            }
        }));
    }


    public void SaveNewList(ABInfoList infoList,string path)
    {
        File.WriteAllText(path, JsonUtility.ToJson(infoList));
    }

    /// <summary>
    /// 等待下载完毕后调用action
    /// </summary>
    /// <param name="wwwSer"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    private IEnumerator Download(WWW wwwSer, Action<WWW> action)
    {
        
        yield return wwwSer;
        if(wwwSer != null)
        {
            action(wwwSer);
        }
    }

    /// <summary>
    /// 移动文件夹内容到另一个文件夹
    /// </summary>
    /// <param name="input"></param>
    /// <param name="output"></param>
    public void MoveDirectory(string input,string output)
    {
        List<string> files = new List<string>(Directory.GetFiles(input));
        files.ForEach((file) =>
        {
            string destFile = Path.Combine(output, Path.GetFileName(file));
            if (!File.Exists(destFile))
            {
                File.Move(file, destFile);
            }
        });
    }

    /// <summary>
    /// 复制文件夹内容到另一文件夹
    /// </summary>
    /// <param name="input"></param>
    /// <param name="output"></param>
    public void CopyDirectory(string input, string output)
    {
        List<string> files = new List<string>(Directory.GetFiles(input));
        files.ForEach(file =>
        {
            string destFile = Path.Combine(output, Path.GetFileName(file));
            File.Copy(file, destFile, true);
        });
    }
    /// <summary>
    /// 从文件中读取lua脚本
    /// </summary>
    /// <param name="modulename"></param>
    /// <returns></returns>
    public string LoadLuaFromFile(string modulename)
    {
        string s ="";
        #region 旧的读取方式

        /*WWW www = new WWW("file:///E:/GameProject/ToLua/tolua-master/AssetBundles/Windows/lua");*/
        //WWW www = new WWW("file:///E:/GameProject/ToLua/tolua-master/Assets/StreamingAssets/lua");
        /*if (abLua.assetBundle)
        {
            s = abLua.assetBundle.LoadAsset<TextAsset>(modulename).text;
        }
        else
        {

            print($"找不到{modulename}文件");
        }
        www.assetBundle.Unload(false);
        www.Dispose();*/
        #endregion
        AssetBundle abLua = AssetBundle.LoadFromFile(Application.persistentDataPath + "/Lua.ab");
        if (abLua)
        {
            s = abLua.LoadAsset<TextAsset>(modulename).text;
        }
        else
        {
            
            print($"找不到{modulename}文件");
        }
        abLua.Unload(false);
        return s;
    }

    private void OnDestroy()
    {
        lua.Dispose();
        lua = null;
    }
}
