using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner;
    public Hand[] hands;
    public RuntimeAnimatorController[] animCon;

    SpriteRenderer sprite;
    Rigidbody2D rigid;
    Animator anim;

    // ... 시작 시 한번만 실행되는 함수 Awake
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);    // .. 비활성화된 오브젝트
    }

    void OnEnable()
    {
        speed *= Character.Speed;
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];
    }

    // ... 물리 연산 프레임마다 호출되는 함수 FixedUpdate
    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;    
        // ... 플레이어의 위치(좌표) 이동
        rigid.MovePosition(rigid.position + nextVec);
    }

    // ... 인풋 받기
    void OnMove(InputValue value)
    {
        if (!GameManager.instance.isLive)
            return;

        inputVec = value.Get<Vector2>();        
    }

    
    void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        anim.SetFloat("Speed", inputVec.magnitude);

        if (inputVec.x != 0)
        {
            // ... 플레이어가 보는 방향 변경
            sprite.flipX = inputVec.x < 0;
        }
    }


    // .. 플레이어 피격
    void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive)
            return;

        GameManager.instance.health -= Time.deltaTime * 10;

        // .. 플레이어 사망 시
        if(GameManager.instance.health < 0)
        {
            // .. 플레이어의 자식 오브젝트 비활성화
            for(int index = 2;index<transform.childCount;index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }

            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }    
    }
}
