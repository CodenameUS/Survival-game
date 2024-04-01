using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float damage;        // ... �Ѿ� ������
    public int per;             // ... ����

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();   
    }

    // ... ���� �ʱ�ȭ
    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;       // ... ������ damge�� Bullet Ŭ������ damage, �������� �Ű������� damage
        this.per = per;

        // ... ������ ���Ѻ��� ū��(���Ÿ�����)
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

        // ... �Ҹ��� ���� ������ ��
        if(per < 0)
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    // .. ����ü ����
    private void OnTriggerExit2D(Collider2D collision)
    {
        // .. Player�� Area�� ����� ����
        if(!collision.CompareTag("Area") || per == -100)
            return;

        gameObject.SetActive(false);
    }
}
