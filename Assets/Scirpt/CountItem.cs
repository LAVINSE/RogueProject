using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CountItem : Item
{
    public CountItemData CountItemData { get; private set; }
    public int Amount { get; protected set; } // 현재 아이템 개수
    public int MaxAmount => CountItemData.oMaxAmount; // 아이템 최대 개수
    public bool IsMax => Amount >= CountItemData.oMaxAmount; // 수량이 가득 찾는지 확인하는 변수
    public bool IsEmpty => Amount <= 0; // 개수가 없는지 확인하는 변수

    public CountItem(CountItemData Data, int Amount = 1) : base(Data)
    {
        CountItemData = Data;
        SetAmount(Amount);
    }

    /** 개수 지정, 범위 제한 (1 ~ 99) */
    public void SetAmount(int Amount)
    {
        Amount = Mathf.Clamp(Amount, 0, MaxAmount);
    }

    /** 개수 추가 및 최대치 초과량 반환, 초과량 없을 경우 0  */
    public int AddAmountAndGetExcess(int Amount)
    {
        int NextAmount = Amount + this.Amount;
        SetAmount(NextAmount);

        return (NextAmount > MaxAmount) ? (NextAmount - MaxAmount) : 0;
    }

    /** 개수를 나누어 복제 */
    public CountItem SeperateAndClone(int Amount)
    {
        // 수량이 한개 이하일 경우, 복제 불가
        if(Amount <= 1)
        {
            return null;
        }

        if(Amount > this.Amount - 1)
        {
            this.Amount = Amount - 1;
        }

        Amount -= this.Amount;
        return Clone(Amount);
    }

    protected abstract CountItem Clone(int Amount);
}
