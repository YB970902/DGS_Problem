using System.Collections;
using System.Collections.Generic;
using DGS.Utils;
using UnityEngine;

namespace DGS
{
    /// <summary>
    /// 게임 매니저. 게임의 룰을 관리하는 역할을 한다.
    /// </summary>
    public class GameManager : MonoSingleton<GameManager>
    {
        [SerializeField] GameObject prefabCircle;
        /// <summary> 서클의 개수 </summary>
        [SerializeField] private int circleCount;

        protected override void OnInit()
        {
            for (int i = 0; i < circleCount; ++i)
            {
                SpawnCircle();
            }
        }
        
        /// <summary>
        /// 서클을 생성한다.
        /// </summary>
        private void SpawnCircle()
        {
            GameObject newCircle = Instantiate(prefabCircle);
            Vector2 newPosition = new Vector2();
            newPosition.x = Random.Range(Define.ObjectPoolCircle.MinPoint.x, Define.ObjectPoolCircle.MaxPoint.x);
            newPosition.y = Random.Range(Define.ObjectPoolCircle.MinPoint.y, Define.ObjectPoolCircle.MaxPoint.y);
            newCircle.transform.position = newPosition;
        }
    }
}