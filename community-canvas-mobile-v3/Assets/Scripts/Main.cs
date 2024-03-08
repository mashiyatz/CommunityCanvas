using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class Main : MonoBehaviour
{
    public enum State { EXPLORE, PLACE }
    private State currentState;
    public PinchDetection pinchDetection;
    public TapDetection tapDetection;

    public GameObject panelLibrary;
    public Transform generatedAssetParent;
    public GameObject textPrompt;
    public ObjectLibrary objectLibrary;

    private SpawnedObjectList objectList;
    private string jsonPath;

    void Start()
    {
        currentState = State.EXPLORE;
       
        jsonPath = Application.persistentDataPath + "/SpawnObjectsData.json";
        if (File.Exists(jsonPath))
        {
            print("File already exists. Loading object list...");
            objectList = (SpawnedObjectList)JsonUtility.FromJson(File.ReadAllText(jsonPath), typeof(SpawnedObjectList));
            if (objectList != null)
            {
                foreach (SpawnedObject obj in objectList.objectList)
                {
                    Instantiate(objectLibrary.assets[obj.index], new Vector3(obj.x, obj.y, obj.z), new Quaternion(obj.qx, obj.qy, obj.qz, obj.qw), generatedAssetParent);
                }
            }

            // print(File.ReadAllText(jsonPath));
        }
        else
        {
            print("File does not exist. Creating...");
            File.Create(jsonPath);
        }

        objectList ??= new();
    }

    // for debugging
    public void CreateNewSpawnedObjectList()
    {
        objectList = new SpawnedObjectList();
    }

    public void ChangeState(int stateID)
    {
        if ((State)stateID == State.PLACE && currentState == State.EXPLORE)
        {
            pinchDetection.enabled = false;
            tapDetection.enabled = true;
            panelLibrary.SetActive(false);
            textPrompt.SetActive(true);

        } else if ((State)stateID == State.EXPLORE && currentState == State.PLACE)
        {
            pinchDetection.enabled = true;
            tapDetection.enabled = false;
            textPrompt.SetActive(false);
            
        }
        currentState = (State)stateID;
    }

    public IEnumerator WaitForPlacement(int index, GameObject asset)
    {
        tapDetection.isWaitingForPlacement = true;
        yield return new WaitForSeconds(0.5f);
        while (tapDetection.isWaitingForPlacement)
        {
            yield return null;
        }
        GameObject go = Instantiate(asset, tapDetection.GetObjectPosition(), tapDetection.GetObjectRotation(), generatedAssetParent);
        
        SpawnedObjectUnity o = go.GetComponent<SpawnedObjectUnity>();
        o.SetIndex(index);
        objectList.objectList.Add(o.AssignTransformValues());

        string objectJson = JsonUtility.ToJson(o, true);
        // string objectJson = JsonConvert.SerializeObject(o);
        print(objectJson);
        // PlayerPrefs.SetInt("NumAssets", PlayerPrefs.GetInt("NumAssets") + 1);
        ChangeState((int)State.EXPLORE);
    }

    public void SerializeObjectListToJson()
    {
        string objectListJson = JsonUtility.ToJson(objectList, true);
        // string objectListJson = JsonConvert.SerializeObject(objectList);
        // Will overwrite existing text file https://learn.microsoft.com/en-us/dotnet/api/system.io.file.writealltext?view=net-8.0
        File.WriteAllText(jsonPath, objectListJson);
    }
}
