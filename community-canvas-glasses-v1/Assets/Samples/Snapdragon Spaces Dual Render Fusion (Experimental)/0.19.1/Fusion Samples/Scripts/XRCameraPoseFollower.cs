/******************************************************************************
 * File: XRCameraPoseFollower.cs
 * Copyright (c) 2023 Qualcomm Technologies, Inc. and/or its subsidiaries. All rights reserved.
 *
 ******************************************************************************/

using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Qualcomm.Snapdragon.Spaces.Samples
{
    public class XRCameraPoseFollower : MonoBehaviour
    {
        public bool TrackPosition = true;
        public bool TrackRotation = true;
        public Vector3 PositionOffset = new Vector3(0.0f, -0.5f, 0.0f);
        public bool UsePositionOffset = true;

        [SerializeField]
        private Camera xrCamera;

        [SerializeField]
        private Transform _rotationTransform;

        void Awake() {
            if (xrCamera == null)
            {
                xrCamera = XROriginHelper.GetXROriginCamera(true);
                if (xrCamera == null)
                {
                    Debug.LogError("No XR Camera in the scene");
                    this.enabled = false;
                }
            }
        }

        private void Update() {
            if (TrackPosition) {
                transform.position = xrCamera.transform.position + (UsePositionOffset ? PositionOffset : Vector3.zero);
            }

            if (TrackRotation && _rotationTransform != null) {
                transform.rotation = _rotationTransform.rotation;
            }
        }
    }
}