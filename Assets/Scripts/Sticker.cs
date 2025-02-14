using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Sticker : MonoBehaviour
{
    public IObjectPool<Sticker> Pool { get; set; }

    void OnEnable()
    {
        StartCoroutine(DDestroy());
    }
    IEnumerator DDestroy()
    {
        yield return new WaitForSeconds(3f);
        Pool.Release(this);
    }
}
