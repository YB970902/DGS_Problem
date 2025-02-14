using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DGS.Utils;

public class ObjectPool : MonoSingleton<ObjectPool>
{
    [SerializeField] List<GameObject> prefabs;
    [SerializeField] int poolSize;

    // currentPool : ������� ��ȯ���ְ� �ִ� Pool�� �ε�����
    // pivot : currentPool���� ��ȯ���ְ� �ִ� ������ ������Ʈ �ε���
    int currentPool, pivot;

    // �� Ǯ���� pivot�� ������. �׳� for�� ���鼭 ����ִ� ���� ���ָ� �������� �� ���� �ΰ� ���� �� ����
    int[] pivots;
    Transform[] transforms;
    [SerializeField] List<GameObject[]> pools = new List<GameObject[]>();

    protected override void OnInit()
    {
        base.OnInit();

        pools = new List<GameObject[]>();
        pivots = new int[poolSize];
        transforms = new Transform[poolSize];

        for (int i = 0; i < prefabs.Count; i++)
        {
            pivots[i] = 0;

            Transform newPoolTransform = new GameObject(prefabs[i].name + " Pool").transform;
            transforms[i] = newPoolTransform;

            newPoolTransform.transform.parent = transform;
            
            GameObject[] newPool = new GameObject[poolSize];
            pools.Add(newPool);

            for (int j = 0; j < poolSize; j++)
            {
                GameObject newObject = Instantiate(prefabs[i], Vector3.zero, Quaternion.identity); 

                newObject.SetActive(false);
                newObject.transform.SetParent(newPoolTransform);
                newPool[j] = newObject;
            }
        }
    }

    // id = prefabs �� ���� ������Ʈ ����
    public GameObject Get(int id)
    {
        // ����ڵ�
        if (id > pools.Count) return null;

        // ���� Ǯ�� �ٸ��� ���ο� Ǯ�� �°� ����
        if(id != currentPool)
        {
            // �������� ���� pivot�� ����
            pivots[currentPool] = pivot;

            // ���ο� Ǯ ����
            currentPool = id;
            pivot = pivots[currentPool];
        }

        // pivot�� poolSize�� ���� ��� 0���� ����
        if (pivot > poolSize - 1) pivot = 0;

        // Ȥ�ó� pivot�� ����Ű�� ������Ʈ�� �������� ��� ���
        if (pools[currentPool][pivot].activeSelf == false)
        {
            pivot++; pools[currentPool][pivot - 1].SetActive(true);
            return pools[currentPool][pivot - 1];
        }
        else // ���������� 0���� �������� ���� ������Ʈ ã��
        {
            for (int i = 0; i < pools.Count; i++)
            {
                if (pools[currentPool][i].activeSelf == false)
                { 
                    pivot = i + 1; 
                    pools[currentPool][pivot].SetActive(true); 

                    return pools[currentPool][i]; 
                }
            }

            // �� �����ִ� ��� null ��ȯ. Ǯ ����� Ű�� ���� �ֱ� �ѵ� �װ� ���ο� �ΰ� ¥�� �ʾҴ�.
            // Ȯ�� �����ϰ� ©�Ÿ� ������ ���ݰ��� ���� �޶����� �� ��
            return null;
        }
        
    }

    /// <summary>
    /// ���⼭�� ���� �� ������Ʈ�� ���� ��ȯ�ȴٴ� ���� �Ͽ� �ڵ带 ®���� ��ųʸ��� ����Ʈ�� �� ������Ʈ���� �� ����ϰ� ������
    /// ���ʿ��� ���� ���� ������尡 �߻����� �ʴ´�. ��� �����ִ� ������Ʈ�� ����ϰ� �ִ� �͵� �ڿ��� ���Ǵ� ������ �غ����� ��
    /// </summary>
    /// <param name="obj"> ��ȯ�� ������Ʈ </param>
    /// <param name="id"> ��ȯ�� ������Ʈ�� Ǯ �ε���</param>
    public void Release(GameObject obj, int id)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transforms[id], false);
        pivots[id]--;
    }

}
