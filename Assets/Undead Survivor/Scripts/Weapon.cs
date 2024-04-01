using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;              // ... 무기 id
    public int prefabId;        // ... 프리팹 id
    public float damage;        // ... 무기데미지
    public int count;           // ... 개수
    public float speed;         // ... 속도

    float timer;                // ... 공격 주기를 위한 타이머
    Player player;              // ... 플레이어 스크립트

    void Awake()
    {
        player = GameManager.instance.player;
    }

   
    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        switch (id)
        {
            case 0:
                // ... Bullet 0 회전 로직
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
                timer += Time.deltaTime;

                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }
    }

    // ... 레벨업 함수
    public void LevelUp(float damage, int count)
    {
        this.damage = damage * Character.Damage;
        this.count += count;

        if (id == 0)        // .. 근접무기
            Batch();

        // player가 가진 모든 gear에 대해 ApplyGear 실행
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {
        // .. basic set
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        // .. property set
        id = data.itemId;
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;

        for(int index=0;index < GameManager.instance.pool.prefabs.Length; index++)
        {
            if(data.projectile == GameManager.instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }
        switch (id)
        {
            case 0:
                speed = 150 * Character.WeaponSpeed;
                Batch();
                break;
            default:
                speed = 0.3f * Character.WeaponRate;
                break;
        }

        // .. Hand Set
        Hand hand = player.hands[(int)data.itemType];   // .. 정수형 캐스팅
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver);  
    }

    // ... Bullet 0 무기 배치 함수
    void Batch()
    {
        for(int index=0; index < count; index++)
        {
            // ... 불릿의 위치정보
            Transform bullet;

            if (index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                // ... 부모를 자식으로
                bullet.parent = transform;
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;
            
            Vector3 rotVec = Vector3.forward * 360 * index / count;     // ... 무기 위치
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);    // ... 무기 배치                 
            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero);     // ... -100 값은 무한 관통(근접무기)
        }
    }

    // ... Bullet 1 
    void Fire()
    {
        // ... 대상이 없다면
        if (!player.scanner.nearsetTarget)
            return;

        // ... 총알이 나가고자 하는 방향 설정
        Vector3 targetPos = player.scanner.nearsetTarget.position;
        Vector3 dir = targetPos - transform.position;       // ... 크기가 포함된 방향
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        // ... 위치와 회전 결정
        bullet.position = transform.position;
        // ... 지정된 축을 중심으로 목표를 향해 회전
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        // ... count는 관통력
        bullet.GetComponent<Bullet>().Init(damage, count , dir);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);

    }
}
