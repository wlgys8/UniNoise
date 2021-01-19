using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MS.Noise{
    public class ValueNoise2D : INoise2D
    {
        public float Evaluate(float x, float y)
        {
            var ix = Mathf.FloorToInt(x);
            var iy = Mathf.FloorToInt(y);
            var fx = x - ix;
            var fy = y - iy;

            var v1 = RandomValue(ix,iy);
            var v2 = RandomValue(ix + 1,iy);
            var v3 = RandomValue(ix,iy + 1);
            var v4 = RandomValue(ix + 1,iy + 1);

            var a = MathUtility.SmoothLerp2(v1,v2,fx);
            var b = MathUtility.SmoothLerp2(v3,v4,fx);
            return MathUtility.SmoothLerp2(a,b,fy);
        }

        private static float RandomValue(int x,int y){
            var hash = RandomHash.Get(x,y);
            return (hash - 128) / 128f;
        }
    }
}
