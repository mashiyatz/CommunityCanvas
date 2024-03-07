using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

[Serializable]
public class SpawnedObjectUnity: MonoBehaviour 
{
    // index in library (which I need to make)
    public int index;
    
    // position (Vector3)
    public float x;
    public float y;
    public float z;

    // rotation (Quaternion)
    public float qw;
    public float qx;
    public float qy;
    public float qz;

    public void AssignTransformValues()
    {
        x = transform.position.x; y = transform.position.y; z = transform.position.z;
        qw = transform.rotation.w; qx = transform.rotation.x; qy = transform.rotation.y; qz = transform.rotation.z;
    }
}

[Serializable]
public class SpawnedObjectList
{
    public List<SpawnedObjectUnity> objectList = new();
}

// See: https://docs.unity3d.com/2020.1/Documentation/Manual/JSONSerialization.html
// See also: https://prasetion.medium.com/saving-data-as-json-in-unity-4419042d1334
// See also (on encrypting): https://stackoverflow.com/questions/72464659/best-way-to-json-encrypting-in-unity
// Can simply use: string json = JsonUtility.ToJson(myObject);
// For tracking many objects: List<string> listOfJSON; // append to list
// Docs say Monobehavior subclasses are also okay: JsonUtility.FromJsonOverwrite(json, myObject);
// Use this for converting back into AR scene.

/*namespace Data
{
    public class SpawnedObject
    {
        [JsonProperty] public int index;
        [JsonProperty] public float x;
        [JsonProperty] public float y;
        [JsonProperty] public float z;
        [JsonProperty] public float qw;
        [JsonProperty] public float qx;
        [JsonProperty] public float qy;
        [JsonProperty] public float qz;
    }
}*/


