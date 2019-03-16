using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;

[DataContract]
public static class ItemInfo {

    static List<ItemProperty> ItemField;//それぞれのアイテムがItemID番目に保存されると仮定した
    [DataMember]
    static Dictionary<string, int> ItemIDResolution;
    [DataMember]
    static List<string> ItemNameResolution;

    public static bool HaveAttribute(string itemname, string attribute)//対象のアイテムのAttributeにattributeがあればtrue, そうでなければfalse
    {
        int thisItemID = ItemIDResolution[itemname];
        LinkedList<string> thisAttribute = ItemField[thisItemID].Attribute;

        foreach (string att in thisAttribute)
        {
            if (att == attribute)
            {
                return true;
            }
        }
        return false;
    }
    public static bool HaveAttribute(int itemid, string attribute)//対象のアイテムのAttributeにattributeがあればtrue, そうでなければfalse
    {
        LinkedList<string> thisAttribute = ItemField[itemid].Attribute;

        foreach(string att in thisAttribute)
        {
            if(att == attribute)
            {
                return true;
            }
        }
        return false;
    }
    public static void LoadItemInfo()//メンバ変数の更新
    {
        string path = SystemVariables.RootPath + "/Data/Item/ItemProperty";
        int idtemp = 0;
        ItemProperty itemProperty;

        string[] files = System.IO.Directory.GetFiles(@path, "*", System.IO.SearchOption.TopDirectoryOnly);
        foreach(string i in files)
        {
            string name = System.IO.Path.GetFileNameWithoutExtension(i);
            if (itemProperty.JsonImport(path, name))
            {
                ItemField.
            }
        }


    }
	
}
