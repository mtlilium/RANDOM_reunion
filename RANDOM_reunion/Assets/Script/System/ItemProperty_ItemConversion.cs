using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;

[DataContract]
public class ItemProperty : IJsonI
{
    [DataMember]
    public string Name;
    [DataMember]
    public LinkedList<string> Attribute;

    public bool JsonImport(string path, string name)
    {
        string FilePath = path + "/" + name + ".json";
        if (!File.Exists(FilePath))
        {
            return false;
        }
        ItemProperty ip = JsonIO.JsonImport<ItemProperty>(path, name);

        Name = ip.Name;
        Attribute = ip.Attribute;

        return true;
    }
}

[DataContract]
public class ItemConversion : IJsonI
{
    [DataMember]
    public string Name;
    [DataMember]
    public LinkedList<Tuple<string, int>> ItemConversionRecipe;
    [DataMember]
    public LinkedList<string> Attribute;

    public bool JsonImport(string path, string name)
    {
        string FilePath = path + "/" + name + ".json";
        if (!File.Exists(FilePath))
        {
            return false;
        }
        ItemConversion ic = JsonIO.JsonImport<ItemConversion>(path, name);

        Name = ic.Name;
        ItemConversionRecipe = ic.ItemConversionRecipe;
        Attribute = ic.Attribute;

        return true;
    }
}
