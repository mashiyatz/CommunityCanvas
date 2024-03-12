//using UnityEngine;
//using UnityEngine.Networking;
//using System.Collections;

//public class controller : MonoBehaviour
//{
   
//    public text_to_3d_controller textTo3DController;

    
//    //Stable Diffusion
//    private void Start()
//    {
//        // Find the instance of StableDiffusionAPI in the scene
//        SD_api diffusionAPI = FindObjectOfType<SD_api>();

//        if (diffusionAPI != null)
//        {
//            Debug.Log("controller script running");
//            //StartCoroutine(diffusionAPI.Generate3DImage("rose", 64, 20, 256, "gif", null, null));
//        }
//        else
//        {
//            Debug.LogError("StableDiffusionAPI component not found in the scene.");
//        }
//    }

//    // Meshy - Method to be called by button's OnClick event
//    public void StartTextTo3DTask()
//    {
//        if (textTo3DController != null)
//        {
//            // Call the CreateTextTo3DTask method from the text_to_3d_controller script
//            textTo3DController.CreateTextTo3DTask("a monster mask", "red fangs, Samurai outfit that fused with japanese batik style", true, "realistic", "low quality, low resolution, low poly, ugly", "1024");
//        }
//        else
//        {
//            Debug.LogError("text_to_3d_controller script is not assigned!");
//        }
//    }
//}

//// Method to be called by button's OnClick event
////public void StartTextTo3DTask()
////{ 
////    StartCoroutine(diffusionAPI.Generate3DImage("rose", 64, 20, 256, "gif", null, null));

////string objectPrompt = "a monster mask";
////string stylePrompt = "red fangs, Samurai outfit that fused with japanese batik style";
////bool enablePBR = true;
////string artStyle = "realistic";
////string negativePrompt = "low quality, low resolution, low poly, ugly";
////string resolution = "1024"; 

////textTo3DController.CreateTextTo3DTask(objectPrompt, stylePrompt, enablePBR, artStyle, negativePrompt, resolution);
////    }

////    // Rest of the script...



