using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MS.Noise{
    public class FractalNoise1D:INoise1D
    {

        private INoise1D[] _octaves;
        public FractalNoise1D(params INoise1D[] octaves){
            _octaves = octaves;
        }
        
        public float Evaluate(float x)
        {
            float frequency = 1;
            float amplitude = 1;
            float maxAmplitude = 0;
            float total = 0;
            foreach(var oc in _octaves){
                maxAmplitude += amplitude;
                total += oc.Evaluate(x * frequency) * amplitude;
                frequency *= 2;
                amplitude *= 0.5f;
            }
            return total / maxAmplitude;
        }
    }
}
