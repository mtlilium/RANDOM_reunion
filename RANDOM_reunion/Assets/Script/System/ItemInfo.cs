using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;

[DataContract]
public static class ItemInfo {
    List<ItemProperty> ItemField;

    [DataMember]
    Dictionary<string, int> ItemIDResolution;
    [DataMember]
    List<string> ItemNameResolution;

    public bool HaveAttribute(string itemname, string attribute)
    {

    }
    public bool HaveAttribute(int itemid, string attribute)
    {

    }
    public void LoadItemInfo()
    {

    }
	
}
