﻿using Cinemachine;
using Perrinn424.Utilities;
using UnityEngine;
using UnityEngine.Assertions;
using VehiclePhysics;
using EdyCommonTools;


namespace Perrinn424.CameraSystem
{
    public class CameraController : MonoBehaviour
    {
        public enum Mode
        {
            Driver,
            SmoothFollow,
            Orbit,
            OrbitFixed,
            LookAt,
            Free,
            Tv
        }

        public Transform tvCameraSystem;
        public Camera mainCamera;

        private VPCameraController m_vppController;
        private CinemachineBrain m_cmController;
        private CameraFovController m_fovController;
        private CircularIterator<Mode> m_modeIterator;
        private float m_savedCameraFov;


        private void OnEnable()
        {
            m_modeIterator = new CircularIterator<Mode>(new[] { Mode.Driver, Mode.SmoothFollow, Mode.Orbit, Mode.OrbitFixed, Mode.Tv });
            m_vppController = tvCameraSystem.GetComponent<VPCameraController>();
            m_cmController = tvCameraSystem.GetComponent<CinemachineBrain>();
            m_fovController = mainCamera.GetComponent<CameraFovController>();
            m_savedCameraFov = mainCamera.fieldOfView;
            UpdateMode();
        }

        private void OnDisable()
        {
        }


        public void SetMode(Mode mode)
        {
            if (isActiveAndEnabled)
            {
                m_modeIterator.Current = mode;
                UpdateMode();
            }
        }

        public void NextMode()
        {
            if (isActiveAndEnabled)
            {
                m_modeIterator.MoveNext();
                UpdateMode();
            }
        }

        private void UpdateMode()
        {
            switch (m_modeIterator.Current)
            {
                case Mode.Driver:
                    SetVPCamera(VPCameraController.Mode.Driver);
                    break;
                case Mode.SmoothFollow:
                    SetVPCamera(VPCameraController.Mode.SmoothFollow);
                    break;
                case Mode.Orbit:
                    m_vppController.orbit.targetRelative = false;
                    SetVPCamera(VPCameraController.Mode.Orbit);
                    break;
                case Mode.OrbitFixed:
                    m_vppController.orbit.targetRelative = true;
                    SetVPCamera(VPCameraController.Mode.Orbit);
                    break;
                case Mode.LookAt:
                    SetVPCamera(VPCameraController.Mode.LookAt);
                    break;
                case Mode.Free:
                    SetVPCamera(VPCameraController.Mode.Free);
                    break;
                case Mode.Tv:
                    SetTVMode();
                    break;
            }
        }


        private void SetVPCamera(VPCameraController.Mode mode)
        {
            // TV mode may have changed the camera FoV. Restore it here.

            m_cmController.enabled = false;
            mainCamera.fieldOfView = m_savedCameraFov;

            m_vppController.enabled = true;
            m_vppController.mode = mode;

            // Also disable the FoV controller if existing, so it can't
            // change the FoV before is disabled by the TV Camera Zoom Controller.

            if (m_fovController)
                m_fovController.enabled = false;
        }

        private void SetTVMode()
        {
            // Save current camera FoV so TV mode may change it.

            m_vppController.enabled = false;
            m_savedCameraFov = mainCamera.fieldOfView;

            m_cmController.enabled = true;
        }
    }
}
