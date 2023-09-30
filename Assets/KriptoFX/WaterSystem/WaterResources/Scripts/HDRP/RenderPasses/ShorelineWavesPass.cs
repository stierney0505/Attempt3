using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace KWS
{
    public class ShorelineWavesPass : WaterPass
    {
        ShorelineWavesPassCore _pass;
        CommandBuffer _cmd;
        RenderTargetIdentifier[] _mrt = new RenderTargetIdentifier[2];

        protected override void Setup(ScriptableRenderContext renderContext, CommandBuffer cmd)
        {
            if (_pass == null)
            {
                _pass = new ShorelineWavesPassCore(WaterInstance);
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
            _pass.Execute(cam, _cmd);

        }

        private void OnSetRenderTarget(CommandBuffer cmd, Camera cam, RTHandle rt1, RTHandle rt2)
        {
            _mrt[0] = rt1;
            _mrt[1] = rt2;
            CoreUtils.SetRenderTarget(cmd, _mrt, rt1.rt.depthBuffer, ClearFlag.Color, Color.black);
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
