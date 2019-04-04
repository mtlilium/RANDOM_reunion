using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;

[DataContract]
public static class ItemInfo {

    /*IDについて、スクリプトを読み込んだ順にIDも（0から）決まると仮定した*/

    static List<ItemProperty> ItemField;
    [DataMember]
    static Dictionary<string, int> ItemIDResolution;
    [DataMember]
    static List<string> ItemNameResolution;
    
    public static int GetItemID(string str)
    {
        if (ItemIDResolution.ContainsKey(str))
            return ItemIDResolution[str];
        else
            return -1;
    }

    public static bool HaveAttribute(string itemname, string attribute)//対象のアイテムのAttributeリストにattributeがあればtrue, そうでなければfalse
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
    public static bool HaveAttribute(int itemid, string attribute)//対象のアイテムのAttributeリストにattributeがあればtrue, そうでなければfalse
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
    public static void LoadItemInfo()//メンバ変数とItemFieldの更新
    {
        string path = SystemVariables.RootPath + "/Data/Item/ItemProperty";
        string[] files = System.IO.Directory.GetFiles(@path, "*.json", System.IO.SearchOption.TopDirectoryOnly);

        ItemField = new List<ItemProperty>();//メンバ変数とItemFieldを空に
        ItemIDResolution = new Dictionary<string, int>();
        ItemNameResolution = new List<string>();

        foreach(string i in files)//全アイテムを読み込み, ItemField, ItemIDResolution, ItemNameResolutionに追加
        {
            ItemProperty itemProperty = new ItemProperty();
            if (itemProperty.JsonImport(path, Path.GetFileNameWithoutExtension(i)))
            {
                int idtemp = ItemIDResolution.Count;
                ItemField.Add(itemProperty);
                ItemIDResolution.Add(itemProperty.Name, idtemp);//末尾に追加しIDは0からはじめ、追加前の長さをIDとした
                ItemNameResolution.Add(itemProperty.Name);
            }
        }
    }
}
