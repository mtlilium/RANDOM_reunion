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
        int thisitemid = ItemIDResolution[itemname];
        LinkedList<string> thisAttribute = ItemField[thisitemid].Attribute;

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
        //それぞれ別のJsonファイルがあると仮定, staticなクラスはジェネリックできないため
        ItemField = JsonIO.JsonImport<List<ItemProperty>>(path, "ItemField");
        ItemIDResolution = JsonIO.JsonImport<Dictionary<string, int>>(path, "ItemIDResolution");
        ItemNameResolution = JsonIO.JsonImport<List<string>>(path, "ItemNameResolution");


    }
	
}
