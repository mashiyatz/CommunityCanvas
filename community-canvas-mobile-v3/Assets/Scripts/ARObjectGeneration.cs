using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARObjectGeneration : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnable;
    [SerializeField]
    private ARSessionOrigin origin;

    void Start()
    {
        for (int i = 0; i < PlayerPrefs.GetInt("NumAssets"); i++)
        {
            Instantiate(spawnable, transform.position + new Vector3(Random.Range(0, 10), 0, Random.Range(0, 10)), Quaternion.identity, origin.trackablesParent);
        }    
    }

}
