using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DGS.Core.ObjectPool
{
    /// <summary>
    /// 모든 오브젝트 풀을 관리하는 모듈
    /// </summary>
    public class ObjectPoolModule : MonoBehaviour
    {
        private Dictionary<Type, IObjectPool> dictPool;
        
        /// <summary>
        /// 게임에서 사용할 오브젝트 풀을 세팅하는 함수.
        /// </summary>
        public void Init()
        {
            dictPool = new Dictionary<Type, IObjectPool>();
            
            AddMonoPool<Sticker>("Sticker", 10, false);
            AddPool<TestPoolingObject>(10, true);
        }
        
        public T GetPoolingObject<T>() where T : class, IPoolingObject
        {
            if (dictPool[typeof(T)].TryGetObject(out IPoolingObject result))
            {
                return result as T;
            }
            return null;
        }

        public bool TryGetPoolingObject<T>(out T poolingObject) where T : class, IPoolingObject
        {
            poolingObject = null;
            bool success = dictPool[typeof(T)].TryGetObject(out IPoolingObject result);
            if (success) poolingObject = result as T;

            return success;
        }

        /// <summary>
        /// MonoBehaviour를 상속받은 오브젝트의 풀을 추가한다. 
        /// </summary>
        private void AddMonoPool<T>(string address, int initCount, bool isFixed) where T : MonoBehaviour, IPoolingObject
        {
            GameObject newGameObject = new GameObject();
            newGameObject.transform.SetParent(gameObject.transform);
            newGameObject.name = address;
            MonoObjectPool<T> pool = new MonoObjectPool<T>();
            pool.Init(address, newGameObject.transform, initCount, isFixed);
            dictPool.Add(typeof(T), pool);
        }

        /// <summary>
        /// 일반 클래스 오브젝트의 풀을 추가한다.
        /// </summary>
        private void AddPool<T>(int initCount, bool isFixed) where T : class, IPoolingObject, new()
        {
            ObjectPool<T> pool = new ObjectPool<T>();
            pool.Init(initCount, isFixed);
            dictPool.Add(typeof(T), pool);
        }
    }
}