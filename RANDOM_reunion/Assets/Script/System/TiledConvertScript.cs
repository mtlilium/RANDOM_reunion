using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;

/// <summary>
/// タイルマップのレイヤーのデータを管理するクラス
/// </summary>
[DataContract]
public sealed class LayerData
{
    //====================================================================================
    // 変数(DataMember)
    //====================================================================================
    [DataMember] 
    private long[] data = null;

    [DataMember] 
    private int height = 0;

    [DataMember]
    private int id=0;

    [DataMember] 
    private string name = string.Empty;

    [DataMember] 
    private int opacity = 0;

    [DataMember] 
    private string type = string.Empty;

    [DataMember] 
    private bool visible = false;

    [DataMember] 
    private int width = 0;

    [DataMember] 
    private int x = 0;

    [DataMember] 
    private int y = 0;

    //====================================================================================
    // プロパティ
    //====================================================================================
    [IgnoreDataMember] 
    public long[] Data { get { return data; } }

    [IgnoreDataMember] 
    public int Height { get { return height; } }

    [IgnoreDataMember] 
    public string Name { get { return name; } }

    [IgnoreDataMember] 
    public int Opacity { get { return opacity; } }

    [IgnoreDataMember] 
    public string Type { get { return type; } }

    [IgnoreDataMember] 
    public bool Visible { get { return visible; } }

    [IgnoreDataMember] 
    public int Width { get { return width; } }

    [IgnoreDataMember] 
    public int X { get { return x; } }

    [IgnoreDataMember] 
    public int Y { get { return y; } }

}

/// <summary>
/// タイルマップのタイルセットのデータを管理するクラス
/// </summary>
[DataContract]
public sealed class TilesetData
{
    //====================================================================================
    // 変数(DataMember)
    //====================================================================================
    [DataMember] 
    private int firstgid = 0;

    [DataMember]
    private string source = string.Empty;

    //====================================================================================
    // プロパティ
    //====================================================================================
    [IgnoreDataMember] 
    public int FirstGId { get { return firstgid; } }

    [IgnoreDataMember] 
    public string Source { get { return source; } }

}

/// <summary>
/// タイルマップのデータを管理するクラス
/// </summary>
[DataContract]
public sealed class TileMapData
{
    //====================================================================================
    // 変数(DataMember)
    //====================================================================================
    [DataMember]
    private int height = 0;

    [DataMember] 
    private bool infinite = false;

    [DataMember] 
    private LayerData[] layers = null;

    [DataMember]
    private int nextlayerid = 0;

    [DataMember] 
    private int nextobjectid = 0;

    [DataMember] 
    private string orientation = string.Empty;

    [DataMember] 
    private string renderorder = string.Empty;

    [DataMember] 
    private string tiledversion = string.Empty;
    
    [DataMember] 
    private int tileheight = 0;

    [DataMember] 
    private TilesetData[] tilesets = null;

    [DataMember] 
    private int tilewidth = 0;

    [DataMember] 
    private string type = string.Empty;

    [DataMember] 
    private float version = 0;

    [DataMember] 
    private int width = 0;

    //====================================================================================
    // プロパティ
    //====================================================================================
    [IgnoreDataMember] 
    public int Height { get { return height; } }

    [IgnoreDataMember] 
    public bool Infinite { get { return infinite; } }

    [IgnoreDataMember] 
    public LayerData[] Layers { get { return layers; } }

    [IgnoreDataMember] 
    public int NextObjectId { get { return nextobjectid; } }

    [IgnoreDataMember] 
    public string Orientation { get { return orientation; } }

    [IgnoreDataMember] 
    public string RenderOrder { get { return renderorder; } }

    [IgnoreDataMember] 
    public string TiledVersion { get { return tiledversion; } }

    [IgnoreDataMember] 
    public int TileHeight { get { return tileheight; } }

    [IgnoreDataMember] 
    public TilesetData[] Tilesets { get { return tilesets; } }

    [IgnoreDataMember] 
    public int TileWidth { get { return tilewidth; } }

    [IgnoreDataMember] 
    public string Type { get { return type; } }

    [IgnoreDataMember] 
    public float Version { get { return version; } }

    [IgnoreDataMember] 
    public int Width { get { return width; } }
}
