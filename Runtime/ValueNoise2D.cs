using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MS.Noise{
    public class ValueNoise2D : INoise2D
    {
        private Options _options;

        public ValueNoise2D(){
            _options = new Options();
        }

        public ValueNoise2D(Options options){
            _options = options;
        }

        public float Evaluate(float x, float y)
        {
            var ix = Mathf.FloorToInt(x);
            var iy = Mathf.FloorToInt(y);
            var fx = x - ix;
            var fy = y - iy;

            var loopSize = _options.loopSize;
            ix = MathUtility.ModPositive(ix,loopSize.x);
            var ix1 = MathUtility.ModPositive(ix + 1, loopSize.x);
            iy = MathUtility.ModPositive(iy, loopSize.y);
            var iy1 = MathUtility.ModPositive(iy + 1, loopSize.y);
 

            var v1 = RandomValue(ix,iy);
            var v2 = RandomValue(ix1,iy);
            var v3 = RandomValue(ix,iy1);
            var v4 = RandomValue(ix1,iy1);

            var a = MathUtility.SmoothLerp2(v1,v2,fx);
            var b = MathUtility.SmoothLerp2(v3,v4,fx);
            return MathUtility.SmoothLerp2(a,b,fy);
        }

        private static float RandomValue(int x,int y){
            var hash = RandomHash.Get(x,y);
            return (hash - 128) / 128f;
        }


        public class Options{
            public Vector2Int loopSize;
        }
    }
}
