using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DGS.Core.ObjectPool
{
    /// <summary>
    /// 오브젝트 풀에 들어갈 수 있는 클래스.
    /// 이 클래스를 상속받으면 풀에 들어갈 수 있다.
    /// </summary>
    public class PoolingObject : IPoolingObject
    {
        /// <summary>
        /// 자신이 속해있는 풀
        /// </summary>
        protected IObjectPool pool;
        
        public void Init(IObjectPool objectPool)
        {
            pool = objectPool;
        }

        public bool IsInPool { get; private set; }

        public void Sleep()
        {
            IsInPool = true;
            OnSleep();
        }

        protected virtual void OnSleep() { }

        public void WakeUp()
        {
            IsInPool = false;
            OnWakeUp();
        }
        
        protected virtual void OnWakeUp() { }

        public void ReturnToPool()
        {
            if (IsInPool) return;
            
            pool.ReturnObject(this);
        }
    }
}
