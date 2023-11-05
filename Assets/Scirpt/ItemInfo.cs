using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemInfo : ScriptableObject
{
    public enum eItemType
    {
        None = 0,
        Consumable,
        Equip
    }

    [Header("=====> 아이템 정보 <=====")]
    public string ItemName;
    public eItemType ItemType = eItemType.None;
    public Sprite ItemImage;

    [Header("=====> 아이템 객체 <=====")]
    public GameObject ItemPrefab;
}
