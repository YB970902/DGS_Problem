using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DGS.Utils;

public class ObjectPool : MonoSingleton<ObjectPool>
{
    [SerializeField] List<GameObject> prefabs;
    [SerializeField] int poolSize;

    // currentPool : 현재까지 반환해주고 있던 Pool의 인덱스값
    // pivot : currentPool에서 반환해주고 있던 마지막 오브젝트 인덱스
    int currentPool, pivot;

    // 매 풀마다 pivot을 저장함. 그냥 for문 돌면서 비어있는 곳을 해주면 편하지만 이 편이 싸게 먹힐 것 같음
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

    // id = prefabs 의 게임 오브젝트 순서
    public GameObject Get(int id)
    {
        // 방어코드
        if (id > pools.Count) return null;

        // 현재 풀과 다르면 새로운 풀에 맞게 세팅
        if(id != currentPool)
        {
            // 이전까지 쓰던 pivot값 저장
            pivots[currentPool] = pivot;

            // 새로운 풀 세팅
            currentPool = id;
            pivot = pivots[currentPool];
        }

        // pivot이 poolSize를 넘은 경우 0으로 리셋
        if (pivot > poolSize - 1) pivot = 0;

        // 혹시나 pivot이 가리키는 오브젝트가 켜져있을 경우 방어
        if (pools[currentPool][pivot].activeSelf == false)
        {
            pivot++; pools[currentPool][pivot - 1].SetActive(true);
            return pools[currentPool][pivot - 1];
        }
        else // 켜져있으면 0부터 나가있지 않은 오브젝트 찾기
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

            // 다 나가있는 경우 null 반환. 풀 사이즈를 키울 수도 있긴 한데 그걸 염두에 두고 짜질 않았다.
            // 확장 가능하게 짤거면 구조가 지금과는 조금 달라져야 할 듯
            return null;
        }
        
    }

    /// <summary>
    /// 여기서는 먼저 준 오브젝트가 먼저 반환된다는 가정 하에 코드를 짰지만 딕셔너리나 리스트로 준 오브젝트들을 다 기록하고 있으면
    /// 불필요한 변수 복제 오버헤드가 발생하지 않는다. 대신 나가있는 오브젝트를 기록하고 있는 것도 자원이 사용되니 생각은 해봐야할 듯
    /// </summary>
    /// <param name="obj"> 반환할 오브젝트 </param>
    /// <param name="id"> 반환할 오브젝트의 풀 인덱스</param>
    public void Release(GameObject obj, int id)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transforms[id], false);
        pivots[id]--;
    }

}
