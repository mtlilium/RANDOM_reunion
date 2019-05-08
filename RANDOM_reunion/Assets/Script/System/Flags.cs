using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;

public static class Flags /*:MonoBehaviour*/ {

    public static Dictionary<string, bool> FlagField;

    public static bool GetFlag(string flagname)
    {
        if (FlagField.ContainsKey(flagname))
        {
            return FlagField[flagname];
        }
        else
        {
            Debug.Log(flagname + "というキーは存在しません");
            return false;
        }
    }
    public static bool HaveFlag(string flagname)
    {
        if (FlagField.ContainsKey(flagname))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

        //IJsonSaveLoadable
    public static bool JsonExport(string path, string name, bool overwrite)
    {
        string filePath = path + "/" + name + ".json";

        if (File.Exists(filePath) && !overwrite)
        {
            return false;
        }
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        FlagsForSerialization exporter = new FlagsForSerialization();

        exporter.FlagField = FlagField;

        return JsonIO.JsonExport(exporter, path, name);
    }

    public static bool JsonImport(string path, string name)
    {
        string FilePath = path + "/" + name + ".json";
        if (!File.Exists(FilePath))
        {
            return false;
        }
        FlagsForSerialization scr = JsonIO.JsonImport<FlagsForSerialization>(path, name);

        FlagField = scr.FlagField;
        return true;
    }
        
    [DataContract]
    public class FlagsForSerialization
    {
        [DataMember]
        public Dictionary<string, bool> FlagField;

        public FlagsForSerialization() { }
    }
}
