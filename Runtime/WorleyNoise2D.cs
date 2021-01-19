using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MS.Noise{


    public class WorleyNoise2D : INoise2D
    {


        private static LCGRandom _lcgRandom = new LCGRandom(0);

        private static float SQRT2 = Mathf.Sqrt(2);

        private DistanceAlgorithm _distanceAlgorithm;
        private System.Func<Vector2,Vector2,float> _distanceCalculator;
        private System.Func<float,float> _distanceNormalize;

        private int _maxPointsInCell;

        public WorleyNoise2D():this(DistanceAlgorithm.Euclidean,3){
        }

        /// <summary>
        /// 根据距离算法和最大特征点数量构造WorleyNoise
        /// </summary>
        /// <param name="distanceAlgorithm">采用的距离算法</param>
        /// <param name="maxPointsInCell">单个晶胞内，生成的最大特征点数量</param>
        public WorleyNoise2D(DistanceAlgorithm distanceAlgorithm,int maxPointsInCell){
            _distanceAlgorithm = distanceAlgorithm;
            _maxPointsInCell = Mathf.Max(1,maxPointsInCell);
            switch(distanceAlgorithm){
                case DistanceAlgorithm.Chebyshev:
                _distanceCalculator = MathUtility.ChebyshevDistance;
                _distanceNormalize = (dis)=>{
                    return dis * 2 -1;
                };
                break;
                case DistanceAlgorithm.Euclidean:
                _distanceCalculator = MathUtility.EuclideanDistance;
                _distanceNormalize = (dis)=>{
                    return dis * SQRT2 - 1;
                };
                break;
                case DistanceAlgorithm.Manhattan:
                _distanceCalculator = MathUtility.ManhattanDistance;
                _distanceNormalize = (dis)=>{
                    return dis - 1;
                };
                break;
            }
        }

        private int _printCount = 0;
        public float Evaluate(float x, float y)
        {
            var xy = new Vector2(x,y);
            var ix = Mathf.FloorToInt(x);
            var iy = Mathf.FloorToInt(y);
            var minDis = float.MaxValue;
            for(var i = ix - 1;i <= ix + 1; i ++){
                for(var j = iy - 1;j <= iy + 1; j ++){
                    var hash = RandomHash.Get(i,j);
                    _lcgRandom.Seed = (uint)(hash * (uint.MaxValue >> 9));
                    var checkPointCount = _lcgRandom.NextRange(1,_maxPointsInCell + 1);
                    for(var idx = 0; idx < checkPointCount; idx++){
                        var rx = i + _lcgRandom.NextFloat();
                        var ry = j + _lcgRandom.NextFloat();
                        var dis = _distanceCalculator(xy,new Vector2(rx,ry));
                        minDis = Mathf.Min(dis,minDis);
                    }
                }
            }
            return _distanceNormalize(minDis);
        }



    }
}
