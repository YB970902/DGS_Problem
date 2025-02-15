using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DGS.Core.ObjectPool
{
    public sealed class MonoObjectPool<T> : IObjectPool where T : MonoBehaviour, IPoolingObject
    {
        private bool isInit = false;
        private List<T> allPoolingObjects;
        private List<T> pool;
        private Transform transform;
        private string address;
        public bool IsFixed { get; private set; }
        private T prefab;

        /// <summary>
        /// 오브젝트풀을 초기화하는 함수.
        /// </summary>
        /// <param name="address"> 프리팹의 주소 </param>
        /// <param name="initCount"> 기본 크기. 이 개수만큼 미리 생성해둔다. </param>
        /// <param name="isFixed"> 풀에 오브젝트가 없을때 더이상 생성하지 않을것인지 옵션이다. </param>
        public void Init(string address, Transform transform, int initCount, bool isFixed)
        {
            isInit = true;
            this.address = address;
            this.transform = transform;
            IsFixed = isFixed;
            prefab = Resources.Load<T>(address);

            allPoolingObjects = new List<T>(initCount);
            pool = new List<T>(initCount);
            for (int i = 0; i < initCount; ++i)
            {
                pool.Add(CreateObject());
            }
        }

        /// <summary>
        /// 오브젝트 생성에 실패할 수 있으므로, 반환타입으로는 성공 여부를 반환한다.
        /// 이를통해 풀로부터 객체를 받을때 예외처리를 강제할 수 있다.
        /// </summary>
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
                Debug.LogError($"잘못된 타입의 오브젝트가 풀에 들어오려고 시도함. 풀 : {GetType()}, 오브젝트 타입 : {poolingObject.GetType()}");
                return;
            }
            
            poolingObject.Sleep();
            T monoPoolingObject = poolingObject as T;
            monoPoolingObject.transform.SetParent(transform);
            pool.Add(monoPoolingObject);
        }
        
        public void ReturnAllObject()
        {
            allPoolingObjects.ForEach(_ => _.ReturnToPool());
        }

        private T CreateObject()
        {
            T result = GameObject.Instantiate(prefab, transform);
            result.Init(this);
            result.Sleep();
            allPoolingObjects.Add(result);
            
            return result;
        }
    }
}