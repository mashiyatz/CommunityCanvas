using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Main : MonoBehaviour
{
    public enum State { EXPLORE, PLACE }
    private State currentState;
    public PinchDetection pinchDetection;
    public TapDetection tapDetection;

    public GameObject panelLibrary;
    public Transform generatedAssetParent;
    public GameObject textPrompt;

    void Start()
    {
        currentState = State.EXPLORE;
    }

    public void ChangeState(int stateID)
    {
        if ((State)stateID == State.PLACE && currentState == State.EXPLORE)
        {
            pinchDetection.enabled = false;
            tapDetection.enabled = true;
            panelLibrary.SetActive(false);
            textPrompt.SetActive(true);

        } else if ((State)stateID == State.EXPLORE && currentState == State.PLACE)
        {
            pinchDetection.enabled = true;
            tapDetection.enabled = false;
            textPrompt.SetActive(false);
            
        }
        currentState = (State)stateID;
    }

    public IEnumerator WaitForPlacement(GameObject asset)
    {
        tapDetection.isWaitingForPlacement = true;
        yield return new WaitForSeconds(0.5f);
        while (tapDetection.isWaitingForPlacement)
        {
            yield return null;
        }
        Instantiate(asset, tapDetection.GetObjectPosition(), tapDetection.GetObjectRotation(), generatedAssetParent);
        PlayerPrefs.SetInt("NumAssets", PlayerPrefs.GetInt("NumAssets") + 1);
        ChangeState((int)State.EXPLORE);
    }
}
