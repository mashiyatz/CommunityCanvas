using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpColorOverTime : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField]
    private Color startColor;
    [SerializeField]
    private Color endColor;

    void Start()
    {
        mainCamera = GetComponent<Camera>(); 
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.backgroundColor = Color.Lerp(startColor, endColor, Mathf.Pow(Mathf.Sin(Time.time / 8), 2));
    }
}
