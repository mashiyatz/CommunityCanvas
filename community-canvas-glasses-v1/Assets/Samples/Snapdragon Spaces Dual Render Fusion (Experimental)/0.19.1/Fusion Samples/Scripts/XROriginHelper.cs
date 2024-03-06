/******************************************************************************
 * File: XROriginHelper.cs
 * Copyright (c) 2023 Qualcomm Technologies, Inc. and/or its subsidiaries. All rights reserved.
 *
 ******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Qualcomm.Snapdragon.Spaces.Samples
{
    public class XROriginHelper : MonoBehaviour
    {
        public static GameObject GetXROrigin(bool includeInactive = false)
        {
            XROrigin xro = FindObjectOfType<XROrigin>(includeInactive);
            if (xro != null)
            {
                return xro.gameObject;
            }

            ARSessionOrigin aso = FindObjectOfType<ARSessionOrigin>(includeInactive);
            if (aso != null)
            {
                return aso.gameObject;
            }
            return null;
        }

        public static Camera GetXROriginCamera(bool includeInactive = false)
        {
            GameObject xrOriginObject = GetXROrigin(includeInactive);
            if (xrOriginObject != null)
            {
                XROrigin xrOrigin = xrOriginObject.GetComponent<XROrigin>();
                if (xrOrigin != null)
                {
                    if (xrOrigin.Camera != null) {
                        return xrOrigin.Camera;
                    }

                    // In case there is both an XROrigin and an ARSessionOrigin and only the ARSessionOrigin has a set camera
                    ARSessionOrigin aso = FindObjectOfType<ARSessionOrigin>(includeInactive);
                    if (aso != null)
                    {
                        return aso.camera;
                    }

                }
                if (xrOriginObject.GetComponent<ARSessionOrigin>() != null)
                {
                    return xrOriginObject.GetComponent<ARSessionOrigin>().camera;
                }
            }
            return null;
        }
    }
}
