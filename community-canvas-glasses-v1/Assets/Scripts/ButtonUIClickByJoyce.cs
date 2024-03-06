/******************************************************************************
 * File: TouchpadDragHandler.cs
 * Copyright (c) 2023 Qualcomm Technologies, Inc. and/or its subsidiaries. All rights reserved.
 *
 ******************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Qualcomm.Snapdragon.Spaces.Samples
{
    public class ButtonUIClickByJoyce : MonoBehaviour
    {
        //public Image spriteImage;
        //public Sprite touchpadUntouched;
        //public Sprite touchpadTouched;
        public GameObject objectA;
        public GameObject CanvasA;
        //public GameObject objectB;
        //public GameObject CanvasB;
        //public GameObject objectC;
        //public GameObject CanvasC;
        // private int caseNum;
        public GameObject UIMenu;

        //public CanvasControllerCompanion inputDevice;
        private Camera XROriginCamera;
        private Vector3 startPosition;
        private Camera environmentCamera;

        public void Start()
        {
            startPosition = objectA.transform.position;

            objectA.SetActive(false);
            CanvasA.SetActive(false);
            // this comment is just a test to see if compiling times slow down a bit. 

            print("hello"); // this doesn't mean anything, just to check compilation. 

            XROriginCamera = XROriginHelper.GetXROriginCamera();
            environmentCamera = GameObject.Find("EnvCamera").GetComponent<Camera>();
        }

        public void Button1Clicked()
        {
            Debug.Log("button is clicked");
            CanvasA.SetActive(true);
            objectA.SetActive(true);
            UIMenu.SetActive(false);

/*            if (xrOriginCamera != null)
            {
                objectA.transform.position = xrOriginCamera.transform.position + xrOriginCamera.transform.forward * 2.5f + xrOriginCamera.transform.up * -0.5f;
            }*/

            if (environmentCamera != null)
            {
                environmentCamera.clearFlags = CameraClearFlags.SolidColor;
                environmentCamera.transform.position = startPosition - environmentCamera.transform.forward * 2.5f + environmentCamera.transform.up * 0.5f;
            }

        }

        public void GoBackToAssetUI1()
        {
            CanvasA.SetActive(false);
            //objectA.SetActive(false);
            UIMenu.SetActive(true);
            Debug.Log("now we go back to asset UI");

            // objectA.transform.position = startPosition;

            if (environmentCamera != null)
            {
                environmentCamera.clearFlags = CameraClearFlags.Skybox;
            }

        }
       
    }
}