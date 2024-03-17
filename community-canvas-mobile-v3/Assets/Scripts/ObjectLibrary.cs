using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectLibrary : MonoBehaviour
{
    public static ObjectLibrary Instance;
    public GameObject[] assets;
    public Transform scrollSelectionContent;
    public GameObject scrollSelectionObjectPrefab;

    private void Awake()
    {
        // start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        foreach (GameObject obj in assets)
        {
            GameObject go = Instantiate(scrollSelectionObjectPrefab, scrollSelectionContent);
            go.GetComponent<Image>().sprite = obj.GetComponent<SpawnedObjectUnity>().image;
            go.GetComponent<LibrarySelectionChoice>().modelPrefab = obj; 
            go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{obj.GetComponent<SpawnedObjectUnity>().cost:N0}";
        }
    }
}

// maybe move these someplace else

[Serializable]
public class SpawnedObject
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
}

[Serializable]
public class SpawnedObjectList
{
    public List<SpawnedObject> objectList = new();
}
