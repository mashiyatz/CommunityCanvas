using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class TapDetection : MonoBehaviour
{
    private TouchControls controls;
    public bool isWaitingForPlacement = false;
    private Vector3 objectPos;
    private Quaternion objectRot;
    public GameObject selectedObject;

    private void Awake()
    {
        controls = new TouchControls();
    }
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Start()
    {
        controls.Touch.PrimaryTouchContact.started += _ => PlaceObject();
    }

    private void PlaceObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(controls.Touch.PrimaryFingerPosition.ReadValue<Vector2>());

        if (Physics.Raycast(ray, out RaycastHit hitInfo) ) { 
            if (hitInfo.collider.CompareTag("Surface")) {
                if (!isWaitingForPlacement) return;
                isWaitingForPlacement = false;
                objectPos = hitInfo.point;
                objectRot = Quaternion.FromToRotation(hitInfo.transform.up, hitInfo.normal);
            } else if (hitInfo.collider.CompareTag("Model"))
            {
                selectedObject = hitInfo.collider.gameObject;
            }
        }
    }

    public Vector3 GetObjectPosition()
    {
        return objectPos;
    }

    public Quaternion GetObjectRotation() 
    {
        return objectRot;
    }
}
