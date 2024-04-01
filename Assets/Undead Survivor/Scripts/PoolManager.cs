using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // .. �������� ������ ����
    public GameObject[] prefabs;

    // .. Ǯ ����� ����Ʈ
    List<GameObject>[] pools;

    void Awake()
    {
        // .. ����Ʈ �ʱ�ȭ
        pools = new List<GameObject>[prefabs.Length];
        
        // .. Ǯ ����Ʈ ��� �ʱ�ȭ
        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // ... ������ Ǯ���� ��Ȱ��ȭ �� ���ӿ�����Ʈ ����
        foreach(GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                // ... ���� �� select ������ �Ҵ�
                select = item;
                select.SetActive(true);
                break;
            }
        }
        // ... �� ã���� ��
        if (select == null)
        {
            // ... ���Ӱ� ���� �ϰ� select ������ �Ҵ�
            // ... transform ���ڴ� Hierarchy �信 �������ϰ� ������ �ʵ��� PoolManager �Ʒ��� �����ϱ�����
            select = Instantiate(prefabs[index], transform);

            // ... ������ ������Ʈ�� Pool�� �߰�
            pools[index].Add(select);
        }
        return select;
    }
}
