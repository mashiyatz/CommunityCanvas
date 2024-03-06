using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLibrary : MonoBehaviour
{
    [SerializeField]
    private GameObject[] assetLibrary;

    [SerializeField]
    private Main mainControl;

    public void InstantiateLibrarySelection(int index)
    {
        mainControl.ChangeState(1);
        StartCoroutine(mainControl.WaitForPlacement(assetLibrary[index]));
    }




}
