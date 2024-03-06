/******************************************************************************
 * File: GyroResetButton.cs
 * Copyright (c) 2023 Qualcomm Technologies, Inc. and/or its subsidiaries. All rights reserved.
 *
 ******************************************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Qualcomm.Snapdragon.Spaces.Samples
{
    public class GyroResetButton : Button
    {
        public delegate void PointerActionHappened(Button button);
        bool repeatReset = false;
        GyroToRotation[] gyroToRotations;

        protected override void Awake()
        {
            base.Awake();
            gyroToRotations = FindObjectsOfType<GyroToRotation>();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            repeatReset = true;
            StartCoroutine(GyroResetRepeat());
        }

        IEnumerator GyroResetRepeat()
        {
            while (repeatReset)
            {
                if (gyroToRotations != null)
                {
                    foreach (GyroToRotation gyroToRotation in gyroToRotations)
                    {
                        gyroToRotation.resetGyro();
                    }
                }
                yield return new WaitForEndOfFrame();
            }
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            repeatReset = false;
        }
    }
}