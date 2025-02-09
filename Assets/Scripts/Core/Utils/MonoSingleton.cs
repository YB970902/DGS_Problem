using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DGS.Utils
{
    public class MonoSingleton<T> : MonoBehaviour where T : Component
    {
        private static T instance;
        private static bool isInit = false;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindAnyObjectByType<T>();
                    if (instance == null)
                    {
                        GameObject obj = new GameObject(nameof(T));
                        instance = obj.AddComponent<T>();
                        DontDestroyOnLoad(obj);
                    }
                }

                return instance;
            }
        }

        protected virtual void Start()
        {
            if (isInit)
            {
                Destroy(gameObject);
                return;
            }

            isInit = true;

            OnInit();
        }

        protected virtual void OnInit()
        {
        }
    }
}