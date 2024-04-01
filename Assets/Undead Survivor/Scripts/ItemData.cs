using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ... Ŀ���� �޴��� �����ϴ� �Ӽ�
[CreateAssetMenu(fileName ="Item",menuName ="Scriptable Object/ItemData")]
// ... ������(����) ���� ��ũ��Ʈ
public class ItemData : ScriptableObject
{
    // .. ���� ���Ÿ� �۷��� �Ź� ��
    public enum ItemType { Melee, Range, Glove, Shoe, Heal}

    [Header("# Main Info")]
    public ItemType itemType;
    public int itemId;
    public string itemName;
    [TextArea]
    public string itemDesc;
    public Sprite itemIcon;

    [Header("# Level Data")]
    public float baseDamage;
    public int baseCount;
    public float[] damages;
    public int[] counts;

    [Header("# Weapon")]
    public GameObject projectile;
    public Sprite hand;

}
