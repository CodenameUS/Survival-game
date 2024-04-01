using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange;         // ... 스캔할 범위
    public LayerMask targetLayer;   // ... 레이어를 담을 변수
    public RaycastHit2D[] targets;  // ... 스캔 결과
    public Transform nearsetTarget; // ... 가장 가까운 타겟

    void FixedUpdate()
    {
        // 원형의 캐스트를 쏘고 모든 결과 반환
        // ... (캐스팅시작위치, 원의반지름, 캐스팅방향, 캐스팅길이, 대상레이어)
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        nearsetTarget = GetNearest();
    }

    // ... 가장 가까운 적찾기
    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        // ... 스캔결과를 돌면서 가장 가까운 타겟을 계속해서 업데이트
        foreach(RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;             // ... 플레이어 위치
            Vector3 targetPos = target.transform.position;  // ... 타겟의 위치

            float curDiff = Vector3.Distance(myPos, targetPos); // ... 거리차이

            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;
    }
}
