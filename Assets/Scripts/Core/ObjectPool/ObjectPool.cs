using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DGS.Core.ObjectPool
{
    public sealed class ObjectPool<T> : IObjectPool where T : class, IPoolingObject, new()
    {
        private bool isInit = false;
        public bool IsFixed { get; private set; }
        private List<T> allPoolingObjects;
        private List<T> pool;

        public void Init(int initCount, bool isFixed)
        {
            isInit = true;
            IsFixed = isFixed;
            allPoolingObjects = new List<T>(initCount);
            pool = new List<T>(initCount);
            for (int i = 0; i < initCount; ++i)
            {
                pool.Add(CreateObject());
            }
        }

        public bool TryGetObject(out IPoolingObject poolingObject)
        {
            poolingObject = null;

            if (pool.Count == 0)
            {
                if (IsFixed) return false;
                poolingObject = CreateObject();
            }
            else
            {
                poolingObject = pool[^1];
                pool.RemoveAt(pool.Count - 1);
            }
            
            poolingObject.WakeUp();

            return true;
        }

        public void ReturnObject(IPoolingObject poolingObject)
        {
            if (poolingObject is not T)
            {
                Debug.LogError($"잘못된 타입의 오브젝트가 풀에 들어오려고 시도함. 풀 타입 : {GetType()}, 오브젝트 타입 : {poolingObject.GetType()}");
                return;
            }
            
            poolingObject.Sleep();
            pool.Add(poolingObject as T);
        }

        public void ReturnAllObject()
        {
            allPoolingObjects.ForEach(_ => _.ReturnToPool());
        }
        
        private T CreateObject()
        {
            T newObject = new T();
            newObject.Init(this);
            newObject.Sleep();
            
            return newObject;
        }
    }
}