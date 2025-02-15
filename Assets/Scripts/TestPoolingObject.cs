using System.Collections;
using System.Collections.Generic;
using DGS.Core.ObjectPool;
using UnityEngine;

public class TestPoolingObject : PoolingObject
{
    protected override void OnSleep()
    {
        Debug.Log("TestPooling Sleep");
    }

    protected override void OnWakeUp()
    {
        Debug.Log("TestPooling WakeUp");
    }
}
