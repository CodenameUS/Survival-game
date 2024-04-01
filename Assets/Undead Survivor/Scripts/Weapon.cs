using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;              // ... ���� id
    public int prefabId;        // ... ������ id
    public float damage;        // ... ���ⵥ����
    public int count;           // ... ����
    public float speed;         // ... �ӵ�

    float timer;                // ... ���� �ֱ⸦ ���� Ÿ�̸�
    Player player;              // ... �÷��̾� ��ũ��Ʈ

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
                // ... Bullet 0 ȸ�� ����
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

    // ... ������ �Լ�
    public void LevelUp(float damage, int count)
    {
        this.damage = damage * Character.Damage;
        this.count += count;

        if (id == 0)        // .. ��������
            Batch();

        // player�� ���� ��� gear�� ���� ApplyGear ����
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
        Hand hand = player.hands[(int)data.itemType];   // .. ������ ĳ����
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver);  
    }

    // ... Bullet 0 ���� ��ġ �Լ�
    void Batch()
    {
        for(int index=0; index < count; index++)
        {
            // ... �Ҹ��� ��ġ����
            Transform bullet;

            if (index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                // ... �θ� �ڽ�����
                bullet.parent = transform;
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;
            
            Vector3 rotVec = Vector3.forward * 360 * index / count;     // ... ���� ��ġ
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);    // ... ���� ��ġ                 
            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero);     // ... -100 ���� ���� ����(��������)
        }
    }

    // ... Bullet 1 
    void Fire()
    {
        // ... ����� ���ٸ�
        if (!player.scanner.nearsetTarget)
            return;

        // ... �Ѿ��� �������� �ϴ� ���� ����
        Vector3 targetPos = player.scanner.nearsetTarget.position;
        Vector3 dir = targetPos - transform.position;       // ... ũ�Ⱑ ���Ե� ����
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        // ... ��ġ�� ȸ�� ����
        bullet.position = transform.position;
        // ... ������ ���� �߽����� ��ǥ�� ���� ȸ��
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        // ... count�� �����
        bullet.GetComponent<Bullet>().Init(damage, count , dir);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);

    }
}
