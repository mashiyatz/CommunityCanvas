/******************************************************************************
 * File: DynamicOpenXRLoader.cs
 * Copyright (c) 2023 Qualcomm Technologies, Inc. and/or its subsidiaries. All rights reserved.
 *
 ******************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Management;

namespace Qualcomm.Snapdragon.Spaces.Samples
{
    public class DynamicOpenXRLoader : MonoBehaviour
    {
        bool managerIsLoading = false;

        public bool AutoStartXROnDisplayConnected = true;

        // Set this to false when using MRTK
        public bool ShutDownXROnDisplayDisconnected = true;

        // Auto-manage AR Camera turns the AR Session Origin on / off as needed
        public bool AutoManageXRCamera = true;

        public UnityEvent OnOpenXRAvailable;
        public UnityEvent OnOpenXRUnavailable;

        public UnityEvent OnOpenXRConnecting;
        public UnityEvent OnOpenXRConnected;
        public UnityEvent OnOpenXRDisconnecting;
        public UnityEvent OnOpenXRDisconnected;

        public enum OpenXRState
        {
            OpenXRAvailable,
            OpenXRUnavailable,

            OpenXRConnecting,
            OpenXRConnected,
            OpenXRDisconnecting,
            OpenXRDisconnected,
            OpenXRFailed
        }

        public enum OpenXRFailReason
        {
            Success,
            NoRuntimeInstalled,
            RuntimeInitializing,
            RuntimeActive,
            FailedToInitialize,
            GlassesDisconnected
        }
        
        public delegate void OpenXRStatusCallback(DynamicOpenXRLoader openXRLoader, OpenXRState openXRState, OpenXRFailReason openXRFailReason = OpenXRFailReason.Success);
        public OpenXRStatusCallback OnOpenXRStatus;

        bool ManagerIsActive;

        GameObject sessionOriginObject = null;

        ARSession session = null;

        private void Awake()
        {
            sessionOriginObject = XROriginHelper.GetXROrigin(true);
            session = FindObjectOfType<ARSession>(true);
        }

        void Start()
        {
            StartCoroutine(DisplaysCoroutine());
        }

        public bool IsOpenXRActive()
        {
            return ManagerIsActive;
        }

        public void SetOpenXREnabledBasedOnToggle(Toggle toggle)
        {
            SetOpenXREnabled(toggle.isOn);
        }

        public void SetOpenXREnabled(bool xrShouldBeEnabled)
        {
            if (xrShouldBeEnabled)
            {
                StartOpenXR();
            }
            else
            {
                StopOpenXR();
            }
        }

        private IEnumerator DisplaysCoroutine()
        {
            int numDisplays = 0;
            while (true)
            {
                while (numDisplays == Display.displays.Length)
                {
                    yield return new WaitForSeconds(0.1f);
                }
                numDisplays = Display.displays.Length;

                if (numDisplays == 1)
                {
                    // Changed from 2 displays to 1
                    OnOpenXRStatus?.Invoke(this, OpenXRState.OpenXRUnavailable);
                    OnOpenXRUnavailable?.Invoke();

                    if (ManagerIsActive)
                    {
                        OnOpenXRStatus?.Invoke(this, OpenXRState.OpenXRFailed, OpenXRFailReason.GlassesDisconnected);
                        StopOpenXR();
                    }

#if !UNITY_EDITOR
                    if (AutoManageXRCamera)
                    {
                        SetSessionOriginActive(false);
                    }
#endif
                }
                else if (numDisplays == 2)
                {
                    OnOpenXRStatus?.Invoke(this, OpenXRState.OpenXRAvailable);
                    OnOpenXRAvailable?.Invoke();
                    if (AutoStartXROnDisplayConnected)
                    {
                        StartOpenXR();
                    }
                }
                yield return new WaitForSeconds(0.1f);
            }
        }

        bool RuntimeIsInstalled()
        {
            var activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            var context = activity.Call<AndroidJavaObject>("getApplicationContext");
            AndroidJavaClass runtimeChecker = null;
            try
            {
                runtimeChecker = new AndroidJavaClass("com.qualcomm.snapdragon.spaces.serviceshelper.RuntimeChecker");
            } catch (Exception e)
            {
                Debug.Log("Could not find services helper. Looking for unity services helper. " + e.ToString());
                try
                {
                    runtimeChecker = new AndroidJavaClass("com.qualcomm.snapdragon.spaces.unityserviceshelper.RuntimeChecker");
                } catch (Exception e2)
                {
                    Debug.Log("Could not find unity services helper. " + e2.ToString());
                }
            }

            if (runtimeChecker != null) {
                return runtimeChecker.CallStatic<bool>("CheckInstalledWithDialog", new object[] { activity, context, null });
            }
            return false;
        }

        public OpenXRFailReason StartOpenXR()
        {
            if (managerIsLoading)
            {
                return OpenXRFailReason.RuntimeInitializing;
            }

            if (ManagerIsActive)
            {
                return OpenXRFailReason.RuntimeActive;
            }


            if (!RuntimeIsInstalled())
            {
                return OpenXRFailReason.NoRuntimeInstalled;
            }

    //        if (Permission.HasUserAuthorizedPermission())

            StartCoroutine(LoadXRCoroutine());
            return OpenXRFailReason.Success;
        }

        public void StopOpenXR()
        {
            if (ManagerIsActive)
            {
                OnOpenXRStatus?.Invoke(this, OpenXRState.OpenXRDisconnecting);
                OnOpenXRDisconnecting?.Invoke();
                XRManagerSettings manager = XRGeneralSettings.Instance.Manager;

                if (ShutDownXROnDisplayDisconnected)
                {
                    manager.StopSubsystems();
                    manager.DeinitializeLoader();
                }
                ManagerIsActive = false;

                OnOpenXRStatus?.Invoke(this, OpenXRState.OpenXRDisconnected);
                OnOpenXRDisconnected?.Invoke();
            }
        }

        private IEnumerator LoadXRCoroutine()
        {
            OnOpenXRStatus?.Invoke(this, OpenXRState.OpenXRConnecting);
            OnOpenXRConnecting?.Invoke();

            managerIsLoading = true;
            yield return new WaitForSeconds(0.5f);

            XRManagerSettings manager = XRGeneralSettings.Instance.Manager;
            StartCoroutine(manager.InitializeLoader());
            while (manager.isInitializationComplete == false)
            {
                yield return new WaitForEndOfFrame();
            }

            managerIsLoading = false;

            // Handle the results....
            if (manager.isInitializationComplete)
            {
                manager.StartSubsystems();

#if !UNITY_EDITOR
                if (AutoManageXRCamera)
                {
                    SetSessionOriginActive(true);
                }
#endif
                ManagerIsActive = true;

                OnOpenXRStatus?.Invoke(this, OpenXRState.OpenXRConnected);
                OnOpenXRConnected?.Invoke();
            }
            else
            {
                OnOpenXRStatus?.Invoke(this, OpenXRState.OpenXRFailed, OpenXRFailReason.FailedToInitialize);
            }
        }

        void SetSessionOriginActive(bool shouldBeActive)
        {
            if (sessionOriginObject != null)
            {
                sessionOriginObject.SetActive(shouldBeActive);
            }

            if (session != null)
            {
                session.gameObject.SetActive(shouldBeActive);
            }
        }
    }
}