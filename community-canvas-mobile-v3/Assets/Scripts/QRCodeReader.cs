using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public class QRCodeReader : MonoBehaviour
{
    private ARTrackedImage triggerImage;
    private ARTrackedImageManager trackedImageManager;
    
    // need to set up a database

    void Start()
    {
        triggerImage = GetComponent<ARTrackedImage>();
        trackedImageManager = transform.parent.parent.GetComponent<ARTrackedImageManager>();
        GenerateBasedOnImage(triggerImage);
    }

    void GenerateBasedOnImage(ARTrackedImage trackedImage)
    {
        switch (trackedImage.referenceImage.name)
        {
            case "QR1":
                // 1. set up scene corresponding to QR1
                // 2. turn off tracked image manager
                // 3. destroy object 
                break;
            case "QR2":
                break;
            default:
                break;
        }
    }
}
