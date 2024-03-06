/******************************************************************************
 * File: CubeManipulation.cs
 * Copyright (c) 2023 Qualcomm Technologies, Inc. and/or its subsidiaries. All rights reserved.
 *
 ******************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

namespace Qualcomm.Snapdragon.Spaces.Samples
{
    public class CubeManipulation : MonoBehaviour
    {
        public GameObject cube;
        public MeshRenderer meshRenderer;

        public void SetColor(Image image)
        {
            meshRenderer.material.color = image.color;
        }

        public void UpdateRotation(Slider sliderRot)
        {
            Vector3 localEulerAngles = cube.transform.localEulerAngles;
            localEulerAngles.y = -sliderRot.value;

            cube.transform.localEulerAngles = localEulerAngles;
        }

        public void UpdateVisibility(Toggle toggle)
        {
            cube.SetActive(toggle.isOn);
        }

        public void PlaceCube()
        {
            Camera c = XROriginHelper.GetXROriginCamera();
            if (c != null)
            {
                cube.transform.position = c.transform.position + c.transform.forward * 1.25f;
            }
        }
    }
}