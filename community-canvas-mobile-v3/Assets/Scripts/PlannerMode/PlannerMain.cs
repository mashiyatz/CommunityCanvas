using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using TMPro;
using System;
using UnityEngine.UI;

public class PlannerMain : MonoBehaviour
{
    public enum State { EXPLORE, PLACE, ADJUST }
    private State currentState;
    public PinchDetection pinchDetection;
    public TapDetection tapDetection;

    public Transform generatedAssetParent;
    public GameObject textPrompt;
    public ObjectLibrary objectLibrary;
    
    public GameObject infoPanel;
    public Image infoPanelImage;
    public TextMeshProUGUI infoPanelText;

    public GameObject scrollPanel;

    private SpawnedObjectList objectList;
    private string jsonPath;

    private int remainingBudget;
    public SpawnedObjectUnity selectedObjectSpawn;
    public List<SpawnedObjectUnity> spawnedObjectsInScene = new();

    public Button[] panelButtons;

    [SerializeField]
    private EnvironmentParameters envParams;

    [SerializeField]
    private TextMeshProUGUI remainingBudgetText;

    public GameObject confirmPosButton;

    // for testing
    public SceneChangeManager sceneChangeManager;

    [SerializeField]
    private float dragCoefficient;

    private Coroutine currentCoroutine;

    public BrowseMain browse;

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

    public void CreateNewSpawnedObjectList()
    {
        objectList = new SpawnedObjectList();
    }

    public void ToggleObjectsVisibility(bool toggle)
    {
        foreach (SpawnedObjectUnity spawn in spawnedObjectsInScene)
        {
            spawn.gameObject.SetActive(toggle);
        }
    }

    public void ResetUI()
    {
        pinchDetection.enabled = false;
        tapDetection.enabled = false;
        textPrompt.SetActive(false);
        pinchDetection.enabled = false;
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        confirmPosButton.SetActive(false);
        infoPanel.SetActive(false);
        browse.ToggleCommunityObjects(false);
        ToggleObjectsVisibility(true);

        foreach (Button btn in panelButtons) btn.enabled = false;
    }

    public void ChangeState(int stateID)
    {
        ResetUI();

        if ((State)stateID == State.PLACE)
        {
            tapDetection.enabled = true;
            textPrompt.SetActive(true);
            tapDetection.isWaitingForPlacement = true;
        }
        else if ((State)stateID == State.EXPLORE)
        {
            pinchDetection.enabled = true;
            tapDetection.enabled = true;
            selectedObjectSpawn = null;
            foreach (Button btn in panelButtons) btn.enabled = true;
        } else if ((State)stateID == State.ADJUST)
        {
            infoPanel.SetActive(true);
            tapDetection.enabled = true;
            infoPanelImage.sprite = selectedObjectSpawn.image;
            infoPanelText.text = $"{selectedObjectSpawn.cost:N0}";

            confirmPosButton.SetActive(true);
            tapDetection.isWaitingForPlacement = true;
        }
        currentState = (State)stateID;
    }

    public void StartWaitForPlacement(GameObject go)
    {
        StartCoroutine(WaitForFirstPlacement(go));
    }

    public IEnumerator WaitForFirstPlacement(GameObject asset)
    {
        yield return new WaitForSeconds(0.5f);
        while (tapDetection.isWaitingForPlacement)
        {
            yield return null;
        }
        GameObject go = Instantiate(asset, tapDetection.GetObjectPosition(), tapDetection.GetObjectRotation(), generatedAssetParent);
        tapDetection.selectedObject = go;
        selectedObjectSpawn = go.GetComponent<SpawnedObjectUnity>();
        // objectList.objectList.Add(selectedObjectSpawn.AssignTransformValues());
        spawnedObjectsInScene.Add(selectedObjectSpawn); 

        remainingBudget -= selectedObjectSpawn.cost;
        remainingBudgetText.text = $"Budget: ${remainingBudget:N0}";

        ChangeState(2);
    }

    public void RotateObjectWithSlider(Slider slider)
    {
        selectedObjectSpawn.transform.rotation = Quaternion.Euler(selectedObjectSpawn.transform.eulerAngles.x, slider.value, selectedObjectSpawn.transform.eulerAngles.z);
    }

    public void ResizeObjectWithSlider(Slider slider)
    {
        selectedObjectSpawn.transform.localScale = Vector3.one * slider.value;
    }

    public void AdjustObjectPosition()
    {
        if (selectedObjectSpawn == null) return;
        if (currentState == State.ADJUST) selectedObjectSpawn.transform.position = tapDetection.GetObjectPosition();
    }

    public void ObjectSelection(SpawnedObjectUnity obj)
    {
        // if (currentState == State.ADJUST) return;
        if (currentState == State.EXPLORE)
        {
            selectedObjectSpawn = obj;
            ChangeState(2);
        }

    }

/*    public IEnumerator WaitForRotation()
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
    }*/

    public void SerializeObjectListToJson()
    {
        foreach (SpawnedObjectUnity spawn in spawnedObjectsInScene)
        {
            objectList.objectList.Add(spawn.AssignTransformValues());
        }
        string objectListJson = JsonUtility.ToJson(objectList, true);
        // Will overwrite existing text file https://learn.microsoft.com/en-us/dotnet/api/system.io.file.writealltext?view=net-8.0
        File.WriteAllText(jsonPath, objectListJson);
    }
}
