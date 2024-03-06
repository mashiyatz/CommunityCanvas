/******************************************************************************
 * File: TransformFollower.cs
 * Copyright (c) 2023 Qualcomm Technologies, Inc. and/or its subsidiaries. All rights reserved.
 *
 ******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qualcomm.Snapdragon.Spaces.Samples
{
    public class TransformFollower : MonoBehaviour
    {
        public Transform transformToFollow;

        // Update is called once per frame
        void Update()
        {
            if (transformToFollow != null)
            {
                transform.position = transformToFollow.position;
                transform.rotation = transformToFollow.rotation;
            }
        }
    }
}