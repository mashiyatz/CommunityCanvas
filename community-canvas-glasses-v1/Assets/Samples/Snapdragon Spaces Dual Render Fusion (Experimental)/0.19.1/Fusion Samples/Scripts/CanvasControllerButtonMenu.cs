/******************************************************************************
 * File: CanvasControllerButtonMenu.cs
 * Copyright (c) 2023 Qualcomm Technologies, Inc. and/or its subsidiaries. All rights reserved.
 *
 ******************************************************************************/
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Qualcomm.Snapdragon.Spaces.Samples
{
    public class CanvasControllerButtonMenu : Button
    {
        public CanvasControllerCompanion inputDevice;

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            inputDevice.SendMenuButtonEvent(1);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            inputDevice.SendMenuButtonEvent(0);
        }
    }
}