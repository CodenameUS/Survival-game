using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AchiveManager : MonoBehaviour
{

    public GameObject[] lockCharacter;      // .. ��ݵ� ĳ�� ���
    public GameObject[] unlockCharacter;    // .. �رݵ� ĳ�� ���
    public GameObject uiNotice;             // .. �ر� �˸� UI

    // .. ���� ����
    enum Achive { UnlockGrape,  UnlockStrawberry }
    Achive[] achives;
    WaitForSecondsRealtime wait;

    void Awake()
    {
        achives = (Achive[])Enum.GetValues(typeof(Achive));
        wait = new WaitForSecondsRealtime(5);

        // .. ������ �����Ͱ� �ִ��� Ȯ��
        if(!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
    }


    // .. Device ���� ������ �ʱ�ȭ �Լ�
    void Init()
    {
        // .. PlayerPrefs : ������ ���� ��� �����ϴ� Ŭ����
        PlayerPrefs.SetInt("MyData", 1);    // .. SetInt �Լ� : key(MyData)�� ����� int(1) ������ ����

        // .. ���������� ������ ����
        foreach(Achive achive in achives)
        {
            PlayerPrefs.SetInt(achive.ToString(), 0);   // .. 0 : �޼����� ���Ѱ�

        }
    }
    void Start()
    {
        UnlockCharacter();
    }

    // .. ĳ���� �ر� �Լ�
    void UnlockCharacter()
    {
        for(int index = 0; index < lockCharacter.Length;index++)
        {
            string achiveName = achives[index].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;    // .. ������ �޼��ߴ� ��, 1 : �����޼�(�ر�)
            lockCharacter[index].SetActive(!isUnlock);              // .. ��ݵ� ĳ��
            unlockCharacter[index].SetActive(isUnlock);             // .. �رݵ� ĳ��
        }
    }

    // .. �����Ӹ��� ������ �޼��ƴ� �� Ȯ��
    void LateUpdate()
    {
        foreach(Achive achive in achives)
        {
            CheckAchive(achive);
        }
    }
    
    // .. ���� �޼� Ȯ�� �Լ�
    void CheckAchive(Achive achive)
    {
        bool isAchive = false;

        switch(achive)
        {
            // .. ������� ���� ���� : ų�� 10�̻�
            case Achive.UnlockGrape:
                isAchive = GameManager.instance.kill >= 10;
                break;
            // .. ������ ���� ���� : ���� ����
            case Achive.UnlockStrawberry:
                isAchive = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
                break;
        }

        // .. �����޼��Ϸ� && �ر��� �ȵ� ĳ������ �� 
        if(isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);   // .. ĳ���� �ر�

            for(int index = 0;index < uiNotice.transform.childCount; index++)
            {
                bool isActive = index == (int)achive;
                uiNotice.transform.GetChild(index).gameObject.SetActive(isActive);
            }
            StartCoroutine(NoticeRoutine());
        }
    }

    // .. ���� �޼� �˸�
    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);   // .. UI Ȱ��ȭ

        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);

        yield return wait;      // .. 5�� �� 

        uiNotice.SetActive(false);  // .. UI ��Ȱ��ȭ
    }
}
