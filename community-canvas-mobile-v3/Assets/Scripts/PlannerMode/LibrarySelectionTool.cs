using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibrarySelectionTool : MonoBehaviour
{
    [SerializeField]
    private Main mainControl;

    [SerializeField]
    private PlannerMain plannerControl;

    [SerializeField]
    private ObjectLibrary library;

    public void InstantiateLibrarySelection(int index)
    {
        plannerControl.ChangeState(1);
        StartCoroutine(plannerControl.WaitForPlacement(library.assets[index]));
    }
}
