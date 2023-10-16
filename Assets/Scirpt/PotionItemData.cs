using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Item_Potion", menuName = "Inventory System/Item Data/Potion")]
public class PotionItemData : CountItemData
{
    [SerializeField] private float PotionValue; // 회복량

    public float oPotionValue => PotionValue;

    public override Item CreateItem()
    {
        return new PotionItem(this);
    }
}
