using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class TapInteraction : MonoBehaviour
{
    [SerializeField]
    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    [SerializeField]
    private Camera arCamera;

    [SerializeField]
    private StateManager stateMachine;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (stateMachine.CurrentState != StateManager.State.VIEW) { return; }
        if (UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count > 0)
        {
            Ray ray = arCamera.ScreenPointToRay(UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches[0].screenPosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.CompareTag("Generated"))
                {
                    stateMachine.ChangeStateTo((int)StateManager.State.EDIT);
                    stateMachine.activeObject = hit.collider.gameObject;
                }
            }
        }
    }
}
