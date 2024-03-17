using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using TMPro;

public class PlannerMain : MonoBehaviour
{
    public enum State { EXPLORE, PLACE, ADJUST }
    private State currentState;
    public PinchDetection pinchDetection;
    public TapDetection tapDetection;

    public Transform generatedAssetParent;
    public GameObject textPrompt;
    public ObjectLibrary objectLibrary;

    private SpawnedObjectList objectList;
    private string jsonPath;

    private int remainingBudget;

    [SerializeField]
    private EnvironmentParameters envParams;

    [SerializeField]
    private TextMeshProUGUI remainingBudgetText;

    public GameObject confirmPosButton;

    // for testing
    public SceneChangeManager sceneChangeManager;

    [SerializeField]
    private float dragCoefficient;


    void Start()
    {
        sceneChangeManager.ResetPlayerPrefs();
        currentState = State.EXPLORE;
        remainingBudget = envParams.GetBudget();

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
            File.Create(jsonPath).Dispose();
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
            textPrompt.SetActive(true);

        }
        else if ((State)stateID == State.EXPLORE && currentState == State.PLACE)
        {
            pinchDetection.enabled = true;
            tapDetection.enabled = false;
            StopCoroutine(WaitForRotation());
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

        tapDetection.selectedObject = go;
        SpawnedObjectUnity o = go.GetComponent<SpawnedObjectUnity>();
        o.SetIndex(index);
        objectList.objectList.Add(o.AssignTransformValues());

        remainingBudget -= o.cost;
        remainingBudgetText.text = $"Budget: ${remainingBudget:N0}";

        confirmPosButton.SetActive(true);
        StartCoroutine(WaitForRotation());
    }

    public IEnumerator WaitForRotation()
    {
        while (true)
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Moved)
                {
                    tapDetection.selectedObject.transform.Rotate(transform.up, -touch.deltaPosition.x / dragCoefficient, Space.World);
                }
            }
            yield return null;
        }
    }

    public void SerializeObjectListToJson()
    {
        string objectListJson = JsonUtility.ToJson(objectList, true);
        // Will overwrite existing text file https://learn.microsoft.com/en-us/dotnet/api/system.io.file.writealltext?view=net-8.0
        File.WriteAllText(jsonPath, objectListJson);
    }
}
