using UnityEngine;


namespace MS.Noise{
    public class WhiteNoise2D : INoise2D
    {
        public float Evaluate(float x, float y)
        {
            var ix = Mathf.FloorToInt(x);
            var iy = Mathf.FloorToInt(y);
            return RandomHash.Get(ix,iy)/128f - 1;
        }
    }
}
