using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ToAssetLibrary()
    {
        SceneManager.LoadScene("02_SelectAssetFromLibraryPage");
    }

    public void ToAIGeneration()
    {
        SceneManager.LoadScene("03_AIGenPage");
    }

    public void ToSelectPage()
    {
        SceneManager.LoadScene("01_SelectPage");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
