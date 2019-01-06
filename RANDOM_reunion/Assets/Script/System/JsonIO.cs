using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;


public static class JsonIO
{
    public static bool JsonExport<T>(T obj, string path, string name)//objをpathディレクトリに[naem].jsonとして保存 成功すればtrue 失敗すればfalse
    {
        try
        {
            using (StreamWriter exportResultStream = new StreamWriter(path + '/' + name + ".json", false, Encoding.Default))//ファイルを読み込み
            {
                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(T));// T 用のjsonSerializerを準備
                jsonSerializer.WriteObject(exportResultStream.BaseStream, obj);//exportして書き込み
            }
        }
        catch
        {
            return false;//あらゆる例外に対してfalseを返す
        }
        return true;
    }
    public static T JsonImport<T>(string path, string name)//pathディレクトリの[naem].jsonを読み込む 失敗などで読み込めなければ,LogAssertionで警告を表示しdefault(T)を返す
    {
        T importResultObj = default(T);

        try
        {
            using (StreamReader importerStream = new StreamReader(path + '/' + name + ".json", Encoding.Default))//ファイルを読み込み
            {
                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(T));// T 用のjsonSerializerを準備
                importResultObj = (T)jsonSerializer.ReadObject(importerStream.BaseStream);//読み込みしてimport
            }
        }
        catch (Exception e)
        {
            Debug.LogAssertion($"JsonIO.JsonImport<{typeof(T)}>()内で{path + '/' + name + ".json"}を読み込もうとした際に例外が発生.");
            return default(T);//あらゆる例外に対してdefaultを返す
        }
        
        return importResultObj;
    }
}