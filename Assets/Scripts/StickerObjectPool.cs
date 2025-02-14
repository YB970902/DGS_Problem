using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class StickerObjectPool : MonoBehaviour
{
    //�⺻ Ǯ ũ��
    private int defaultPoolSize = 5;

    //Ǯ�� ������ �ν��Ͻ� �ִ� ����
    public int maxPoolSize = 10;

    public IObjectPool<Sticker> Pool
    {
        get
        {
            if (_pool == null)
                _pool = new ObjectPool<Sticker>(
                                        CreatedPooledItem,
                                        OnTakeFromPool,
                                        OnReturnedToPool,
                                        OnDestroyPoolObject,
                                        true,
                                        defaultPoolSize,
                                        maxPoolSize);
            return _pool;
        }
    }

    private IObjectPool<Sticker> _pool;

    private Sticker CreatedPooledItem()
    {
        //������ ��ġ ����
        Sticker stickerPrefab = Resources.Load<Sticker>("Prefabs/Sticker");
        //�ν��Ͻ�
        var sticker = GameObject.Instantiate(stickerPrefab);
        sticker.Pool = Pool;

        return sticker;
        
    }

    private void OnReturnedToPool(Sticker _sticker)
    {
        _sticker.gameObject.SetActive(false);
    }

    private void OnTakeFromPool(Sticker _sticker)
    {
        _sticker.gameObject.SetActive(true);
    }

    private void OnDestroyPoolObject(Sticker _sticker)
    {
        Destroy(_sticker.gameObject);
    }

    public void Spawn()
    {
        var sticker = Pool.Get();
    }
}
