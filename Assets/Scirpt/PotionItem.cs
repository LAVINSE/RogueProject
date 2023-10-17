using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionItem : CountItem, IUsableItem
{
    public PotionItem(PotionItemData Data, int Amount = 1) : base(Data, Amount)
    { 

    }

    public bool Use()
    {
        Amount--;
        return true;
    }

    protected override CountItem Clone(int Amount)
    {
        return new PotionItem(CountItemData as PotionItemData, Amount);
    }
}
