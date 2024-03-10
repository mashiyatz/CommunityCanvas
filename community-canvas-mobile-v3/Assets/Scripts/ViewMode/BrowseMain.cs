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
    [SerializeField]
    private MeshRenderer platformMeshRenderer;
    [SerializeField]
    private Material[] colors;
    [SerializeField]
    private TextMeshProUGUI costTextbox;

    private bool isCoroutineRunning = false;

    void Start()
    {
        
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
        int randomAmt = UnityEngine.Random.Range(1, 5);
        for (int i = 0; i < randomAmt; i++)
        {
            Vector3 randomPos = new(UnityEngine.Random.Range(-10, -1), 0.5f, UnityEngine.Random.Range(-3, 2));
            Instantiate(placeholderPrefab, randomPos, Quaternion.identity, generatedObjectParent);
            yield return null;
        }
        costTextbox.text = $"Redesign Cost: {(randomAmt * 1000):N0}";
        isCoroutineRunning = false;
        
    }

    public void CreateRandomArrangement()
    {
        if (!isCoroutineRunning)
        {
            StartCoroutine(DeleteChildren());
            platformMeshRenderer.material = colors[UnityEngine.Random.Range(0, colors.Length)];
        }
    }

    void Update()
    {
        
    }
}
