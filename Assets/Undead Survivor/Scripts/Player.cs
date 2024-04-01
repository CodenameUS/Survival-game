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

    // ... ���� �� �ѹ��� ����Ǵ� �Լ� Awake
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);    // .. ��Ȱ��ȭ�� ������Ʈ
    }

    void OnEnable()
    {
        speed *= Character.Speed;
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];
    }

    // ... ���� ���� �����Ӹ��� ȣ��Ǵ� �Լ� FixedUpdate
    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;    
        // ... �÷��̾��� ��ġ(��ǥ) �̵�
        rigid.MovePosition(rigid.position + nextVec);
    }

    // ... ��ǲ �ޱ�
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
            // ... �÷��̾ ���� ���� ����
            sprite.flipX = inputVec.x < 0;
        }
    }


    // .. �÷��̾� �ǰ�
    void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive)
            return;

        GameManager.instance.health -= Time.deltaTime * 10;

        // .. �÷��̾� ��� ��
        if(GameManager.instance.health < 0)
        {
            // .. �÷��̾��� �ڽ� ������Ʈ ��Ȱ��ȭ
            for(int index = 2;index<transform.childCount;index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }

            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }    
    }
}
