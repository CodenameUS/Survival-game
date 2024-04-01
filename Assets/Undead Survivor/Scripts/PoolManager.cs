using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // .. 프리팹을 보관할 변수
    public GameObject[] prefabs;

    // .. 풀 담당할 리스트
    List<GameObject>[] pools;

    void Awake()
    {
        // .. 리스트 초기화
        pools = new List<GameObject>[prefabs.Length];
        
        // .. 풀 리스트 요소 초기화
        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // ... 선택한 풀에서 비활성화 된 게임오브젝트 접근
        foreach(GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                // ... 있을 때 select 변수에 할당
                select = item;
                select.SetActive(true);
                break;
            }
        }
        // ... 못 찾았을 때
        if (select == null)
        {
            // ... 새롭게 생성 하고 select 변수에 할당
            // ... transform 인자는 Hierarchy 뷰에 지저분하게 보이지 않도록 PoolManager 아래에 생성하기위함
            select = Instantiate(prefabs[index], transform);

            // ... 생성된 오브젝트를 Pool에 추가
            pools[index].Add(select);
        }
        return select;
    }
}
