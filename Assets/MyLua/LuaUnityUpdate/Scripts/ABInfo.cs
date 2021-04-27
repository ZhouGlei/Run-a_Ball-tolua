using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]  //便于jsonUtility对该类序列化
public class ABInfo 
{
    public string abName; // 包名
    public string md5; // md5编码
    public int version; // 版本号
}

[Serializable] //便于jsonUtility对该类序列化
public class ABInfoList
{
   public  List<ABInfo> ABInfos = new List<ABInfo>();
}
