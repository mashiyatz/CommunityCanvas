using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qualcomm.Snapdragon.Spaces.Samples;

public class ClickOnGenExamples : MonoBehaviour
{
    [SerializeField]
    private CubeManipulation target;

    [SerializeField]
    private GameObject[] premadeObjects;
    
    [SerializeField]
    private GameObject slide;
    [SerializeField]
    private GameObject swings;
    [SerializeField]
    private GameObject birdnest;
    [SerializeField]
    private GameObject dogpark;
    
    
    
    [SerializeField]
    private GameObject aiGenPanel;

    private Camera xrOriginCamera;
    private Vector3 startPosition;
    private Camera environmentCamera;
    private int lastIndex;

    void Start()
    {
        xrOriginCamera = XROriginHelper.GetXROriginCamera();
        environmentCamera = GameObject.Find("EnvCamera").GetComponent<Camera>();
    }

    public void SelectPremadeObject(int index)
    {
        lastIndex = index;
        target.cube = premadeObjects[index];
        target.meshRenderer = premadeObjects[index].GetComponent<MeshRenderer>();
        premadeObjects[index].SetActive(true);

        startPosition = premadeObjects[index].transform.position;
        // premadeObjects[index].transform.position = xrOriginCamera.transform.position + xrOriginCamera.transform.forward * 2.5f + xrOriginCamera.transform.up * -0.5f;

        if (environmentCamera != null)
        {
            environmentCamera.transform.position = startPosition - environmentCamera.transform.forward * 2.5f + environmentCamera.transform.up * 0.5f;
            environmentCamera.clearFlags = CameraClearFlags.SolidColor;
        }

        aiGenPanel.SetActive(false);
    }

    public void ReturnToGenerateMenu()
    {
        aiGenPanel.SetActive(true);
        // premadeObjects[lastIndex].transform.position = startPosition;

        if (environmentCamera != null)
        {
            environmentCamera.clearFlags = CameraClearFlags.Skybox;
        }
    }


    // deprecated
    public void SelectSlide()
    {
        target.cube = slide;
        target.meshRenderer = slide.GetComponent<MeshRenderer>();
        slide.SetActive(true);

        startPosition = slide.transform.position;
        slide.transform.position = xrOriginCamera.transform.position + xrOriginCamera.transform.forward * 2.5f + xrOriginCamera.transform.up * -0.5f;

        aiGenPanel.SetActive(false);

    }

    public void SelectSwings()
    {
        target.cube = swings;

        target.meshRenderer = swings.GetComponent<MeshRenderer>();
        swings.SetActive(true);
        aiGenPanel.SetActive(false);
    }

    public void SelectNest()
    {
        target.cube = birdnest;
        birdnest.SetActive(true);
        target.meshRenderer = birdnest.GetComponent<MeshRenderer>();
        aiGenPanel.SetActive(false);
    }

    public void SelectDog()
    {
        target.cube = dogpark;
        dogpark.SetActive(true);
        target.meshRenderer = dogpark.GetComponent<MeshRenderer>();
        aiGenPanel.SetActive(false);
    }

}
