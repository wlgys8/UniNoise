using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MS.Noise{


    public enum DistanceAlgorithm{
        Euclidean,
        Manhattan,
        Chebyshev
    }


    internal static class MathUtility 
    {
        
        public static float SmoothLerp2(float a, float b, float t){
            var t2 = t * t;
            var t3 = t2 * t;
            var t4 = t2 * t2;
            var t5 = t4 * t;
            float k =  t5 * 6 - t4 * 15 + t3 * 10;
            return (1 - k) * a + k * b;
        }

        public static float ManhattanDistance(Vector2 v1,Vector2 v2){
            var d = v2 - v1;
            return Mathf.Abs(d.x) + Mathf.Abs(d.y); 
        }

        public static float EuclideanDistance(Vector2 v1,Vector2 v2){
            return Vector2.Distance(v2,v1);
        }

        public static float ChebyshevDistance(Vector2 v1,Vector2 v2){
            var d = v2 - v1;
            return Mathf.Max(Mathf.Abs(d.x),Mathf.Abs(d.y));
        }

        /// <summary>
        /// if b <= 0, return a;
        /// if b > 0 and a > 0 then return mod(a,b);
        /// if b > 0 and a < 0 then return (a%b + b)%b
        /// </summary>
        public static int ModPositive(int a,int b){
            if(b <= 0){
                return a;
            }
            a = a %= b;
            if(a < 0){
                a += b;
            }
            return a;
        }
    }
}
