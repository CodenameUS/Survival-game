using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;         // ... ���� �ӵ�
    public float health;        // ... ����ü��
    public float maxHealth;     // ... �ִ�ü��

    public RuntimeAnimatorController[] animCont;    // ... �ִϸ����� ��Ʈ�ѷ�
    public Rigidbody2D target;  // ... Ÿ��(�÷��̾�)

    bool isLive;              // ... ���� ��������

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

    // ... �������� ���� �� �� Target(�÷��̾�) ã��
    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;    // ... ������ٵ� ��Ȱ��ȭ�� simulated
        sprite.sortingOrder = 2;
        anim.SetBool("Dead", false);

        health = maxHealth;     // ... hp ���󺹱�
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        // ... Hit �����̸� ����
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;
       
        // ... �÷��̾�� ���� ��ġ ����
        Vector2 dirVec = target.position - rigid.position;  
        // ... ���� ���� ������ ���� ����
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
        // ... ���Ͱ� �ٶ󺸴� ���⼳��(�ִϸ��̼�)
        sprite.flipX = target.position.x < rigid.position.x;    
    }

    // ... ���� �ʱ�ȭ
    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCont[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    // ... �Ҹ��� �浹 �̺�Ʈ
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());

        if (health > 0)
        {
            // ... ü���� �������� ���
            anim.SetTrigger("Hit"); // ... �ִϸ��̼�
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else  // ... �׾��� ��
        {
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;    // ... ������ٵ� ��Ȱ��ȭ�� simulated
            sprite.sortingOrder = 1;
            anim.SetBool("Dead", true);

            // ... ų������, exp����
            GameManager.instance.kill++;
            GameManager.instance.GetExp();

            if (GameManager.instance.isLive)
            {
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
            }
        }
        
    }

    // ... �ڷ�ƾ : �����ֱ�� �񵿱�ó�� ����Ǵ� �Լ�
    IEnumerator KnockBack()
    {
        yield return wait; // ... ���� �ϳ��� ���� ������ ������
        Vector3 playerPos = GameManager.instance.player.transform.position; // ... �÷��̾� ��ġ
        Vector3 dirVec = transform.position - playerPos;    // ... �÷��̾� ���� �ݴ����
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse); // ... �ݴ�������� �� ���ϱ�
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}
