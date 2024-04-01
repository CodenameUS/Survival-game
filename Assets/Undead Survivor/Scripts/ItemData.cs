using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ... 커스텀 메뉴를 생성하는 속성
[CreateAssetMenu(fileName ="Item",menuName ="Scriptable Object/ItemData")]
// ... 아이템(무기) 관련 스크립트
public class ItemData : ScriptableObject
{
    // .. 근접 원거리 글러브 신발 힐
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
