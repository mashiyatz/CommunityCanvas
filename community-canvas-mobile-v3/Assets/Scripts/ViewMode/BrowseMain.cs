using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BrowseMain : MonoBehaviour
{
    [SerializeField]
    private GameObject placeholderPrefab;
    [SerializeField]
    private Transform generatedObjectParent;
/*    [SerializeField]
    private MeshRenderer platformMeshRenderer;*/
/*    [SerializeField]
    private Material[] colors;*/
/*    [SerializeField]
    private TextMeshProUGUI costTextbox;*/
    [SerializeField]
    private TextMeshProUGUI titleTextbox;
    [SerializeField]
    private TextMeshProUGUI designerTextbox;

    // preset names
    [SerializeField]
    private string[] titles;
    [SerializeField]
    private string[] designerNames;

    private bool isStart = true;
    private bool isCoroutineRunning = false;

    void Start()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(GenerateNewArrangement());
    }

    IEnumerator DeleteChildren()
    {
        isCoroutineRunning = true;
        while (generatedObjectParent.childCount > 0)
        {
            Destroy(generatedObjectParent.GetChild(0).gameObject);
            yield return null; 
        }
        StartCoroutine(GenerateNewArrangement());
    }

    IEnumerator GenerateNewArrangement()
    {
        int randomAmt = UnityEngine.Random.Range(1, 10);
        int randomIndex = UnityEngine.Random.Range(0, 5);
        string randomName = designerNames[randomIndex];
        string randomTitle = titles[randomIndex];

        for (int i = 0; i < randomAmt; i++)
        {
            Vector3 randomPos = new(UnityEngine.Random.Range(-25, -1), 0.5f, UnityEngine.Random.Range(-3, 6));
            Instantiate(placeholderPrefab, randomPos, Quaternion.identity, generatedObjectParent);
            yield return null;
        }
        // costTextbox.text = $"Redesign Cost: {(randomAmt * 10000):N0}";

        try
        {
            titleTextbox.text = randomTitle;
            designerTextbox.text = $"Designer: {randomName}{Environment.NewLine}Cost: {(randomAmt * 10000):N0}";
        }
        catch { }

        if (isStart) { ToggleCommunityObjects(false); isStart = false; }
        isCoroutineRunning = false;
        
    }

    public void CreateRandomArrangement()
    {
        if (!isCoroutineRunning)
        {
            StartCoroutine(DeleteChildren());
            // platformMeshRenderer.material = colors[UnityEngine.Random.Range(0, colors.Length)];
        }
    }

    public void ToggleCommunityObjects(bool toggle)
    {
        for (int i = 0; i < generatedObjectParent.transform.childCount; i++)
        {
            generatedObjectParent.transform.GetChild(i).gameObject.SetActive(toggle);
        }
    }

    void Update()
    {
        
    }
}
