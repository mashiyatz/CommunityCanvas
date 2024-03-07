using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    public void ChangeToScene(int sceneID)
    { 
        SceneManager.LoadScene(sceneID);
    }

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.SetInt("NumAssets", 0);
        
    }
}
