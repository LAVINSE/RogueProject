using System;
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

    private HashSet<int> IndexSetForUpdate = new HashSet<int>(); // 업데이트 할 인덱스 목록
    private readonly static Dictionary<Type, int> SortWeightDict = new Dictionary<Type, int>
    {
        {typeof(PotionItemData), 10000 }
    };

    private class ItemCompare : IComparer<Item>
    {
        public int Compare(Item A, Item B)
        {
            return (A.Data.oItemId + SortWeightDict[A.Data.GetType()]) -
                (B.Data.oItemId + SortWeightDict[B.Data.GetType()]);
        }
    }

    private static readonly ItemCompare oItemCompare = new ItemCompare();
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
    private int FindEmptySlotIndex(int StartSlotIndex = 0)
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
        //oInventoryUI.SetAccessSlotRange(oCapacity);
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

    /** 셀 수 있는 아이템의 수량 나누기 (A > B 슬롯으로) */
    public void SeparateAmount(int SlotIndexA, int SlotIndexB, int AmountSeparate)
    {
        // AmountSeparate : 나눌 목표 수량

        if(!IsValidIndex(SlotIndexA))
        {
            return;
        }
        if (!IsValidIndex(SlotIndexB))
        {
            return;
        }

        Item ItemA = Items[SlotIndexA];
        Item ItemB = Items[SlotIndexB];

        CountItem CountItemA = ItemA as CountItem;

        // 조건 : A 슬롯 - 셀 수 있는 아이템 / B 슬롯 - null
        // 조건에 맞는 경우, 복제하여 슬롯 B에 추가

        if(CountItemA != null && ItemB == null)
        {
            Items[SlotIndexB] = CountItemA.SeperateAndClone(AmountSeparate);

            UpdateSlot(SlotIndexA, SlotIndexB);
        }
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
            //oInventoryUI.SettingItemIcon(SlotIndex, oItem.Data.oItemIconSprite);


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
                    //oInventoryUI.SetItemAmoutText(SlotIndex, Count.Amount);
                }
            }
            else
            {
                // 셀 수 없는 아이템인 경우 텍스트 제거
                //oInventoryUI.HideItemAmoutText(SlotIndex);
            }
        }
        else
        {
            // 빈 슬롯인 경우
            RemoveIcon();
        }

        void RemoveIcon()
        {
            //oInventoryUI.RemoveItem(SlotIndex);
            //oInventoryUI.HideItemAmoutText(SlotIndex); // 수량 텍스트 숨기기
        }
    }

    /** 해당하는 인덱스의 슬롯 상태 및 UI 갱신 */
    private void UpdateSlot(params int[] SlotIndex)
    {
        foreach(var i in SlotIndex)
        {
            UpdateSlot(i);
        }
    }

    /** 해당하는 인덱스의 슬롯 상태 및 UI 갱신 */
    private void UpdateAllSlot()
    {
        for(int i = 0; i < oCapacity; i++)
        {
            UpdateSlot(i);
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
        //UpdateSlot(SlotIndexA, SlotIndexB);
    }

    /** 인벤토리에 아이템을 추가한다 */
    public int Add(ItemData oItemData, int Amount = 1)
    {
        int SlotIndex;

        // 수량이 있는 아이템일 경우
        if(oItemData is CountItemData CountData)
        {
            bool FindNextCount = true;
            SlotIndex = -1;

            // 아이템 개수 만큼 반복
            while(Amount > 0)
            {
                // 이미 해당 아이템이 인벤토리 내에 존재하고, 개수 여유 있는지 검사
                if(FindNextCount)
                {
                    // 아이템이 슬롯에 존재하는지 확인
                    //SlotIndex = FindCountSlotItemIndex(CountData, SlotIndex + 1);

                    // 개수 여유있는 기존 슬롯이 더이상 없다고 판단될 경우, 빈 슬롯부터 탐색
                    if(SlotIndex == -1)
                    {
                        FindNextCount= false;
                    }
                    else
                    {
                        // 기존 슬롯을 찾은 경우, 양 증가시키고 초과량 존재 시 Amount 초기화
                        CountItem Count = Items[SlotIndex] as CountItem;
                        Amount = Count.AddAmountAndGetExcess(Amount);

                        UpdateSlot(SlotIndex);
                    }
                }
                else
                {
                    // 빈 슬롯 탐색
                    SlotIndex = FindEmptySlotIndex(SlotIndex + 1);


                    // 빈 슬롯조차 없는 경우 종료
                    if(SlotIndex == -1)
                    {
                        break;
                    }
                    else
                    {
                        // 빈 슬롯 발견 시, 슬롯에 아이템 추가 및 남는량 계산
                        CountItem Count = CountData.CreateItem() as CountItem;
                        Count.SetAmount(Amount);

                        // 슬롯에 추가
                        Items[SlotIndex] = Count;

                        // 남은 개수 계산
                        Amount = (Amount > CountData.oMaxAmount) ? (Amount - CountData.oMaxAmount) : 0;

                        UpdateSlot(SlotIndex);
                    }
                }
            }
        }
        else
        {
            // 수량이 없는 아이템
            // 1개만 넣는 경우
            if(Amount == 1)
            {
                SlotIndex = FindEmptySlotIndex();
                if(SlotIndex != -1)
                {
                    // 아이템을 생성하여 슬롯에 추가
                    Items[SlotIndex] = oItemData.CreateItem();
                    Amount = 0;

                    UpdateSlot(SlotIndex);
                }
            }


            // 2개 이상의 수량 없는 아이템을 동시에 추가하는 경우
            SlotIndex = -1;

            for (; Amount > 0; Amount--)
            {
                // 아이템 넣은 인덱스의 다음 인덱스부터 슬롯 탐색
                SlotIndex = FindEmptySlotIndex(SlotIndex + 1);

                // 다 넣지 못한 경우 루프 종료
                if(SlotIndex == -1)
                {
                    break;
                }

                // 아이템을 생성하여 슬롯에 추가
                Items[SlotIndex] = oItemData.CreateItem();

                UpdateSlot(SlotIndex);
            }
        }

        // 반환이 0 이면 추가하는데 성공
        return Amount;
    }

    /** 인벤토리에서 아이템을 제거한다 */
    public void Remove(int SlotIndex)
    {
        if(!IsValidIndex(SlotIndex))
        {
            return;
        }

        Items[SlotIndex] = null;
        UpdateSlot(SlotIndex);
    }

    /** 해당 슬롯의 아이템 사용 */
    public void Use(int SlotIndex)
    {
        // 해당 슬롯에 아이템이 없다면
        if (Items[SlotIndex] == null)
        {
            return;
        }

        // 사용 가능한 아이템인 경우
        if (Items[SlotIndex] is IUsableItem UseItem)
        {
            // 아이템 사용
            bool Use = UseItem.Use();

            // 아이템 사용을 성공했을 경우
            if(Use)
            {
                UpdateSlot(SlotIndex);
            }
        }
    }

    /** */
    public void TrimAll()
    {
        /*  i 커서와 j 커서가 존재한다.
            i 커서 : 가장 앞에 있는 빈칸을 찾는 커서
            j 커서 : i 커서 위치에서부터 뒤로 이동하며 빈칸이 아닌 곳을 찾는 커서

            i커서가 빈칸을 찾으면 j 커서는 i+1 위치부터 뒤로 이동하며 빈칸이 아닌 곳을 찾는다.
            j커서가 아이템을 찾으면 아이템을 i커서 위치로 옮기고, i 커서는 i+1 위치로 이동한다.
            j커서가 배열 범위를 벗어나면 종료한다
         */

        IndexSetForUpdate.Clear();

        int i = -1;
        while (Items[i++] != null) ;
        int j = i;

        while(true)
        {
            while (j++ < oCapacity && Items[j] == null) ;
            
            if(j == oCapacity)
            {
                break;
            }

            IndexSetForUpdate.Add(i);
            IndexSetForUpdate.Add(j);

            Items[i] = Items[j];
            Items[j] = null;
            i++;
        }

        foreach(var Index in IndexSetForUpdate)
        {
            UpdateSlot(Index);
        }
    }

    public void SortAll()
    {
        // Trim
        int i = -1;
        while (Items[i++] != null) ;
        int j = i;

        while (true)
        {
            while (j++ < oCapacity && Items[j] == null) ;

            if(j == oCapacity)
            {
                break;
            }

            Items[i] = Items[j];
            Items[j] = null;
            i++;
        }

        // Sort
        Array.Sort(Items, 0, i, oItemCompare);

        // Update
        UpdateAllSlot();
    }
    #endregion // 함수
}
