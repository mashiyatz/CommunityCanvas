using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

#if UNITY_ANDROID
using UnityEngine.Android;
#endif

public class ARObjectGeneration : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnable;
    [SerializeField]
    private ARSessionOrigin origin;

    // create a Dictionary<string, Environment>(),
    // where Environment includes the models corresponding to that space
    // there are multiple environments corresponding to different users

    void Start()
    {
        for (int i = 0; i < PlayerPrefs.GetInt("NumAssets"); i++)
        {
            Instantiate(spawnable, transform.position + new Vector3(Random.Range(0, 10), 0, Random.Range(0, 10)), Quaternion.identity, origin.trackablesParent);
        }

        #if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            // PERMISSION NOT AVAILABLE ON ANDROID, DISPLAY POPUP MESSAGE
        }
        #endif
    }

    public static void GenerateObjectsBasedOnQR(string qrName)
    {
        switch (qrName)
        {
            case "QR1":
                // better managed through library or db search?
                break;
            default:
                break;
        }
    }

}
