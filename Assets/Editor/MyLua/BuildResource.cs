using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

public class BuildResource : MonoBehaviour
{
    private static string outputPath = Application.streamingAssetsPath;
    private static string inputPath = Application.dataPath + "/MyLua/LuaUnityUpdate";
    [MenuItem("Build lua/Build")]
    public static void BuildLuaResource()
    {
        //////////////////////////////////////////////////////////////////
        ///step:1 加载已经存在的filelist文件，如果是第一次打包，则构造一个空的filelist文件
        if (!Directory.Exists(outputPath))
        {
            // 目录不存在则创建一个目录
            Directory.CreateDirectory(outputPath);
        }

        ABInfoList oldlist = null;
        if (File.Exists(outputPath + "/filelist.txt"))
        {
            oldlist = JsonUtility.FromJson<ABInfoList>(File.ReadAllText(outputPath + "/filelist.txt"));
        }
        else
            oldlist = new ABInfoList();

        ////////////////////////////////////////////////////////////////////////////////
        /// step:2 资源打包
        AssetBundleBuild abLua = new AssetBundleBuild();
        abLua.assetBundleName = "Lua.ab";
        //按路径取资源
        string[] files =Directory.GetFiles(inputPath, "*.bytes");
        for(int i = 0; i < files.Length; i++)
        {
            files[i] = "Assets" + files[i].Replace(Application.dataPath, "").Replace("\\","/");
        }
        abLua.assetNames = files;
        AssetBundleBuild[] buildMap = new AssetBundleBuild[] { abLua };
        
        //输出路径，buildmap数组，额外的设置，打包的目标（平台）
        //加载（资源已经下载到本地）资源时有两种方式：
        // 1.异步www加载，会造成代码时序上的困扰（时序不容易控制）
        // 2.同步加载AssetBundle.LoadFromFile:（要求）资源打包时必须使用UncompressedAssetBundle选项
        BuildPipeline.BuildAssetBundles(outputPath, buildMap, BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.StandaloneWindows64);


        ////////////////////////////////////////////////////////////////////////////////
        ///step 3:进行资源对比，并更新filelist
        ///
        ABInfo curLuaInfo = new ABInfo();
        curLuaInfo.abName = "Lua.ab";
        curLuaInfo.md5 =  BitConverter.ToString( new MD5CryptoServiceProvider().ComputeHash(File.OpenRead(outputPath + "/lua.ab")));

        ABInfo oldLuaInfo = oldlist.ABInfos.Find((item) => item.abName == curLuaInfo.abName);
        if (oldLuaInfo != null)
        {
            curLuaInfo.version = oldLuaInfo.md5 == curLuaInfo.md5 ? oldLuaInfo.version : oldLuaInfo.version + 1;
        }
        else
        {
            curLuaInfo.version = 0;
        }

        var curList = new ABInfoList();
        curList.ABInfos.Add(curLuaInfo);
        string s = JsonUtility.ToJson(curList);
        File.WriteAllText(outputPath + "/filelist.txt", s);
        LuaController.instance.CopyDirectory(Application.streamingAssetsPath, Application.dataPath + "/../LuaToAB");
    }

    
}
