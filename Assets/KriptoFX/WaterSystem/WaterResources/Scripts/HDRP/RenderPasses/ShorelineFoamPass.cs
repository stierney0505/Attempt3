using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace KWS
{
    public class ShorelineFoamPass : WaterPass
    {
        ShorelineFoamPassCore _pass;
        CommandBuffer _cmd;

        protected override void Setup(ScriptableRenderContext renderContext, CommandBuffer cmd)
        {
            if (_pass == null)
            {
                _pass = new ShorelineFoamPassCore(WaterInstance);
                _pass.OnSetRenderTarget += OnSetRenderTarget;
            }
            name = _pass.PassName;
        }

        protected override void Execute(CustomPassContext ctx)
        {
            var cam = ctx.hdCamera.camera;
            _cmd = ctx.cmd;
            if (!WaterInstance.IsWaterVisible || !KWS_CoreUtils.CanRenderWaterForCurrentCamera(WaterInstance, cam)) return;

            IsInitialized = true;
            _pass.Execute(cam, _cmd, ctx.cameraColorBuffer, ctx.cameraDepthBuffer);

        }
        private void OnSetRenderTarget(CommandBuffer cmd, Camera cam, RTHandle rt)
        {
            CoreUtils.SetRenderTarget(cmd, rt);
            CoreUtils.ClearRenderTarget(cmd, ClearFlag.Color, Color.clear);
        }

        public override void Release()
        {
            if (_pass != null)
            {
                _pass.OnSetRenderTarget -= OnSetRenderTarget;
                _pass.Release();
            }
            IsInitialized = false;
        }

    }
}
