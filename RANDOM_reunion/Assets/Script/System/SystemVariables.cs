using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;

public static class SystemVariables//システム関係の変数を司る静的クラス.
{
    public static string RootPath { get; private set; } = Application.dataPath;//Wiki参照
    public static Sprite[] SpriteList;//スプライトのリスト
    public static GameObject MapChipPrefab;//マップチップのプレハブ

    public static void CopiedFrom(SystemScript scr)//scrをコピー
    {

    }
}