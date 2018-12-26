using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;

public static class JsonIO
{
    public static bool JsonExport<T>(T obj, string path, string name)//objをpathディレクトリに[naem].jsonとして保存 成功すればtrue 失敗すればfalse
    {
        return false;//(返り値なしだとエラーのため適当にしています.要修正)
    }
    public static T JsonImport<T>(string path, string name)//pathディレクトリの[naem].jsonを読み込む 失敗などで読み込めなければ,LogAssertionで警告を表示しdefault(T)を返す
    {
        return default(T);//(返り値なしだとエラーのため適当にしています.要修正)
    }
}