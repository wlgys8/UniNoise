using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MS.Noise{
    public class PerlinNoise2D:INoise2D
    {
        private Vector2Int _loopSize;

        public PerlinNoise2D(){
        }

        public PerlinNoise2D(Options options){
            _loopSize = options.loopSize;
        }

        public float Evaluate(float x,float y){
            var ix = Mathf.FloorToInt(x);
            var iy = Mathf.FloorToInt(y);
            var fx = x - ix;
            var fy = y - iy;

            var ix1 = ix + 1;
            var iy1 = iy + 1;

            ix = MathUtility.ModPositive(ix,_loopSize.x);
            iy = MathUtility.ModPositive(iy,_loopSize.y);
            ix1 = MathUtility.ModPositive(ix1,_loopSize.x);
            iy1 = MathUtility.ModPositive(iy1,_loopSize.y);

            var p1 = new Vector2(fx,fy);
            var p2 = p1 - new Vector2(1,0);
            var p3 = p1 - new Vector2(0,1);
            var p4 = p1 - new Vector2(1,1);

            var hash1 = RandomHash.Get(ix,iy);
            var hash2 = RandomHash.Get(ix1,iy);
            var hash3 = RandomHash.Get(ix,iy1);
            var hash4 = RandomHash.Get(ix1,iy1);

            var g1 = RandomGrad.GetVector(hash1);
            var g2 = RandomGrad.GetVector(hash2);
            var g3 = RandomGrad.GetVector(hash3);
            var g4 = RandomGrad.GetVector(hash4);

            var v1 = Vector2.Dot(g1,p1);
            var v2 = Vector2.Dot(g2,p2);
            var v3 = Vector2.Dot(g3,p3);
            var v4 = Vector2.Dot(g4,p4);

            var a = MathUtility.SmoothLerp2(v1,v2,fx);
            var b = MathUtility.SmoothLerp2(v3,v4,fx);
            return MathUtility.SmoothLerp2(a,b,fy);
        }


        public class Options{

            /// <summary>
            /// <=0 means non-loop mode, otherwise noise will be looped in the configurated size. It is useful when you want to generate a Seamless Noise Texture.
            /// </summary>
            public Vector2Int loopSize;
        }
       


    }
}
