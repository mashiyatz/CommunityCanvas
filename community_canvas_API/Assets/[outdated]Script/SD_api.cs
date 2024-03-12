using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class myParams : MonoBehaviour
{
    public string apiKey = "OSkttBWG87llDS7PUONBFLswiKKhprRRqDDIrqzSdFdDLLrwjaSU5CO2zJUp";
    public string baseUrl = "https://stablediffusionapi.com/api/v3/txt_to_3d";
    public string prompt = "rose";
    public string steps = "64";
    public string guidance_scale = "20";
    public string frame_size = "256";
    public string output_type = "gif";

}

public class SD_api : MonoBehaviour
{
    //private string apiKey = "OSkttBWG87llDS7PUONBFLswiKKhprRRqDDIrqzSdFdDLLrwjaSU5CO2zJUp";
    //private string baseUrl = "https://stablediffusionapi.com/api/v3/txt_to_3d";


    //public IEnumerator Generate3DImage(string prompt, int steps, int guidanceScale, int frameSize, string outputType, string webhook, string trackId)
    //{
    //    //create variable from the class
    //    myParams x = new myParams();
    //    // Create request body
    //    Dictionary<string, string> requestBody = new Dictionary<string, string>();
    //    requestBody.Add("key", apiKey);
    //    requestBody.Add("prompt", "rose");
    //    requestBody.Add("steps", "64");
    //    requestBody.Add("guidance_scale", "20");
    //    requestBody.Add("frame_size", "256");
    //    requestBody.Add("output_type", "gif");
    //    //requestBody.Add("webhook", webhook);
    //    //requestBody.Add("track_id", trackId);

    //    // Convert request body to JSON
    //    foreach (string key in requestBody.Values)
    //    {
    //        Debug.Log(key);
    //    }
    //    //string jsonRequestBody = JsonUtility.ToJson(x);//jsonutility SUS
        
    //    //"{ \"field1\": 1, \"field2\": 2 }"
    //    string jsonRequestBody = "{\"apiKey\":\"OSkttBWG87llDS7PUONBFLswiKKhprRRqDDIrqzSdFdDLLrwjaSU5CO2zJUp\",\"baseUrl\":\"https://stablediffusionapi.com/api/v3/txt_to_3d\",\"prompt\":\"rose\",\"steps\":64,\"guidance_scale\":20,\"frame_size\":256,\"output_type\":\"gif\";}";

    //    Debug.Log(jsonRequestBody);

    //    // Create request headers
    //    Dictionary<string, string> requestHeaders = new Dictionary<string, string>();
    //    requestHeaders.Add("Content-Type", "application/json"); // Set Content-Type header

        

    //    // Create and send request
    //    using (UnityWebRequest request = UnityWebRequest.Post(baseUrl, jsonRequestBody, "application/json"))//any other types of post?
    //    {
    //        //foreach (KeyValuePair<string, string> header in requestHeaders)
    //        //{
    //        //    request.SetRequestHeader(header.Key, header.Value);
    //        //}

    //        yield return request.SendWebRequest();
    //        //yield return www.SendWebRequest();

    //        // Process response
    //        if (request.result != UnityWebRequest.Result.Success)
    //        {
    //            Debug.LogError("Error: " + request.error);
                
    //        }
    //        else
    //        {
    //            string jsonResponse = request.downloadHandler.text;
    //            Debug.Log("Response: " + jsonResponse);
                

    //            // Parse JSON response
    //            // Example parsing: you may need to adjust this based on the actual response structure
    //            Dictionary<string, object> response = JsonUtility.FromJson<Dictionary<string, object>>(jsonResponse);
    //            if (response.ContainsKey("status") && (string)response["status"] == "success")
    //            {
    //                string[] outputUrls = (string[])response["output"];
    //                foreach (string outputUrl in outputUrls)
    //                {
    //                    Debug.Log("Generated image URL: " + outputUrl);
    //                    // Handle the generated image URL as needed
    //                }
    //            }
    //            else
    //            {
    //                Debug.LogError("Error in response: " + jsonResponse);
    //            }
    //        }
        
    //}
    //}



   
}

