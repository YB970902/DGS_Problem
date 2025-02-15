using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DGS.Core.ObjectPool
{
    public interface IObjectPool
    {
        public bool TryGetObject(out IPoolingObject poolingObject);
        
        public void ReturnObject(IPoolingObject poolingObject);
        
        /// <summary>
        /// 모든 오브젝트를 풀로 불러들이는 함수.
        /// </summary>
        public void ReturnAllObject();
    }
}