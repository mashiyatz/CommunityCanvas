using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class GenerateAssetFromSelection : MonoBehaviour
{
    public GameObject[] assets;
    public StateManager stateMachine;

    public void GenerateAsset(int index)
    {
        stateMachine.ChangeStateTo((int)StateManager.State.PLACE);
        GameObject go = Instantiate(assets[index], stateMachine.origin.trackablesParent);
        stateMachine.activeObject = go;
        stateMachine.activeObjectStartHeight = stateMachine.activeObject.transform.position.y;
    }
}
