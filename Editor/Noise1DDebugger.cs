using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MS.Noise.Editor{
    public class Noise1DDebugger
    {
        private Material _material;

        private INoise1D _noise;

        private float _frequency = 1;
        private float _amplitude = 1;

        public Noise1DDebugger(INoise1D noise){
            _noise = noise;
        }

        public float frequency{
            get{
                return _frequency;
            }set{
                _frequency = value;
            }
        }

        public float amplitude{
            get{
                return _amplitude;
            }set{
                _amplitude = value;
            }
        }

        public void OnPostRender() {
            if(!_material){
                _material = new Material(Shader.Find("Hidden/Internal-Colored"));
            }
            GL.PushMatrix();
            GL.LoadOrtho();
            _material.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            var startX = 0f;
            var endX = 1f;
            var count = 1000;
            var delta = (endX - startX)/count;
            var x = startX;
            for(var i = 0; i < count; i ++){
                var y = 0.5f;
                var offset = _noise.Evaluate(x * _frequency) * _amplitude;
                GL.Vertex(new Vector3(x,y + offset,0.5f));
                x += delta;
                offset = _noise.Evaluate(x * _frequency) * _amplitude;
                GL.Vertex(new Vector3(x,y + offset,0.5f));
            }
            GL.End();
            GL.PopMatrix();            
        }
    }
}
