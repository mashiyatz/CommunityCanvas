//using UnityEngine;
//using UnityEngine.Networking;
//using System.Collections;
//using System.Collections.Generic;

//public class text_to_3d_controller : MonoBehaviour
//{
//    private string apiKey = "msy_rkCy9GMKGyFy9mFJ4SObMn4hn2TiGNkMI8ml";
//    private string baseUrl = "https://api.meshy.ai/v1/text-to-3d/018a210d-8ba4-705c-b111-1f1776f7f578";



//    public void CreateTextTo3DTask(string objectPrompt, string stylePrompt, bool enablePBR, string artStyle, string negativePrompt, string resolution)
//    {
//        StartCoroutine(PostTextTo3DTask(objectPrompt, stylePrompt, enablePBR, artStyle, negativePrompt, resolution));
//        Debug.Log("create text to 3d exceuted");
//    }

   

//    IEnumerator PostTextTo3DTask(string objectPrompt, string stylePrompt, bool enablePBR, string artStyle, string negativePrompt, string resolution)
//    {
//        string url = baseUrl;
//        string json = JsonUtility.ToJson(new TextTo3DRequest(objectPrompt, stylePrompt, enablePBR, artStyle, negativePrompt, resolution));

//        Debug.Log("Request JSON: " + json); // Log request JSON data

//        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(url, json))
//        {
//            request.SetRequestHeader("Authorization", "Bearer " + apiKey);
//            request.SetRequestHeader("Content-Type", "application/json");

//            yield return request.SendWebRequest();

//            if (request.result != UnityWebRequest.Result.Success)
//            {
//                Debug.LogError("Request failed: " + request.error); // Log error message
//            }
//            else
//            {
//                Debug.Log("Text to 3D Task created successfully!");
//                string taskId = JsonUtility.FromJson<TextTo3DResponse>(request.downloadHandler.text).result;
//                StartCoroutine(GetTextTo3DTask(taskId));
//            }
//        }
//    }

//    IEnumerator GetTextTo3DTask(string taskId)
//    {
//        string url = baseUrl + "/" + taskId;

//        using (UnityWebRequest request = UnityWebRequest.Get(url))
//        {
//            request.SetRequestHeader("Authorization", "Bearer " + apiKey);

//            yield return request.SendWebRequest();

//            if (request.result != UnityWebRequest.Result.Success)
//            {
//                Debug.Log(request.error);
//            }
//            else
//            {
//                Debug.Log("Text to 3D Task retrieved successfully!");
//                Debug.Log(request.downloadHandler.text);
//                // Parse the response and use the data as needed
//            }
//        }
//    }
//}

//[System.Serializable]
//public class TextTo3DRequest
//{
//    public string object_prompt;
//    public string style_prompt;
//    public bool enable_pbr;
//    public string art_style;
//    public string negative_prompt;
//    public string resolution;

//    public TextTo3DRequest(string objectPrompt, string stylePrompt, bool enablePBR, string artStyle, string negativePrompt, string resolution)
//    {
//        object_prompt = objectPrompt;
//        style_prompt = stylePrompt;
//        enable_pbr = enablePBR;
//        art_style = artStyle;
//        negative_prompt = negativePrompt;
//        this.resolution = resolution;
//    }
//}

//[System.Serializable]
//public class TextTo3DResponse
//{
//    public string result;
//}
