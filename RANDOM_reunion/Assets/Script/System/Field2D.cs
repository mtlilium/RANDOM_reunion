using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;

[DataContract]
public class Field2D<T>//単純な2次元フィールド
{
    [DataMember]
    public int X{get;set;}//第1インデクサーのサイズ. set時にはfieldを拡大,縮小(縮小の場合はDebug.Logでの表示)すること.

    [DataMember]
    public int Y{get;set;}//第2インデクサーのサイズ. set時にはfieldを拡大,縮小(縮小の場合はDebug.Logでの表示)すること.

    [DataMember]
    public T[][] field;//実際に値が格納されるフィールド.

    //コンストラクタ. 
    public Field2D() { }
    public Field2D(int x,int y)//X,Y,fieldをサイズを適用し初期化すること.
    {
        X = x;
        Y = y;
        field = new T[X][];
        for(int i = 0; i < X; i++)
        {
            field[i] = new T[Y];
        }
    }
    public void Foreach(Action<T> action)
    {
        for (int i = 0; i < X; i++)
        {
            for (int j = 0; j < Y; j++)
            {
                action(field[i][j]);
            }
        }
    }

    public override string ToString()
    {
        string str = "[";
        for (int i = 0; i < X; i++)
        {
            str += '[';
            for (int j = 0; j < Y; j++)
            {
                str += field[i][j].ToString();
                if (j + 1 != Y)
                    str += ',';
            }
            str += ']';
            if (i + 1 != X)
                str += ',';
        }
        str += ']';
        return str;
    }
}