using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace KWS
{
    class MaskDepthNormalPass : WaterPass
    {
        MaskDepthNormalPassCore _pass;
        RenderTargetIdentifier[] _mrt = new RenderTargetIdentifier[2];

        protected override void Setup(ScriptableRenderContext renderContext, CommandBuffer cmd)
        {
            if (_pass == null)
            {
                _pass                           =  new MaskDepthNormalPassCore(WaterInstance);
                _pass.OnInitializedRenderTarget += OnInitializedRenderTarget;
            }
            name = _pass.PassName;
        }

        protected override void Execute(CustomPassContext ctx)
        {
            var cam = ctx.hdCamera.camera;
            if (!WaterInstance.IsWaterVisible || !KWS_CoreUtils.CanRenderWaterForCurrentCamera(WaterInstance, cam)) return;

            IsInitialized = true;
            _pass.Execute(cam, ctx.cmd);

        }

        private void OnInitializedRenderTarget(CommandBuffer cmd, RTHandle rt1, RTHandle rt2, RTHandle rt3)
        {
            _mrt[0] = rt1;
            _mrt[1] = rt2;
            CoreUtils.SetRenderTarget(cmd, _mrt, rt3, ClearFlag.All, Color.black);
        }

        public override void Release()
        {
            if (_pass != null)
            {
                _pass.OnInitializedRenderTarget -= OnInitializedRenderTarget;
                _pass.Release();
            }
            IsInitialized = false;
        }
    }
}
