using System.Collections;
using System.Collections.Generic;
using DGS;
using DGS.Core.ObjectPool;
using UnityEngine;

public class Sticker : MonoPoolingObject
{
    private TestPoolingObject testPoolingObject = null;
    
    protected override void OnSleep()
    {
        Debug.Log("Sleep");
        testPoolingObject?.ReturnToPool();
        testPoolingObject = null;
    }

    protected override void OnWakeUp()
    {
        Debug.Log("Wakeup");
        testPoolingObject = GameManager.Instance.ObjectPoolModule.GetPoolingObject<TestPoolingObject>();
    }
}
