using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ... ���ġ ��ũ��Ʈ(Ÿ�ϸ�, ����)
public class Reposition : MonoBehaviour
{
    Collider2D coll;

    void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // ... �÷��̾�(Area)�� �ƴϸ� ����
        if (!collision.CompareTag("Area"))
            return;

        // ... �÷��̾��� ��ġ�� ������
        Vector3 playerPos = GameManager.instance.player.transform.position; 
        // ... ���� Ÿ�Ը��� ��ġ�� ������
        Vector3 myPos = transform.position;


        // ... Ÿ�ϸ�/���� ��ġ ����
        switch (transform.tag)
        {
            case "Ground":
                // .. �� ������Ʈ�� ��ġ ���̸� Ȱ��
                float diffX = playerPos.x - myPos.x;
                float diffY = playerPos.y - myPos.y;
                float dirX = diffX < 0 ? -1 : 1;
                float dirY = diffY < 0 ? -1 : 1;
                diffX = Mathf.Abs(diffX);
                diffY = Mathf.Abs(diffY);

                // ... Ÿ�ϸ� ��ġ�� �÷��̾ ���� �ִ� ���� ������ ����
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
                if (coll.enabled)   // ... ���Ͱ� ������� ���
                {
                    Vector3 dist = playerPos - myPos;
                    Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                    // ... �÷��̾��� �������� ��Ÿ�����ϱ�
                    transform.Translate(ran + dist * 2);      
                }
                break;
        }
    }
}
