using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class CommentPlacement : MonoBehaviour
{
    [SerializeField]
    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    [SerializeField]
    private Camera arCamera;

    [SerializeField]
    private CommentCreator commentCreator;

    [SerializeField]
    private StateManager stateMachine;

    // Update is called once per frame
    void Update()
    {

        if (stateMachine.CurrentState != StateManager.State.PLACE) { return; }
        if (UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count > 0)
        {
            Ray ray = arCamera.ScreenPointToRay(UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches[0].screenPosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.CompareTag("Plane"))
                {
                    stateMachine.ChangeStateTo((int)StateManager.State.VIEW);
                    commentCreator.CreateStickyNote(hit.collider.transform.position);
                }
            }
        }
    }
}
