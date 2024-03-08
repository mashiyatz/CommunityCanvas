using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Unity.XR.CoreUtils;
using System.IO;

#if UNITY_ANDROID
using UnityEngine.Android;
#endif

public class ARObjectGeneration : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnable;
    [SerializeField]
    private XROrigin origin;
    private SpawnedObjectList objectList;

    private ObjectLibrary objectLibrary;
 
    // make this variable global
    private string jsonPath;

    // create a Dictionary<string, Environment>(),
    // where Environment includes the models corresponding to that space
    // there are multiple environments corresponding to different users

    private void GenerateObjectsInAR()
    {
        print("Looking for object library...");
        objectLibrary = GameObject.Find("ObjectLibrary").GetComponent<ObjectLibrary>();
        if (objectLibrary == null) return;
        print("Found object library!");
        if (!File.Exists(jsonPath)) return;
        print("Loading object list...");
        objectList = JsonUtility.FromJson<SpawnedObjectList>(File.ReadAllText(jsonPath));
        if (objectList == null) return;

        print("Spawning objects into AR...");
        foreach (SpawnedObject obj in objectList.objectList)
        {
            Instantiate(objectLibrary.assets[obj.index], new Vector3(obj.x, obj.y, obj.z), new Quaternion(obj.qx, obj.qy, obj.qz, obj.qw), origin.TrackablesParent);
        }

    }

    void Start()
    {
        jsonPath = Application.persistentDataPath + "/SpawnObjectsData.json";

        #if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            // PERMISSION NOT AVAILABLE ON ANDROID, DISPLAY POPUP MESSAGE
        }
        #endif
    
        GenerateObjectsInAR();
    }

    public static void GenerateObjectsBasedOnQR(string qrName)
    {
        switch (qrName)
        {
            case "QR1":
                // better managed through library or db search?
                break;
            default:
                break;
        }
    }

}
