using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private List<GameObject> objectPool;
    private GameObject objPrefab;

    /// <summary>
    /// pool에 size 만큼의 prefab obj를 생성해서 넣음
    /// </summary>
    public void CreatePool(int _size, GameObject _prefab)
    {
        objectPool = new List<GameObject>();
        for (int i = 0; i < _size; i++)
        {
            GameObject obj = Instantiate(_prefab, transform);
            obj.SetActive(false);
            obj.transform.SetParent(transform);
            objectPool.Add(obj);
        }
    }

    /// <summary>
    /// pool에서 obj 하나 꺼내고 obj 활성화
    /// pool이 비어있으면 obj를 새로 생성하여 반환
    /// </summary>
    public GameObject GetObject()
    {
        if (objectPool.Count == 0)
        {
            return Instantiate(objPrefab);
        }
        GameObject removed = objectPool[0];
        objectPool.RemoveAt(0);
        removed.SetActive(true);
        return removed;
    }
    
    /// <summary>
    /// pool에서 obj를 하나 꺼내 인수로 받은 transform의 자식으로 설정
    /// </summary>
    public GameObject GetObjectAsChild(Transform _parent)
    {
        GameObject removed = GetObject();
        removed.transform.SetParent(_parent);
        removed.transform.localPosition = Vector3.zero;
        return removed;
    }

    /// <summary>
    /// pool에 obj 반납
    /// </summary>
    public void ReturnObject(GameObject _obj)
    {
        _obj.SetActive(false);
        _obj.transform.SetParent(transform);
        objectPool.Add(_obj);
    }
}
