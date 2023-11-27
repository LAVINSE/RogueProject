using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemInfoTable : ScriptableObject
{
    public enum eItemType
    {
        None = 0,
        Consumable,
        Equip
    }

    [Header("=====> 아이템 공통 <=====")]
    public string ItemName;
    public int ItemPrice;
    public eItemType ItemType = eItemType.None;
    public Sprite ItemImage;

    [Header("=====> 아이템 소모품 <=====")]
    [Space]
    public int PortionValue;

    [Header("=====> 아이템 객체 <=====")]
    public GameObject ItemPrefab;

    public void UseItem()
    {
        switch (ItemType)
        {
            case eItemType.None:
                break;
            case eItemType.Consumable:
                CSceneManager.Instance.PlayerObj.GetComponent<Player>().oPlayerCurrentHp += PortionValue;
                break;
            case eItemType.Equip:
                break;
        }
    }
}
