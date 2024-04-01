using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ... 재배치 스크립트(타일맵, 몬스터)
public class Reposition : MonoBehaviour
{
    Collider2D coll;

    void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // ... 플레이어(Area)가 아니면 무시
        if (!collision.CompareTag("Area"))
            return;

        // ... 플레이어의 위치를 가져옴
        Vector3 playerPos = GameManager.instance.player.transform.position; 
        // ... 현재 타입맵의 위치를 가져옴
        Vector3 myPos = transform.position;


        // ... 타일맵/몬스터 위치 변경
        switch (transform.tag)
        {
            case "Ground":
                // .. 두 오브젝트의 위치 차이를 활용
                float diffX = playerPos.x - myPos.x;
                float diffY = playerPos.y - myPos.y;
                float dirX = diffX < 0 ? -1 : 1;
                float dirY = diffY < 0 ? -1 : 1;
                diffX = Mathf.Abs(diffX);
                diffY = Mathf.Abs(diffY);

                // ... 타일맵 위치를 플레이어가 가고 있는 방향 쪽으로 변경
                if (diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 40);     
                }
                else if (diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;
            case "Enemy":
                if (coll.enabled)   // ... 몬스터가 살아있을 경우
                {
                    Vector3 dist = playerPos - myPos;
                    Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                    // ... 플레이어의 맞은편에서 나타나게하기
                    transform.Translate(ran + dist * 2);      
                }
                break;
        }
    }
}
