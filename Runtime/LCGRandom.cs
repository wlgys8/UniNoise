using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MS.Noise{
    public class LCGRandom
    {
        private uint _a;
        private uint _c;
        private uint _m;

        private uint _seed;

        private uint _value;

        public LCGRandom(uint a,uint c,uint m,uint seed){
            _a = a;
            _c = c;
            _m = m;
            _seed = seed;
            _value = seed;
        }

        public LCGRandom(uint seed):this(1664525,1013904223,uint.MaxValue,seed){
        }
  
        public uint Seed{
            get{
                return _seed;
            }set{
                _seed = value;
                _value = value;
            }
        }

        public uint Next(){
            _value = (_a * _value + _c) % _m;
            return _value;
        }

        public int NextInt(){
            return (int)this.Next();
        }

        /// <summary>
        /// return a float value between [0,1)
        /// </summary>
        public float NextFloat(){
            var uintValue = Next();
            return (float)(uintValue * 1.0/ uint.MaxValue);
        }

        /// <summary>
        /// generate random int value between min(include) and max(exclude)
        /// </summary>
        public int NextRange(int min,int max){
            return Mathf.FloorToInt(min + NextFloat() * (max - min));
        }


        private const uint A = 1664525;
        private const uint C = 1013904223;
        private const uint M = uint.MaxValue;

        public static uint Get(uint seed){
            return (A * seed + C) % M;
        }

    }
}
