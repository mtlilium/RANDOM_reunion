using System.Collections;
using System.Collections.Generic;
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
}
