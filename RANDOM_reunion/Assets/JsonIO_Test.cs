using System.Collections;
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
    
    Sample person;
	// Use this for initialization
	void Start () {
        Sample tmp = new Sample("aiueo",3);
        JsonIO.JsonExport<Sample>(tmp, "Assets", "jsonIO_Test");
    }
	
	// Update is called once per frame
	void Update () {
        
	}
}
