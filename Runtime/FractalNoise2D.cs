using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MS.Noise{
    public class FractalNoise2D : INoise2D
    {
        private INoise2D[] _octaves;
        public FractalNoise2D(INoise2D[] octaves){
            _octaves = octaves;
        }

        public float Evaluate(float x, float y)
        {
            float frequency = 1;
            float amplitude = 1;
            float maxAmplitude = 0;
            float total = 0;
            foreach(var oc in _octaves){
                maxAmplitude += amplitude;
                total += oc.Evaluate(x * frequency,y * frequency) * amplitude;
                frequency *= 2;
                amplitude *= 0.5f;
            }
            return total / maxAmplitude;
        }

   
    }
}
