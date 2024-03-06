/******************************************************************************
 * File: CanvasControllerCompanion.cs
 * Copyright (c) 2023 Qualcomm Technologies, Inc. and/or its subsidiaries. All rights reserved.
 *
 ******************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace Qualcomm.Snapdragon.Spaces.Samples
{
    public class CanvasControllerCompanion : MonoBehaviour
    {
        public Transform controllerTransform;

        public Button buttonStateMenu;
        public GameObject touchpad;

        CanvasControllerCompanionInputDeviceState companionState;
        CanvasControllerCompanionInputDevice inputDevice;

        void Awake()
        {
            companionState = new CanvasControllerCompanionInputDeviceState();
            companionState.trackingState = 1;
        }

        public void ReloadApp()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }

        public void SendMenuButtonEvent(int phase)
        {
            var bit = 1 << (int)0;
            if (phase != 0)
            {
                companionState.buttons |= (ushort)bit;
            }
            else
            {
                companionState.buttons &= (ushort)~bit;
            }

            InputSystem.QueueStateEvent(InputSystem.GetDevice<CanvasControllerCompanionInputDevice>(), companionState);
        }

        public void SendTouchpadEvent(int phase, Vector2 position)
        {
            var bit = 1 << (int)1;
            if (phase != 0)
            {
                companionState.buttons |= (ushort)bit;
            }
            else
            {
                companionState.buttons &= (ushort)~bit;
                OnTouchpadEnd?.Invoke();
            }

            companionState.touchpadPosition.x = position.x;
            companionState.touchpadPosition.y = position.y;

            InputSystem.QueueStateEvent(InputSystem.GetDevice<CanvasControllerCompanionInputDevice>(), companionState);
        }

        public UnityEvent OnTouchpadEnd;
        
        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}