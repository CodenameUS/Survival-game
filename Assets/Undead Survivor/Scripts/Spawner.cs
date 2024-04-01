using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // ... ������ġ�� ���� ����
    public Transform[] spawnPoint;
    // ... ���������͸� ���� ����
    public SpawnData[] spawnData;

    // ... ��ȯ �ֱ⸦ ���� Ÿ�̸�
    float timer;

    // ... ��ȯ ���̺� ���� ����
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

        // ... ������ ���� ���� ����
        if (timer > spawnData[waveLevel].spawnTime)
        {
            timer = 0f;
            Spawn();
        }
    }

    // ... ���� ���� �Լ�
    void Spawn()
    {
        // ... ������ ���� ���� ��ȯ
        GameObject enemy = GameManager.instance.pool.Get(0);
        // ... ���� ��ġ�� ������ ��ġ�� �������� ����
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnData[waveLevel]);
    }
}

// ... ��ȯ ������ ��� Ŭ����
[System.Serializable]   // ... ����ȭ  => �ν�����â���� ����
public class SpawnData
{
    public int spriteType;
    public int health;
    public float spawnTime;
    public float speed;
}