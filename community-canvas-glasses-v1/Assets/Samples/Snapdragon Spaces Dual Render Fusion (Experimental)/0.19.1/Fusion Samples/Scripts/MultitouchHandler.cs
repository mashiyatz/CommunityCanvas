/******************************************************************************
 * File: MultitouchHandler.cs
 * Copyright (c) 2023 Qualcomm Technologies, Inc. and/or its subsidiaries. All rights reserved.
 *
 ******************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;

namespace Qualcomm.Snapdragon.Spaces.Samples
{
    public class MultitouchHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Image spriteImage;
        public Sprite touchpadUntouched;
        public Sprite touchpadTouched;

        public float minScale = 0.01f;
        public float maxScale = 10f;

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
                return normalized;
            }
            return Vector2.zero;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (spriteImage != null)
            {
                spriteImage.sprite = touchpadTouched;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (spriteImage != null)
            {
                spriteImage.sprite = touchpadUntouched;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (spriteImage != null)
            {
                spriteImage.sprite = touchpadTouched;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (spriteImage != null)
            {
                spriteImage.sprite = touchpadUntouched;
            }
        }

        float scaleStart = 0;
        float distanceStart = 0;

        Vector2 touchpadStartCenter;
        int numTouchesLast = 0;

        public Transform targetTransform;
        Vector2 touchpadLastCenter;

        public Transform cameraTransform;

        private void Start()
        {
            if (cameraTransform == null)
            {
                Camera xrCamera = XROriginHelper.GetXROriginCamera();
                if (xrCamera != null)
                {
                    cameraTransform = xrCamera.transform;
                }
                else
                {
                    cameraTransform = Camera.main.transform;
                }
            }
        }

        public enum TouchMode
        {
            None,
            SingleMode,
            DualMode,
            TripleMode
        }

        TouchMode touchMode = TouchMode.None;

        float DistanceOfTouches(List<Vector2> touchPositions)
        {
            if (touchPositions.Count >= 2)
            {
                return Vector2.Distance(touchPositions[0], touchPositions[1]);
            }
            return 0;
        }

        Vector2 CenterOfTouches(List<Vector2> touchPositions)
        {
            int max = Mathf.Min(3, touchPositions.Count);
            if (max == 0)
            {
                return Vector2.zero;
            }

            Vector2 total = Vector2.zero;
            for (int i = 0; i < max; i++)
            {
                total += touchPositions[i];
            }

            return total / max;
        }

        List<Vector2> touchHistory = new List<Vector2>();

        float SignedAngle(List<Vector2> touchHistory, List<Vector2> touchPositions)
        {
            return Vector2.SignedAngle(touchHistory[0] - touchHistory[1], touchPositions[0] - touchPositions[1]);
        }

        bool firstTouchIsInPosition = false;

        private void Update()
        {
            List<Vector2> touchPositions = new List<Vector2>();
            int numTouchesCurrent = 0;

#if UNITY_EDITOR
            if (Input.GetMouseButton(0) && !Input.GetMouseButtonUp(0))
            {
                if (!firstTouchIsInPosition)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), Input.mousePosition))
                        {
                            firstTouchIsInPosition = true;
                        }
                    }
                }
                numTouchesCurrent++;
                touchPositions.Add(NormalizedPosition(Input.mousePosition));
            }
#else
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch t = Input.GetTouch(i);
                if (t.phase != TouchPhase.Canceled && t.phase != TouchPhase.Ended)
                {
                    if (!firstTouchIsInPosition)
                    {
                        if (t.phase == TouchPhase.Began)
                        {
                            if (RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), t.position))
                            {
                                firstTouchIsInPosition = true;
                            }
                        }
                    }
                    numTouchesCurrent++;
                    touchPositions.Add(NormalizedPosition(t.position));
                }
            }
#endif

            if (!firstTouchIsInPosition)
            {
                return;
            }

            bool numTouchesChanged = numTouchesCurrent != numTouchesLast;
            numTouchesLast = numTouchesCurrent;

            if (numTouchesCurrent < 1)
            {
                touchMode = TouchMode.None;
                firstTouchIsInPosition = false;
                return;
            }

            if (numTouchesChanged)
            {
                switch (numTouchesCurrent)
                {
                    case 2:
                        touchMode = TouchMode.DualMode;
                        break;

                    case 1:
                        touchMode = TouchMode.SingleMode;
                        break;

                    default:
                        touchMode = TouchMode.TripleMode;
                        break;
                }

                touchHistory = touchPositions;
                if (touchMode == TouchMode.DualMode)
                {
                    distanceStart = DistanceOfTouches(touchPositions);
                }

                touchpadStartCenter = CenterOfTouches(touchPositions);
                touchpadLastCenter = touchpadStartCenter;

                scaleStart = targetTransform.localScale.x;
                return;
            }

            Vector2 touchpadCurrentCenter = CenterOfTouches(touchPositions);

            float distanceCurrent = DistanceOfTouches(touchPositions);

            switch (touchMode)
            {
                case TouchMode.SingleMode:
                    {
                        Vector3 delta = ((touchpadCurrentCenter.y - touchpadLastCenter.y) / 1.0f * cameraTransform.transform.forward * targetTransform.localScale.x +
                                                    (touchpadCurrentCenter.x - touchpadLastCenter.x) / 1.0f * cameraTransform.transform.right * targetTransform.localScale.x);

                        delta.y = 0;
                        targetTransform.position += delta;
                    }
                    break;

                case TouchMode.DualMode:
                    targetTransform.localScale = Mathf.Max(minScale, Mathf.Min(maxScale, (scaleStart * (1 + distanceCurrent - distanceStart)))) * Vector3.one;

                    float rotateAngle = SignedAngle(touchPositions, touchHistory);
                    targetTransform.Rotate(Vector3.up, rotateAngle);
                    break;

                case TouchMode.TripleMode:
                    targetTransform.position += (touchpadCurrentCenter.y - touchpadLastCenter.y) / 1.0f * new Vector3(0, 1, 0);
                    break;
            }

            touchpadLastCenter = touchpadCurrentCenter;
            touchHistory = touchPositions;
        }
    }
}