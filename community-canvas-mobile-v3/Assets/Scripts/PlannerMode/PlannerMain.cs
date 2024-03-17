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
    private SpawnedObjectUnity selectedObject;

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

    public void ResetUI()
    {
        pinchDetection.enabled = false;
        tapDetection.enabled = false;
        textPrompt.SetActive(false);
        pinchDetection.enabled = false;
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        confirmPosButton.SetActive(false);
        infoPanel.SetActive(false);
    }

    public void ChangeState(int stateID)
    {
        ResetUI();
        if ((State)stateID == State.PLACE)
        {
            tapDetection.enabled = true;
            textPrompt.SetActive(true);
        }
        else if ((State)stateID == State.EXPLORE)
        {
            pinchDetection.enabled = true;
        } else if ((State)stateID == State.ADJUST)
        {
            infoPanel.SetActive(true);
            // tapDetection.enabled = true;
            infoPanelImage.sprite = selectedObject.image;
            infoPanelText.text = $"{selectedObject.cost:N0}";

            confirmPosButton.SetActive(true);
            currentCoroutine = StartCoroutine(WaitForReplace());
        }
        currentState = (State)stateID;
    }

    public void StartWaitForPlacement(GameObject go)
    {
        StartCoroutine(WaitForPlacement(go));
    }

    public IEnumerator WaitForPlacement(GameObject asset)
    {
        tapDetection.isWaitingForPlacement = true;
        
        yield return new WaitForSeconds(0.5f);
        while (tapDetection.isWaitingForPlacement)
        {
            yield return null;
        }
        GameObject go = Instantiate(asset, tapDetection.GetObjectPosition(), tapDetection.GetObjectRotation(), generatedAssetParent);
        tapDetection.selectedObject = go;
        selectedObject = go.GetComponent<SpawnedObjectUnity>();
        objectList.objectList.Add(selectedObject.AssignTransformValues());

        remainingBudget -= selectedObject.cost;
        remainingBudgetText.text = $"Budget: ${remainingBudget:N0}";

        ChangeState(2);
    }

    public void RotateObjectWithSlider(Slider slider)
    {
        selectedObject.transform.rotation = Quaternion.Euler(selectedObject.transform.eulerAngles.x, slider.value, selectedObject.transform.eulerAngles.z);
    }

    public void ResizeObjectWithSlider(Slider slider)
    {
        selectedObject.transform.localScale = Vector3.one * slider.value;
    }

    public IEnumerator WaitForReplace()
    {
        while (true)
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hitInfo))
                {
                    if (hitInfo.collider.CompareTag("Surface"))
                    {
                        selectedObject.transform.position = hitInfo.point;
                    }
                }
            }
            yield return null;
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
        string objectListJson = JsonUtility.ToJson(objectList, true);
        // Will overwrite existing text file https://learn.microsoft.com/en-us/dotnet/api/system.io.file.writealltext?view=net-8.0
        File.WriteAllText(jsonPath, objectListJson);
    }
}
