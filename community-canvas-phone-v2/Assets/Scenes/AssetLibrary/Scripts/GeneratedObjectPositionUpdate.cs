using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GeneratedObjectPositionUpdate : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private float distanceFromCamera = 4;
    public Quaternion rotationOffset = Quaternion.identity;

    void Start()
    {
        // consider MakeContentAppearAt
        transform.position = StateManager.arSessionOrigin.camera.transform.position + Vector3.forward * distanceFromCamera;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("click");
    }

    void Update()
    {
        if (StateManager.GetCurrentState() == StateManager.State.PLACE)
        {
            transform.position =
                new Vector3(StateManager.arSessionOrigin.camera.transform.position.x, transform.position.y, StateManager.arSessionOrigin.camera.transform.position.z) +
                new Vector3(StateManager.arSessionOrigin.camera.transform.forward.x, 0, StateManager.arSessionOrigin.camera.transform.forward.z) * distanceFromCamera;
            transform.LookAt(StateManager.arSessionOrigin.camera.transform.position);
            Quaternion currentRotation = transform.rotation;
            transform.rotation = currentRotation * rotationOffset;
        }
        else if (StateManager.GetCurrentState() == StateManager.State.EDIT)
        {
            ;
        }
        else if (StateManager.GetCurrentState() == StateManager.State.VIEW)
        {
            // Destroy(gameObject);
        }
    }

    
}
