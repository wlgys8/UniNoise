using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MS.Noise{
    internal class RandomGrad
    {

        public static int Get(int hash){
            return (hash & 1) == 0 ? 1 : -1;
        }
        
        public static Vector2Int GetVector(int hash){
            return new Vector2Int((hash & 1) == 0 ? 1 : -1,(hash & 2) == 0 ? 1 : -1);
        }   
    }
}
