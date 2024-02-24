using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class StateManager : MonoBehaviour
{
    public enum State { SELECT, PLACE, EDIT, VIEW }

    public GameObject selectUI;
    public GameObject editUI;
    public GameObject returnToSelectButton;
    public GenerateAssetFromSelection selectionGenerator;

    public ARSessionOrigin origin;
    public static ARSessionOrigin arSessionOrigin;
    public static Vector3 arSessionOriginPos;

    private static State _currentState;

    public GameObject activeObject;
    public float activeObjectStartHeight;

    public State CurrentState 
    {
        get {return _currentState; }
        set {
            if (value == State.PLACE)
            {
                selectUI.SetActive(false);
                returnToSelectButton.SetActive(true);
                editUI.SetActive(true);
                _currentState = value;
            } else if (value == State.EDIT)
            {
                editUI.SetActive(true);
                _currentState = value;
            } else if (value == State.SELECT)
            {
                selectUI.SetActive(true);
                editUI.SetActive(false);
                returnToSelectButton.SetActive(false);
                _currentState = value;
            } else if (value == State.VIEW)
            {
                selectUI.SetActive(false);
                editUI.SetActive(false);
                activeObject = null;
                _currentState = value;
            }
        }
    }

    void Start()
    {
        _currentState = State.SELECT;
        arSessionOrigin = origin;
        // originCameraTransform = originCamera;
    }

    // Update is called once per frame
    void Update()
    {
        arSessionOriginPos = origin.transform.position;
    }

    public void SetRotation(Slider slider)
    {
        activeObject.GetComponent<GeneratedObjectPositionUpdate>().rotationOffset =  Quaternion.Euler(0, slider.value, 0);
    }

    public void SetHeight(Slider slider)
    {
        activeObject.transform.position = new Vector3(0, activeObjectStartHeight + slider.value, 0);
    }

    public void ChangeStateTo(int nextState)
    {
        CurrentState = (State)nextState;
    }

    public static State GetCurrentState()
    {
        return _currentState;
    }
}
