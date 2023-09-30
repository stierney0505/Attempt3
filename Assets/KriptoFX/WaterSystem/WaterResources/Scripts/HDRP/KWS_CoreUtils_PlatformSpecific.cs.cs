﻿using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.XR;

namespace KWS
{
    public static partial class KWS_CoreUtils
    {
        static bool CanRenderWaterForCurrentCamera_PlatformSpecific(Camera cam)
        {
            return true;
        }

        public static Vector2Int GetCameraRTHandleViewPortSize(Camera cam)
        {
            if (XRSettings.enabled)
            {
                return new Vector2Int(XRSettings.eyeTextureWidth, XRSettings.eyeTextureHeight);
            }
            else
            {
                var viewPortSize = RTHandles.rtHandleProperties.currentViewportSize;
                if (viewPortSize.x == 0 || viewPortSize.y == 0) return new Vector2Int(cam.pixelWidth, cam.pixelHeight);
                else return viewPortSize;
            }
        }

        public static bool CanRenderSinglePassStereo(Camera cam)
        {
            return XRSettings.enabled &&
                   (XRSettings.stereoRenderingMode == XRSettings.StereoRenderingMode.SinglePassInstanced && cam.cameraType != CameraType.SceneView);
        }

        public static bool IsSinglePassStereoEnabled()
        {
            return XRSettings.enabled && XRSettings.stereoRenderingMode == XRSettings.StereoRenderingMode.SinglePassInstanced;
        }


        //public static bool IsSinglePassStereoEnabled(Camera cam)
        //{
        //    if (!XRGraphics.enabled) return false;

        //    if (Application.isPlaying)
        //    {
        //        return (XRGraphics.stereoRenderingMode == XRGraphics.StereoRenderingMode.SinglePassInstanced || XRGraphics.stereoRenderingMode == XRGraphics.StereoRenderingMode.SinglePassMultiView);
        //        if (cam.cameraType == CameraType.SceneView) return true;

        //    }
        //    else return XRGraphics.stereoRenderingMode == XRGraphics.StereoRenderingMode.SinglePassInstanced || XRGraphics.stereoRenderingMode == XRGraphics.StereoRenderingMode.SinglePassMultiView;
        //}

        public static void UniversalCameraRendering(WaterSystem waterInstance, Camera camera)
        {
            camera.Render();
        }

        public static void SetPlatformSpecificPlanarReflectionParams(Camera reflCamera)
        {
            var camData = reflCamera.GetComponent<HDAdditionalCameraData>();
            if (camData == null) camData = reflCamera.gameObject.AddComponent<HDAdditionalCameraData>();

            camData.invertFaceCulling = true;
        }

        public static void UpdatePlatformSpecificPlanarReflectionParams(Camera reflCamera, WaterSystem waterInstance)
        {
            if (waterInstance.Settings.UseScreenSpaceReflection && waterInstance.Settings.UseAnisotropicReflections)
            {
                reflCamera.clearFlags = CameraClearFlags.Color;
                reflCamera.backgroundColor = Color.black;
            }
            else
            {
                reflCamera.clearFlags = CameraClearFlags.Skybox;
            }
        }

        public static void SetPlatformSpecificCubemapReflectionParams(Camera reflCamera)
        {
            var cameraData = reflCamera.GetComponent<HDAdditionalCameraData>();
            if (cameraData == null) cameraData = reflCamera.gameObject.AddComponent<HDAdditionalCameraData>();

            cameraData.DisableAllCameraFrameSettings();
            cameraData.customRenderingSettings = true;
            reflCamera.SetCameraFrameSetting(FrameSettingsField.VolumetricClouds, true);
            reflCamera.SetCameraFrameSetting(FrameSettingsField.OpaqueObjects, true);
        }

        public static void SetComputeShadersDefaultPlatformSpecificValues(this CommandBuffer cmd, ComputeShader cs, int kernel)
        {
            cmd.SetComputeTextureParam(cs, kernel, "_AirSingleScatteringTexture", KWS_CoreUtils.DefaultBlack3DTexture);
            cmd.SetComputeTextureParam(cs, kernel, "_AerosolSingleScatteringTexture", KWS_CoreUtils.DefaultBlack3DTexture);
            cmd.SetComputeTextureParam(cs, kernel, "_MultipleScatteringTexture", KWS_CoreUtils.DefaultBlack3DTexture);

        }
    }
}