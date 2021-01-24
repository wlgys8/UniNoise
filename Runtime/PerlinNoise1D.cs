using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MS.Noise{
    public class PerlinNoise1D : INoise1D
    {
        private static LCGRandom _random = new LCGRandom(0);

        private static uint magicNumber = uint.MaxValue / 256;

        public float Evaluate(float x)
        {
            var ix = Mathf.FloorToInt(x);
            var fx = x - ix;
            var p1 = fx;
            var p2 = fx - 1;

            var hash1 = RandomHash.Get(ix);
            var hash2 = RandomHash.Get(ix + 1);

            _random.Seed = (uint)(hash1 * magicNumber);
            var g1 = _random.NextFloat() * 4 - 2;
            _random.Seed = (uint)(hash2 * magicNumber);
            var g2 = _random.NextFloat() * 4 - 2;

            var v1 = g1 * p1;
            var v2 = g2 * p2;

            return MathUtility.SmoothLerp2(v1,v2,fx);
        }
    }
}
