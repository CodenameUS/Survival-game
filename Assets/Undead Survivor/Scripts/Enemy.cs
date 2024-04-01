using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;         // ... 몬스터 속도
    public float health;        // ... 현재체력
    public float maxHealth;     // ... 최대체력

    public RuntimeAnimatorController[] animCont;    // ... 애니메이터 컨트롤러
    public Rigidbody2D target;  // ... 타겟(플레이어)

    bool isLive;              // ... 몬스터 생존여부

    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer sprite;
    WaitForFixedUpdate wait;
    Collider2D coll;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        wait = new WaitForFixedUpdate();
    }

    // ... 프리팹이 생성 될 때 Target(플레이어) 찾기
    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;    // ... 리지드바드 비활성화는 simulated
        sprite.sortingOrder = 2;
        anim.SetBool("Dead", false);

        health = maxHealth;     // ... hp 원상복구
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        // ... Hit 상태이면 무시
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;
       
        // ... 플레이어와 몬스터 위치 차이
        Vector2 dirVec = target.position - rigid.position;  
        // ... 몬스터 다음 움직일 방향 설정
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        if (!isLive)
            return;
        // ... 몬스터가 바라보는 방향설정(애니메이션)
        sprite.flipX = target.position.x < rigid.position.x;    
    }

    // ... 몬스터 초기화
    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCont[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    // ... 불릿과 충돌 이벤트
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());

        if (health > 0)
        {
            // ... 체력이 남아있을 경우
            anim.SetTrigger("Hit"); // ... 애니메이션
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else  // ... 죽었을 때
        {
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;    // ... 리지드바드 비활성화는 simulated
            sprite.sortingOrder = 1;
            anim.SetBool("Dead", true);

            // ... 킬수증가, exp증가
            GameManager.instance.kill++;
            GameManager.instance.GetExp();

            if (GameManager.instance.isLive)
            {
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
            }
        }
        
    }

    // ... 코루틴 : 생명주기와 비동기처럼 실행되는 함수
    IEnumerator KnockBack()
    {
        yield return wait; // ... 다음 하나의 물리 프레임 딜레이
        Vector3 playerPos = GameManager.instance.player.transform.position; // ... 플레이어 위치
        Vector3 dirVec = transform.position - playerPos;    // ... 플레이어 기준 반대방향
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse); // ... 반대방향으로 힘 가하기
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}
