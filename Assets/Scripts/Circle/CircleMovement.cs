using System;
using System.Collections;
using System.Collections.Generic;
using DGS;
using DGS.Define;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

/// <summary>
/// 서클의 움직임을 담당하는 컴포넌트
/// 서클은 맵의 범위 내에서만 움직인다. 만약 맵의 범위를 벗어나면 반대편으로 이동한다.
/// x의 최솟값보다 작다면 최댓값의 위치로 이동하고, x의 최댓값보다 크다면 최솟값의 위치로 이동한다.
/// y축도 동일하다.
/// </summary>
public class CircleMovement : MonoBehaviour
{
    /// <summary> 이동할 방향 </summary>
    private Vector3 dir;

    /// <summary> 이동속도 </summary>
    [SerializeField] float moveSpeed;

    /// <summary> 반지름 </summary>
    [SerializeField] private float radius;

    private Sticker sticker;
    
    private void Start()
    {
        sticker = null;
        
        float randomAngle = Random.Range(0f, Mathf.PI * 2);
        dir = new Vector2();
        dir.x = Mathf.Cos(randomAngle);
        dir.y = Mathf.Sin(randomAngle);
    }

    private void Update()
    {
        Vector3 position = transform.position + dir * (moveSpeed * Time.deltaTime);
        
        if (position.x < ObjectPoolCircle.MinPoint.x) position.x = ObjectPoolCircle.MaxPoint.x;
        else if (position.x > ObjectPoolCircle.MaxPoint.x) position.x = ObjectPoolCircle.MinPoint.x;
        if (position.y < ObjectPoolCircle.MinPoint.x) position.y = ObjectPoolCircle.MaxPoint.y;
        else if (position.y > ObjectPoolCircle.MaxPoint.x) position.y = ObjectPoolCircle.MinPoint.y;

        transform.position = position;

        if (IsInCamera() && sticker == null)
        {
            if (GameManager.Instance.ObjectPoolModule.TryGetPoolingObject<Sticker>(out sticker))
            {
                sticker.transform.SetParent(transform);
                sticker.transform.localPosition = Vector3.zero;
            }
        }
        else if(IsInCamera() == false && sticker != null)
        {
            sticker.ReturnToPool();
            sticker = null;
        }
    }

    /// <summary>
    /// 서클이 카메라 안에 있는지 여부
    /// </summary>
    private bool IsInCamera()
    {
        Camera cam = Camera.main;

        float cameraSize = cam.orthographicSize;
        // 종횡비
        float aspectRatio = (float)Screen.width / Screen.height;
        
        // 카메라 크기
        float camWidth = aspectRatio * cameraSize;
        float camHeight = cameraSize;

        Vector3 position = transform.position;

        return position.x + radius >= -camWidth && position.x - radius <= camWidth &&
               position.y + radius >= -camHeight && position.y - radius <= camHeight;
    }
}
