using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public static void ChangeToScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }

    public static void ChangeToScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
