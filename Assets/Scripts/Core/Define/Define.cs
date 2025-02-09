using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DGS.Define
{
    public static class ObjectPoolCircle
    {
        /// <summary> 서클이 위치할 수 있는 최소 범위 </summary>
        public static readonly Vector2 MinPoint = new Vector2(-10f, -10f);
        /// <summary> 서클이 위치할 수 있는 최대 범위 </summary>
        public static readonly Vector2 MaxPoint = new Vector2(10f, 10f);
    }
}
