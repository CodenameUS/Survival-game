using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // ... 스폰위치를 담을 변수
    public Transform[] spawnPoint;
    // ... 스폰데이터를 담을 변수
    public SpawnData[] spawnData;

    // ... 소환 주기를 위한 타이머
    float timer;

    // ... 소환 웨이브 레벨 변수
    int waveLevel;

    public float levelTime;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        levelTime = GameManager.instance.maxGameTime / spawnData.Length;
    }
    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        timer += Time.deltaTime;
        
        waveLevel = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length - 1);  // ... Float to Integer

        // ... 레벨에 따른 몬스터 스폰
        if (timer > spawnData[waveLevel].spawnTime)
        {
            timer = 0f;
            Spawn();
        }
    }

    // ... 몬스터 스폰 함수
    void Spawn()
    {
        // ... 레벨에 따라 몬스터 소환
        GameObject enemy = GameManager.instance.pool.Get(0);
        // ... 몬스터 위치를 지정한 위치중 랜덤으로 정함
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnData[waveLevel]);
    }
}

// ... 소환 데이터 담당 클래스
[System.Serializable]   // ... 직렬화  => 인스펙터창에서 보임
public class SpawnData
{
    public int spriteType;
    public int health;
    public float spawnTime;
    public float speed;
}