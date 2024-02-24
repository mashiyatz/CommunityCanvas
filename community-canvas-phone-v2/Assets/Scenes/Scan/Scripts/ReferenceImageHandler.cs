using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ReferenceImageHandler : MonoBehaviour
{
    [SerializeField]
    private ARTrackedImageManager trackedImageManager;
    [SerializeField]
    private GameObject scanImagePrompt;
    [SerializeField]
    private GameObject menu;

    private void OnEnable() => trackedImageManager.trackedImagesChanged += OnChanged;
    private void OnDisable() => trackedImageManager.trackedImagesChanged -= OnChanged;

    private void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        if (eventArgs.added.Count > 0)
        {
            scanImagePrompt.SetActive(false);
            menu.SetActive(true);
        } else
        {
            scanImagePrompt.SetActive(true);
            menu.SetActive(false);
        }
    }
}
