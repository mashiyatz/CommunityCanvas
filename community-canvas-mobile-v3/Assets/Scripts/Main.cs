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

    public SpawnedObjectList objectList;
    private string jsonPath;

    void Start()
    {
        currentState = State.EXPLORE;
        objectList = new SpawnedObjectList();

        jsonPath = Application.persistentDataPath + "/SpawnObjectsData.json";
        if (File.Exists(jsonPath))
        {
            objectList = JsonUtility.FromJson<SpawnedObjectList>(File.ReadAllText(jsonPath));
        }
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
        o.index = index;
        o.AssignTransformValues();
        objectList.objectList.Add(o);
         
        // PlayerPrefs.SetInt("NumAssets", PlayerPrefs.GetInt("NumAssets") + 1);
        ChangeState((int)State.EXPLORE);
    }

    public void SerializeObjectListToJson()
    {
        string objectListJson = JsonUtility.ToJson(objectList);
        // Will overwrite existing text file https://learn.microsoft.com/en-us/dotnet/api/system.io.file.writealltext?view=net-8.0
        File.WriteAllText(jsonPath, objectListJson);
    }
}
