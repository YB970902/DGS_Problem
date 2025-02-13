using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject[] poolingObj;
    [SerializeField] private int value;
    static private Queue<GameObject> pool = new Queue<GameObject>();
    static private Transform ts;

    private void Awake()
    {
        ts = transform;

        for (int i = 0; i < poolingObj.Length; i++)
        {
            for (int j = 0; j < value; j++)
            {
                GameObject obj = Instantiate(poolingObj[i]);
                obj.transform.SetParent(this.transform);
                obj.name = (j + 1).ToString() + " | "+ poolingObj[i].ToString();
                obj.SetActive(false);
                pool.Enqueue(obj);
            }
        }
    }

    static public void EnablePool(Transform t)
    {
        pool.Peek().gameObject.SetActive(true);
        pool.Peek().transform.SetParent(t.transform);
        pool.Peek().transform.localPosition = Vector2.zero;
        pool.Dequeue();
    }

    static public void DisablePool(GameObject g)
    {
        g.gameObject.SetActive(false);
        g.transform.SetParent(ts);
        pool.Enqueue(g);
    }
}
