using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AchiveManager : MonoBehaviour
{

    public GameObject[] lockCharacter;      // .. 잠금된 캐릭 목록
    public GameObject[] unlockCharacter;    // .. 해금될 캐릭 목록
    public GameObject uiNotice;             // .. 해금 알림 UI

    // .. 업적 종류
    enum Achive { UnlockGrape,  UnlockStrawberry }
    Achive[] achives;
    WaitForSecondsRealtime wait;

    void Awake()
    {
        achives = (Achive[])Enum.GetValues(typeof(Achive));
        wait = new WaitForSecondsRealtime(5);

        // .. 이전의 데이터가 있는지 확인
        if(!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
    }


    // .. Device 저장 데이터 초기화 함수
    void Init()
    {
        // .. PlayerPrefs : 간단한 저장 기능 제공하는 클래스
        PlayerPrefs.SetInt("MyData", 1);    // .. SetInt 함수 : key(MyData)와 연결된 int(1) 데이터 저장

        // .. 순차적으로 데이터 저장
        foreach(Achive achive in achives)
        {
            PlayerPrefs.SetInt(achive.ToString(), 0);   // .. 0 : 달성하지 못한것

        }
    }
    void Start()
    {
        UnlockCharacter();
    }

    // .. 캐릭터 해금 함수
    void UnlockCharacter()
    {
        for(int index = 0; index < lockCharacter.Length;index++)
        {
            string achiveName = achives[index].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;    // .. 업적을 달성했는 지, 1 : 업적달성(해금)
            lockCharacter[index].SetActive(!isUnlock);              // .. 잠금된 캐릭
            unlockCharacter[index].SetActive(isUnlock);             // .. 해금된 캐릭
        }
    }

    // .. 프레임마다 업적이 달성됐는 지 확인
    void LateUpdate()
    {
        foreach(Achive achive in achives)
        {
            CheckAchive(achive);
        }
    }
    
    // .. 업적 달성 확인 함수
    void CheckAchive(Achive achive)
    {
        bool isAchive = false;

        switch(achive)
        {
            // .. 포도농부 업적 조건 : 킬수 10이상
            case Achive.UnlockGrape:
                isAchive = GameManager.instance.kill >= 10;
                break;
            // .. 딸기농부 업적 조건 : 생존 성공
            case Achive.UnlockStrawberry:
                isAchive = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
                break;
        }

        // .. 업적달성완료 && 해금이 안된 캐릭터일 때 
        if(isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);   // .. 캐릭터 해금

            for(int index = 0;index < uiNotice.transform.childCount; index++)
            {
                bool isActive = index == (int)achive;
                uiNotice.transform.GetChild(index).gameObject.SetActive(isActive);
            }
            StartCoroutine(NoticeRoutine());
        }
    }

    // .. 업적 달성 알림
    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);   // .. UI 활성화

        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);

        yield return wait;      // .. 5초 후 

        uiNotice.SetActive(false);  // .. UI 비활성화
    }
}
