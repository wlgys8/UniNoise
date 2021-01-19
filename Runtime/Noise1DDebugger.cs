using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MS.Noise{
    public class Noise1DDebugger
    {
        private static Material _unlit;

        private INoise1D _noise;

        public Noise1DDebugger(INoise1D noise){
            _noise = noise;
        }

        public void OnPostRender() {
            if(!_unlit){
                _unlit = new Material(Shader.Find("Unlit/Color"));
            }
            GL.PushMatrix();
            GL.LoadOrtho();
            _unlit.SetPass(0);

            GL.Begin(GL.LINES);
            GL.Color(Color.red);

            var startX = 0f;
            var endX = 1f;
            var count = 100;
            var delta = (endX - startX)/count;
            var x = startX;
            for(var i = 0; i < count; i ++){
                var y = 0.5f;
                var offset = _noise.Evaluate(x * 10) * 0.1f;
                GL.Vertex(new Vector3(x,y + offset,0.5f));
                x += delta;
                offset = _noise.Evaluate(x * 10) * 0.1f;
                GL.Vertex(new Vector3(x,y + offset,0.5f));
            }
            GL.End();
            GL.PopMatrix();            
        }
    }
}
