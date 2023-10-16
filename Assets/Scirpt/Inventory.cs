using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region 변수
    [Range(8, 64)][SerializeField] private int InitalCapacity = 32; // 초기 수용 한도
    [Range(8, 64)][SerializeField] private int MaxCapacity = 64; // 최대 수용 한도
    [SerializeField] private InventoryUI oInventoryUI; // 연결된 인벤토리 UI
    [SerializeField] private Item[] Items; // 아이템 목록
    #endregion // 변수

    #region 프로퍼티
    public int oCapacity { get; private set; } // 아이템 수용 한도
    #endregion // 프로퍼티

    #region 함수
    /** 초기화 */
    private void Awake()
    {
        Items = new Item[MaxCapacity]; // 아이템 최대 수용 공간 설정
        oCapacity = InitalCapacity; // 초기 수용 한도 설정
    }

    /** 초기화 */
    private void Start()
    {
        UpdateAccessStateAll();
    }

    /** 인덱스가 수용 범위 내에 있는지 검사 */
    private bool IsValidIndex(int SlotIndex)
    {
        return SlotIndex >= 0 && SlotIndex < oCapacity;
    }

    /** 앞에서부터 비어있는 슬롯 인덱스 탐색 */
    private int FindEmptySlotIndex(int StartSlotIndex)
    {
        for(int i = StartSlotIndex; i < oCapacity; i++)
        {
            // 공간이 비어있을 경우
            if (Items[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    /** 모든 슬롯 UI에 접근 가능 여부 업데이트 */
    public void UpdateAccessStateAll()
    {
        oInventoryUI.SetAccessSlotRange(oCapacity);
    }

    /** 해당 슬롯이 아이템을 가지고 있는지 여부 */
    public bool HasItem(int SlotIndex)
    {
        // 수용 범위 내에 있는지 확인하고, 슬롯이 비어있는지 확인한다
        return IsValidIndex(SlotIndex) && Items[SlotIndex] == null;
    }

    /** 해당 슬롯이 셀 수 있는 아이템인지 여부 */
    public bool IsCountItem(int SlotIndex)
    {
        // 아이템이 가지고 있는지 확인하고 , 해당 슬롯이 셀수 있는 아이템이면 true
        return HasItem(SlotIndex) && Items[SlotIndex] is CountItem;
    }

    /** 해당 슬롯의 현재 아이템 개수 리턴 */
    public int GetCurrentAmount(int SlotIndex)
    {
        // 해당 슬롯이 범위내에 없을 경우 
        if(!IsValidIndex(SlotIndex))
        {
            return -1;
        }

        // 해당 슬롯에 아이템이 없을 경우
        if (Items[SlotIndex] == null)
        {
            return 0;
        }

        CountItem Count = Items[SlotIndex] as CountItem;

        // 셀 수 없는 아이템 일 경우
        if(Count == null)
        {
            return 1;
        }

        return Count.Amount;
    }

    /** 해당 슬롯의 아이템 정보 반환 */
    public ItemData GetItemData(int SlotIndex)
    {
        // 해당 슬롯이 범위내에 없을 경우
        if(!IsValidIndex(SlotIndex))
        {
            return null;
        }

        // 해당 슬롯에 아이템이 없을 경우
        if (Items[SlotIndex] == null)
        {
            return null;
        }

        return Items[SlotIndex].Data;
    }

    /** 해당 슬롯의 아이템 이름 반환 */
    public string GetItemName(int SlotIndex)
    {
        // 해당 슬롯이 범위내에 없을 경우
        if (!IsValidIndex(SlotIndex))
        {
            return string.Empty;
        }

        // 해당 슬롯에 아이템이 없을 경우
        if (Items[SlotIndex] == null)
        {
            return string.Empty;
        }

        return Items[SlotIndex].Data.oItemName;
    }

    /** 해당하는 인덱스의 슬롯 상태 및 UI 갱신 */
    public void UpdateSlot(int SlotIndex)
    {
        // 해당 슬롯이 범위내에 없을 경우
        if (!IsValidIndex(SlotIndex))
        {
            return;
        }

        Item oItem = Items[SlotIndex];

        // 아이템이 슬롯에 존재하는 경우
        if(oItem != null)
        {
            // 아이콘 등록
            oInventoryUI.SettingItemIcon(SlotIndex, oItem.Data.oItemIconSprite);


            // 셀 수 있는 아이템인 경우
            if(oItem is CountItem Count)
            {
                // 수량이 0일 경우, 아이템 제거
                if(Count.IsEmpty)
                {
                    Items[SlotIndex] = null;
                    RemoveIcon();
                    return;
                }
                else
                {
                    // 수량 텍스트 표시
                    oInventoryUI.SetItemAmoutText(SlotIndex, Count.Amount);
                }
            }
            else
            {
                // 셀 수 없는 아이템인 경우 텍스트 제거
                oInventoryUI.HideItemAmoutText(SlotIndex);
            }
        }
        else
        {
            // 빈 슬롯인 경우
            RemoveIcon();
        }

        void RemoveIcon()
        {
            oInventoryUI.RemoveItem(SlotIndex);
            oInventoryUI.HideItemAmoutText(SlotIndex); // 수량 텍스트 숨기기
        }
    }

    /** 아이템을 교환한다 */
    public void Swap(int SlotIndexA, int SlotIndexB)
    {
        if(!IsValidIndex(SlotIndexA) || !IsValidIndex(SlotIndexB))
        {
            return;
        }

        Item ItemA = Items[SlotIndexA];
        Item ItemB = Items[SlotIndexB];

        // 셀 수 있는 아이템이고, 동일한 아이템 일 경우
        if (ItemA != null && ItemB != null && ItemA.Data == ItemB.Data &&
            ItemA is CountItem CountA && ItemB is CountItem CountB) 
        {
            int MaxAmount = CountB.MaxAmount;
            int Sum = CountA.Amount + CountB.Amount;

            // 개수를 합쳤을때 최대 개수를 넘지 않을 경우
            if(Sum <= MaxAmount)
            {
                // 움직인 아이템은 0개로, 합쳐진 아이템은 합 개수로
                CountA.SetAmount(0);
                CountB.SetAmount(Sum);
            }
            else
            {
                // 최대 개수를 넘을 경우, 합쳐진 아이템을 최대 개수로, 움직인 아이템은 남은 개수로
                CountA.SetAmount(Sum - MaxAmount);
                CountB.SetAmount(MaxAmount);
            }
        }
        else
        {
            // 슬롯 교체, 일반적인 경우
            Items[SlotIndexA] = ItemB;
            Items[SlotIndexB] = ItemA;
        }
            
        // 두 슬롯 정보 갱신
        UpdateSlot(SlotIndexA, SlotIndexB);
    }
    #endregion // 함수
}
