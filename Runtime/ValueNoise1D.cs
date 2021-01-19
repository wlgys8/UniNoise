using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MS.Noise{
    public class ValueNoise1D : INoise1D
    {
        public float Evaluate(float x)
        {
            var ix = Mathf.FloorToInt(x);
            var fx = x - ix;
            float v1 = RandomValue(ix);
            float v2 = RandomValue(ix + 1);
            return MathUtility.SmoothLerp2(v1,v2,fx);
        }

        private static float RandomValue(int x){
            var hash = RandomHash.Get(x);
            return (hash - 128) / 128f;
        }
        
    }
}
