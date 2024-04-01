using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float damage;        // ... 총알 데미지
    public int per;             // ... 관통

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();   
    }

    // ... 변수 초기화
    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;       // ... 왼쪽의 damge는 Bullet 클래스의 damage, 오른쪽은 매개변수의 damage
        this.per = per;

        // ... 관통이 무한보다 큰것(원거리무기)
        if(per >= 0)
        {
            rigid.velocity = dir * 15f;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {   
        if (!collision.CompareTag("Enemy") || per == -100)
            return;

        per--;

        // ... 불릿이 힘을 다했을 때
        if(per < 0)
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    // .. 투사체 삭제
    private void OnTriggerExit2D(Collider2D collision)
    {
        // .. Player의 Area를 벗어나면 삭제
        if(!collision.CompareTag("Area") || per == -100)
            return;

        gameObject.SetActive(false);
    }
}
