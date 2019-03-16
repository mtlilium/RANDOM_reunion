using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;

[DataContract]
public static class ItemInfo {

    static List<ItemProperty> ItemField;

    [DataMember]
    static Dictionary<string, int> ItemIDResolution;
    [DataMember]
    static List<string> ItemNameResolution;

    public static bool HaveAttribute(string itemname, string attribute)
    {

    }
    public static bool HaveAttribute(int itemid, string attribute)
    {

    }
    public static void LoadItemInfo()
    {
        string path = SystemVariables.RootPath + "/Data/Item/ItemProperty";

    }
	
}
