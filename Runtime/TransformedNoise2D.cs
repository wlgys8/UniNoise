using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MS.Noise{

    /// <summary>
    /// apply frequency and amplitude to source noise
    /// </summary>
    public class TransformedNoise2D
    {
        private INoise2D _sourceNoise;
        private float _frequency;
        private float _amplitude;
        
        public TransformedNoise2D(INoise2D sourceNoise,float frequency,float amplitude){
            _sourceNoise = sourceNoise;
        }

        public float Evaluate(float x, float y)
        {
            return _sourceNoise.Evaluate(x * _frequency,y * _frequency) * _amplitude;
        }
    }
}
