using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DGS.Core.ObjectPool
{
    /// <summary>
    /// 오브젝트 풀에 들어갈 수 있는 GameObject.
    /// 이 클래스를 상속받으면 오브젝트 풀에 들어갈 수 있다.
    /// </summary>
    public abstract class MonoPoolingObject : MonoBehaviour, IPoolingObject
    {
        protected IObjectPool pool;
        
        /// <summary>
        /// 해당 오브젝트를 생성하기 위한 어드레스
        /// </summary>
        public static string Address { get; } 
        
        public void Init(IObjectPool objectPool)
        {
            pool = objectPool;
        }

        public bool IsInPool { get; private set; }

        public void Sleep()
        {
            IsInPool = true;
            OnSleep();
            gameObject.SetActive(false);
        }

        protected abstract void OnSleep();

        public void WakeUp()
        {
            IsInPool = false;
            OnWakeUp();
            gameObject.SetActive(true);
        }
        
        protected abstract void OnWakeUp();

        public void ReturnToPool()
        {
            if (IsInPool) return;
            
            pool.ReturnObject(this);
        }
    }
}