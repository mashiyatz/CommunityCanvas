using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibrarySelectionTool : MonoBehaviour
{
    [SerializeField]
    private Main mainControl;

    [SerializeField]
    private ObjectLibrary library;

    public void InstantiateLibrarySelection(int index)
    {
        mainControl.ChangeState(1);
        StartCoroutine(mainControl.WaitForPlacement(index, library.assets[index]));
    }
}
