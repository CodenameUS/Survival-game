using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
    }
    
    public void Show()
    {
        Next();
        rect.localScale = Vector3.one;  // 1,1,1
        GameManager.instance.Stop();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        AudioManager.instance.EffectBgm(true);
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        AudioManager.instance.EffectBgm(false);
    }

    public void Select(int index)
    {
        items[index].OnClick();
    }
    
    // .. 랜덤 활성화 함수
    void Next()
    {
        // 1. 모든 아이템 비활성화
        foreach(Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        // 2. 그 중 랜덤 3개 아이템 활성화 
        int[] rand = new int[3];
        
        while(true)
        {
            rand[0] = Random.Range(0, items.Length);
            rand[1] = Random.Range(0, items.Length);
            rand[2] = Random.Range(0, items.Length);

            // .. 중복 아이템 제거
            if (rand[0] != rand[1] && rand[1] != rand[2] && rand[0] != rand[2])
                break;
        }

        for(int index=0;index<rand.Length;index++)
        {
            Item ranItem = items[rand[index]];

            // 3. 만렙 아이템은 소비아이템으로 대체
            if(ranItem.level == ranItem.data.damages.Length)
            {
                items[Random.Range(4,7)].gameObject.SetActive(true);    // .. 힐 아이템으로 대체
            }
            else
            {
                ranItem.gameObject.SetActive(true);
            }

        }

    }
}
