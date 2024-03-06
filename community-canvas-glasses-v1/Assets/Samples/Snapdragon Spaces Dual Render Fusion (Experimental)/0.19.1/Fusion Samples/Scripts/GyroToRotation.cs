/******************************************************************************
 * File: GyroToRotation.cs
 * Copyright (c) 2023 Qualcomm Technologies, Inc. and/or its subsidiaries. All rights reserved.
 *
 ******************************************************************************/
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Qualcomm.Snapdragon.Spaces.Samples
{
    public class GyroToRotation : MonoBehaviour
    {
        public Camera xrCamera;
        public GameObject controllerRepresentation;
        public Vector3 rotationRate = new Vector3(0, 0, 0);

        public UnityEngine.Vector3 RotationRate
        {
            get { return rotationRate; }
            set { rotationRate = value; }
        }

        private void Awake()
        {
            if (xrCamera == null)
            {
                xrCamera = XROriginHelper.GetXROriginCamera();
            }

            enableGyro(true);
        }

        private void Start()
        {
            resetGyro();
        }

        protected void Update()
        {
            if (Input.gyro.enabled)
            {
                rotationRate = Input.gyro.rotationRate;
                float rx = -rotationRate.x;
                float ry = -rotationRate.z;
                float rz = -rotationRate.y;
                rotationRate.x = rx;
                rotationRate.y = ry;
                rotationRate.z = rz;
                controllerRepresentation.transform.Rotate(RotationRate);
            }
        }

        public void enableGyro(bool isOn)
        {
            Input.gyro.enabled = isOn;
        }

        public void resetGyro()
        {
            Vector3 forward = xrCamera.transform.forward;
            forward.y = 0;
            controllerRepresentation.transform.forward = forward;// rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}