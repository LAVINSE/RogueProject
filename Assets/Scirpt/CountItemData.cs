using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CountItemData : ItemData
{
    [SerializeField] private int MaxAmount = 99;

    public int oMaxAmount => MaxAmount;
}
