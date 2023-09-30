﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace KWS
{
    public partial class WaterSystem
    { 
        ///////////////////////////// platform specific components /////////////////////////////////////////////////
        internal ReflectionPass PlanarReflectionComponent  = new ProbePlanarReflection();
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////



        internal List<ThirdPartyAssetDescription> ThirdPartyFogAssetsDescription = new List<ThirdPartyAssetDescription>()
        {
            new ThirdPartyAssetDescription() {EditorName = "Native Unity Fog", ShaderDefine = ""},
            new ThirdPartyAssetDescription() {EditorName = "Expanse", AssetNameSearchPattern = "Expanse", ShaderDefine          = "EXPANSE", ShaderInclude     = "transparency.hlsl"},
            new ThirdPartyAssetDescription() {EditorName = "Time Of Day", AssetNameSearchPattern = "Time Of Day", ShaderDefine      = "TIME_OF_DAY", ShaderInclude = ""},
            new ThirdPartyAssetDescription(){EditorName = "Enviro", AssetNameSearchPattern = "Enviro - Sky and Weather", ShaderDefine                    = "ENVIRO_FOG", ShaderInclude              = "EnviroFogCore.hlsl"},
            new ThirdPartyAssetDescription(){EditorName = "Enviro 3", AssetNameSearchPattern = "Enviro 3 - Sky and Weather", ShaderDefine = "ENVIRO_3_FOG", ShaderInclude = "FogIncludeHLSL.hlsl"},
            new ThirdPartyAssetDescription(){EditorName = "Atmospheric height fog", AssetNameSearchPattern = "Atmospheric Height Fog", ShaderDefine    = "ATMOSPHERIC_HEIGHT_FOG", ShaderInclude  = "AtmosphericHeightFog.cginc", CustomQueue = 3002},
            //new ThirdPartyAssetDescription(){EditorName = "Volumetric fog and mist 2", ShaderDefine = "VOLUMETRIC_FOG_AND_MIST", ShaderInclude = "VolumetricFogOverlayVF.cginc", DrawToDepth = true},
        };

        KWS_WaterPassHandler waterPassHandler;

        void SubscribeBeforeCameraRendering()
        {
            RenderPipelineManager.beginCameraRendering += RenderPipelineManagerOnbeginCameraRendering;
        }

        void UnsubscribeBeforeCameraRendering()
        {
            RenderPipelineManager.beginCameraRendering -= RenderPipelineManagerOnbeginCameraRendering;
        }

        private void RenderPipelineManagerOnbeginCameraRendering(ScriptableRenderContext ctx, Camera cam)
        {
            OnBeforeCameraRendering(cam);
        }

        void SubscribeAfterCameraRendering()
        {
            RenderPipelineManager.endCameraRendering += RenderPipelineManagerOnendCameraRendering;
        }

        void UnsubscribeAfterCameraRendering()
        {
            RenderPipelineManager.endCameraRendering -= RenderPipelineManagerOnendCameraRendering;
        }

        private void RenderPipelineManagerOnendCameraRendering(ScriptableRenderContext ctx, Camera cam)
        {
            OnAfterCameraRendering(cam);
        }

        void InitializeWaterPlatformSpecificResources()
        {
            if (waterPassHandler == null)
            {
                waterPassHandler = _tempGameObject.AddComponent<KWS_WaterPassHandler>();
                waterPassHandler.WaterInstance = this;
            }
            isWaterPlatformSpecificResourcesInitialized = true;
        }

        void RenderPlatformSpecificFeatures(Camera cam)
        {
            SetAmbientLightToShaders();
        }

        void ReleasePlatformSpecificResources()
        {
            isWaterPlatformSpecificResourcesInitialized = false;
        }

        void SetAmbientLightToShaders()
        {
            // return half3(unity_SHAr.w, unity_SHAg.w, unity_SHAb.w);
            SphericalHarmonicsL2 sh;
            LightProbes.GetInterpolatedProbe(WaterRelativeWorldPosition, null, out sh);
            var ambient = new Vector3(sh[0, 0] - sh[0, 6], sh[1, 0] - sh[1, 6], sh[2, 0] - sh[2, 6]);
            ambient = Vector3.Max(ambient, Vector3.zero);
            Shader.SetGlobalVector(KWS_ShaderConstants.DynamicWaterParams.KWS_AmbientColor, ambient);
        }
    }
}