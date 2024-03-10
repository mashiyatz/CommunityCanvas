using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedObjectUnity: MonoBehaviour
{
    public int cost;
    private SpawnedObject obj = new();

    public SpawnedObject AssignTransformValues()
    {
        obj.x = transform.position.x; obj.y = transform.position.y; obj.z = transform.position.z;
        obj.qx = transform.rotation.x; obj.qy = transform.rotation.y; obj.qz = transform.rotation.z; obj.qw = transform.rotation.w;
        return obj; 
    }

    // can we just simplify this by assigning the index to each model used
    public void SetIndex(int index)
    {
        obj.index = index;
    }
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


