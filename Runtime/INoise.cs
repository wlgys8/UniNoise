using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MS.Noise{

    public interface INoise1D{

        float Evaluate(float x);
    }

    public interface INoise2D
    {
        float Evaluate(float x,float y);

    }
}
