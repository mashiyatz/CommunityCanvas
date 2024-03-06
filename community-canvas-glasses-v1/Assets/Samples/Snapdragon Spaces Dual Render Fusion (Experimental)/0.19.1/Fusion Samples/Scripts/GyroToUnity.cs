/******************************************************************************
 * File: GyroToUnity.cs
 * Copyright (c) 2023 Qualcomm Technologies, Inc. and/or its subsidiaries. All rights reserved.
 *
 ******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qualcomm.Snapdragon.Spaces.Samples
{
    public class GyroToUnity : MonoBehaviour
    {
        public Transform deviceTransform;

        private void Awake()
        {
            if (deviceTransform == null)
            {
                deviceTransform = transform;
            }
        }

        void Start()
        {
            Input.gyro.enabled = true;
        }

        void Update()
        {
            deviceTransform.Rotate(0, -Input.gyro.rotationRateUnbiased.y, 0);
        }
    }
}