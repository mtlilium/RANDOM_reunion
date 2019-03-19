using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;

public class ItemConversionInfo
{
    public LinkedList<ItemConversion> RecipesItemConversionAll;

    Dictionary<string, LinkedList<ItemConversion>> RecipesItemConversion;

    public void LoadItemConversionInfo()
    {
        RecipesItemConversionAll = new LinkedList<ItemConversion>();
        RecipesItemConversion = new Dictionary<string, LinkedList<ItemConversion>>();

        DirectoryInfo dir = new DirectoryInfo(SystemVariables.RootPath + "/Data/Item/ItemConversion");
        FileInfo[] info = dir.GetFiles("*.json");

        foreach (FileInfo f in info)
        {
            string name = Path.GetFileNameWithoutExtension(f.Name);
            ItemConversion ic = JsonIO.JsonImport<ItemConversion>(SystemVariables.RootPath + "/Data/Item/ItemConversion", name);
            RecipesItemConversionAll.AddLast(ic);
            foreach(var i in ic.Attribute)
            {
                if (!RecipesItemConversion.ContainsKey(i))
                    RecipesItemConversion.Add(i,new LinkedList<ItemConversion>());
                RecipesItemConversion[i].AddLast(ic);
            }
        }
    }
}
