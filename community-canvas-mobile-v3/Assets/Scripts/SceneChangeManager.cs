using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SceneChangeManager : MonoBehaviour
{
    public void ChangeToScene(int sceneID)
    { 
        SceneManager.LoadScene(sceneID);
    }

    public void ResetPlayerPrefs()
    {
        // PlayerPrefs.SetInt("NumAssets", 0);
        string jsonPath = Application.persistentDataPath + "/SpawnObjectsData.json";
        File.Delete(jsonPath);
    }
}
