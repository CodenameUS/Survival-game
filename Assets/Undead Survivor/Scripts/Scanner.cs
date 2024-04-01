using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange;         // ... ��ĵ�� ����
    public LayerMask targetLayer;   // ... ���̾ ���� ����
    public RaycastHit2D[] targets;  // ... ��ĵ ���
    public Transform nearsetTarget; // ... ���� ����� Ÿ��

    void FixedUpdate()
    {
        // ������ ĳ��Ʈ�� ��� ��� ��� ��ȯ
        // ... (ĳ���ý�����ġ, ���ǹ�����, ĳ���ù���, ĳ���ñ���, ����̾�)
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        nearsetTarget = GetNearest();
    }

    // ... ���� ����� ��ã��
    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        // ... ��ĵ����� ���鼭 ���� ����� Ÿ���� ����ؼ� ������Ʈ
        foreach(RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;             // ... �÷��̾� ��ġ
            Vector3 targetPos = target.transform.position;  // ... Ÿ���� ��ġ

            float curDiff = Vector3.Distance(myPos, targetPos); // ... �Ÿ�����

            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;
    }
}
