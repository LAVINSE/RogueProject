using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    [SerializeField] private int ItemId;
    [SerializeField] private string ItemName;
    [Multiline][SerializeField] private string ItemInfo;
    [SerializeField] private Sprite ItemIconSprite;
    [SerializeField] private GameObject DropItemPrefab;

    public int oItemId => ItemId;
    public string oItemName => ItemName;
    public string oItemInfo => ItemInfo;
    public Sprite oItemIconSprite => ItemIconSprite;

    // 타입에 맞는 새로운 아이템 생성
    public abstract Item CreateItem();
}
