﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;

[DataContract]
internal class Sample
{
    public Sample(string _name, int _age)
    {
        name = _name;
        age = _age;
    }
    [DataMember]
    internal string name;

    [DataMember]
    internal int age;
}

public class JsonIO_Test : MonoBehaviour {
    
    Sample tmp1;
    // Use this for initialization
    void Start()
    {
        Sample tmp2 = new Sample("Yamaguchi", 3);
        JsonIO.JsonExport<Sample>(tmp2, "Assets", "jsonIO_Test");

        tmp1 = JsonIO.JsonImport<Sample>("Assets", "jsonIO_Test");
        Debug.Log("age:" + tmp1.age + ",name:" + tmp1.name);
        
    }
	
	// Update is called once per frame
	void Update () {
        
	}
}
