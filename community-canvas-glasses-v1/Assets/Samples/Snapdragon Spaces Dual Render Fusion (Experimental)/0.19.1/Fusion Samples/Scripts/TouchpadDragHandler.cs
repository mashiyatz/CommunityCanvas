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
    public class TouchpadDragHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Image spriteImage;
        public Sprite touchpadUntouched;
        public Sprite touchpadTouched;

        public CanvasControllerCompanion inputDevice;

        Vector2 NormalizedPosition(Vector2 eventPosition)
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            float halfWidth = rectTransform.rect.width / 2;
            float halfHeight = rectTransform.rect.height / 2;

            Vector2 localizedPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), eventPosition,
                null, out localizedPosition))
            {
                Vector2 normalized = new Vector2(localizedPosition.x / halfWidth, localizedPosition.y / halfHeight);
                // Debug.Log("Localized: " + localizedPosition.ToString("F2") + " => Normalized: " + normalized.ToString("F2"));
                return normalized;
            }
            return Vector2.zero;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            spriteImage.sprite = touchpadTouched;
            inputDevice.SendTouchpadEvent(1, NormalizedPosition(eventData.position));
        }

        public void OnDrag(PointerEventData eventData)
        {
            inputDevice.SendTouchpadEvent(2, NormalizedPosition(eventData.position));
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            spriteImage.sprite = touchpadUntouched;
            inputDevice.SendTouchpadEvent(0, NormalizedPosition(eventData.position));
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            spriteImage.sprite = touchpadTouched;
            inputDevice.SendTouchpadEvent(1, NormalizedPosition(eventData.position));
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            spriteImage.sprite = touchpadUntouched;
            inputDevice.SendTouchpadEvent(0, NormalizedPosition(eventData.position));
        }
    }
}