using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static System.Net.WebRequestMethods;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class SD_api_New : MonoBehaviour
{
    // Start is called before the first frame update
    private string baseUrl = "https://stablediffusionapi.com/api/v3/txt_to_3d";
    private string jsonResponse = "";
    private Dictionary<string, object> response = new Dictionary<string, object>();

    void Start()
    {
        StartCoroutine(Upload());
        Debug.Log("new script running");
    }

    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        form.AddField("key", "yQ1LIHwbovSqwLOOb3sfyIvEVxEwl3VER7L3RefpEJ56CHdrFUt9UBAMNY07");
        form.AddField("prompt", "roses");
        form.AddField("guidance_scale", 20);
        form.AddField("frame_size", 256);
        form.AddField("steps", 20);
        form.AddField("output_type", "gif");

        using (UnityWebRequest www = UnityWebRequest.Post(baseUrl, form))
        {
            yield return www.SendWebRequest();


            if (www.result != UnityWebRequest.Result.Success)
            {
                //Debug.LogError(www.error);
                Debug.Log("API Error" + www.error);
            }
            else
            {
                Debug.Log("API complete!");
                Debug.Log(www.downloadHandler.text);

                //from me - getting url
                jsonResponse = www.downloadHandler.text;

                response = JsonUtility.FromJson<Dictionary<string, object>>(jsonResponse);
                if ((string)response["status"] != "success") StartCoroutine(Upload());


                // response = JsonUtility.FromJson<Dictionary<string, object>>(jsonResponse);


/*                if (response.ContainsKey("status") && (string)response["status"] == "success")
                {
                    string[] outputUrls = (string[])response["output"];
                    foreach (string outputUrl in outputUrls)
                    {
                        //Debug.Log("Generated image URL: " + outputUrl);
                        // Handle the generated image URL as needed
                    }
                }
                else if (response.ContainsKey("status") && (string)response["status"] == "processing")
                {
                    // while ((string)response["status"] == "processing")

                   // int eta = (int)response["eta"]; // Estimated time in seconds
                    //Debug.Log("Image is still processing. Waiting for " + eta + " seconds...");
                    //yield return new WaitForSeconds(eta * 3);
                }
                else
                {
                   // Debug.LogError("Error in response: " + jsonResponse);
                }*/



            }

        }
    }

    private void Update()
    {
        if (jsonResponse != "")
        {
            response = JsonUtility.FromJson<Dictionary<string, object>>(jsonResponse);
            if (response.ContainsKey("status") && (string)response["status"] == "success")
            {
                Debug.Log("Contains key and is successful");
                string[] outputUrls = (string[])response["output"];
                foreach (string outputUrl in outputUrls)
                {
                    Debug.Log("Generated image URL: " + outputUrl);
                    // Handle the generated image URL as needed
                }
            }

            else
            {
                Debug.LogError("Error in response: " + jsonResponse);
            }
        }

    }
}
