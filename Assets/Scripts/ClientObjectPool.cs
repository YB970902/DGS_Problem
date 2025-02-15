using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientObjectPool : MonoBehaviour
{
    private StickerObjectPool _pool;

    private void Start()
    {
        _pool = gameObject.AddComponent<StickerObjectPool>();
    }

    private void OnGUI()
    {
        if(GUILayout.Button("Spawn Drones"))
            _pool.Spawn();
    }
}
