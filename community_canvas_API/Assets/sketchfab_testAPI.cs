using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO.Compression;
using UnityEngine.Networking;
using System.IO;
using GLTFast;
using UnityEngine.SceneManagement;

public class sketchfab_testAPI : MonoBehaviour
{
    public string Email;
    public string Password;
    public string ModelUIDToDownload;
    private string _keywords;
    public TMP_InputField keywordInputField;

    // Start is called before the first frame update
    void Start()
    {
        //check the scene
        Scene scene = SceneManager.GetActiveScene();
        Debug.Log("Active Scene name is: " + scene.name + "\nActive Scene index: " + scene.buildIndex);

        SketchfabAPI.GetAccessToken(Email, Password, (SketchfabResponse<SketchfabAccessToken> answer) =>
        {
            if (answer.Success)
            {
                //AccessToken = answer.Object.AccessToken;
                //logged in
                SketchfabAPI.AuthorizeWithAccessToken(answer.Object);
                
               // DownloadModel();
               
                
            }
            else
            {
                Debug.LogError(answer.ErrorMessage);
                Debug.Log(Email) ;
                Debug.Log(Password);
            }

        });
    }

    
    // Function to get downloadable model list
    private void GetDownloadableModelList()
    {
        UnityWebRequestSketchfabModelList.Parameters p = new UnityWebRequestSketchfabModelList.Parameters();
        p.downloadable = true;
        SketchfabAPI.GetModelList(p, ((SketchfabResponse<SketchfabModelList> _answer) =>
        {
            if (_answer.Success)
            {
                List<SketchfabModel> modelList = _answer.Object.Models;
                // Do something with the model list
                Debug.Log("get the model list");
            }
            else
            {
                Debug.LogError(_answer.ErrorMessage);
            }
        }));
    }

   
    public void SearchModelsByKeywords()
    {
        UnityWebRequestSketchfabModelList.Parameters p = new UnityWebRequestSketchfabModelList.Parameters();
        p.downloadable = true;
        _keywords = keywordInputField.text;
        Debug.Log(_keywords);
        SketchfabAPI.ModelSearch((SketchfabResponse<SketchfabModelList> _answer) =>
        {
            if (_answer.Success)
            {
                List<SketchfabModel> modelList = _answer.Object.Models;
                // Check if there are models in the list
                if (modelList.Count > 0)
                {
                    // For simplicity, let's assume we want to download the first model in the list
                    ModelUIDToDownload = modelList[0].Uid;
                    // Call function to download the model
                    DownloadModel(ModelUIDToDownload);
                    Debug.Log("search model, model in the list");
                }
                else
                {
                    Debug.Log("No models found.");
                }
            }
            else
            {
                Debug.LogError(_answer.ErrorMessage);
            }
        }, p, _keywords);
    }

    private void Import(SketchfabModel _model, string _downloadUrl)
    {
        UnityWebRequest downloadRequest = UnityWebRequest.Get(_downloadUrl);
        //for android
        string relpath = Application.persistentDataPath+"/Users/joycezheng/Desktop/textto3d_test/pathFolder";
        //for mac: w/o the applciation.persistendatapath, @"/users...
        //string relpath = @"/Users/joycezheng/Desktop/textto3d_test/pathFolder";

        SketchfabWebRequestManager.Instance.SendRequest(downloadRequest, (UnityWebRequest _request) => {

            Debug.Log("downloadrequest is" + downloadRequest.ToString());
        

            using (ZipArchive zipArchive = new ZipArchive(new MemoryStream(downloadRequest.downloadHandler.data), ZipArchiveMode.Read))
            {
                zipArchive.ExtractToDirectory(relpath);
            }


          
            GltfImport($"file://{Path.Combine(relpath, "scene.gltf")}", (GameObject _importedModel) =>
            {
               
                Debug.Log(_importedModel);

                //clean up the directory after importing
                CleanupPathFolder(relpath);
            });
                     

        });
    }

    private void CleanupPathFolder(string path)
    {
        DirectoryInfo directory = new DirectoryInfo(path);

        foreach (FileInfo file in directory.GetFiles())
        {
            file.Delete();
        }

        foreach (DirectoryInfo dir in directory.GetDirectories())
        {
            dir.Delete(true);
        }

        Debug.Log("Path folder cleaned up successfully.");
    }

    private async void GltfImport(string _gltfFilePath, Action<GameObject> _onModelImported)
    {
        GltfImport gltf = new GltfImport();
        //gltf.DefaultSceneIndex = 1;
        Debug.Log($"Default scene index is: {gltf.DefaultSceneIndex}");

        bool success = true;
        try
        {
            // Create a settings object and configure it accordingly
            var settings = new ImportSettings
            {
                GenerateMipMaps = true,
                AnisotropicFilterLevel = 3,
                NodeNameMethod = NameImportMethod.OriginalUnique
            };
            // Load the glTF and pass along the settings
           
            success = await gltf.Load(_gltfFilePath,settings);
        }
        catch (Exception ex)
        {
            success = false;
            Debug.Log(ex);
        }

        Debug.Log("so far so good");

        if (!success)
        {
            Debug.Log("not good");
            _onModelImported?.Invoke(null);
           
        
            return;
        }

        GameObject go = new GameObject("SketchfabModel");
        Debug.Log("go is" + go.ToString());
        success = await gltf.InstantiateMainSceneAsync(go.transform);
        //GameObject gogo = Instantiate(go);
        //Debug.Log($"{gogo.transform.position}");
        Debug.Log("192 is good");
 

        _onModelImported?.Invoke(go);
    }


    private void DownloadModel(string ModelUIDToDownload)
    {
        Debug.Log("Downloadmodel is executed");
        SketchfabAPI.GetModel(ModelUIDToDownload, (resp) =>
        {
            Debug.Log("Get model is executed");
            Debug.Log(resp);
            //Debug.Log("Resp Object:");
            //Debug.Log(resp.Object.ToString());

            //new try
            try
            {
                SketchfabAPI.GetGLTFModelDownloadUrl(resp.Object.Uid, (SketchfabResponse<string> _modelDownloadUrl) =>
                {
                    if (!_modelDownloadUrl.Success)
                    {
                        Debug.LogError(_modelDownloadUrl.ErrorMessage);

                        //_onModelImported?.Invoke(null);

                        return;
                    }
                    //Debug.Log(_modelDownloadUrl.Object);
                    Import(resp.Object, _modelDownloadUrl.Object);



                });

            }
            catch (Exception ex)
            {
                Debug.Log("Oh no!");
                Debug.LogException(ex, this);
            }
        });
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}