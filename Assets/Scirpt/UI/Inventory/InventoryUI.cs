using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private enum FilterOption
    {
        All,
        Equip,
        Potion
    }

    #region 변수
    [Header("Slot Option")]
    [Range(0, 10)][SerializeField] private int HorizontalSlotCount = 0; // 슬롯 가로 개수
    [Range(0, 10)][SerializeField] private int VerticalSlotCount = 0; // 슬롯 세로 개수
    [Range(32, 128)][SerializeField] private float SlotSize = 64f;
    [SerializeField] private float SlotMargin = 0f; // 한 슬롯의 상하좌우 여백
    [SerializeField] private float ContentAreaPadding = 20f; // 인벤토리 영역의 내부 여백]

    [Header("Object")]
    [SerializeField] private RectTransform ContentRoot; // 슬롯들이 위치할 곳
    [SerializeField] private GameObject SlotPrefab; // 슬롯 원본 객체
    [SerializeField] private ItemTooltipUI TooltipUI; // 아이템 툴팁 UI
    [SerializeField] private InventoryPopupUI InventoryPopup;
    [SerializeField] private Button TrimButton;
    [SerializeField] private Button SortButton;
    [SerializeField] private Toggle ToggleFilterAll;
    [SerializeField] private Toggle ToggleFilterEquip;
    [SerializeField] private Toggle ToggleFilterPotion;

    private Inventory oInventory; // 연결된 인벤토리

    private List<ItemSlotUI> ItemSlotUIList = new List<ItemSlotUI>();
    private GraphicRaycaster GrRayCaster; // 캔버스에서 레이캐스트 작업할때 사용하는 변수
    private PointerEventData EventData; // 포인트 관련 데이터를 얻기위한 변수
    private List<RaycastResult> RayCastResult; // 레이캐스트 결과값을 저장하는 리스트

    private ItemSlotUI PointerOverSlot; // 현재 포인터가 위차한 곳의 슬롯
    private ItemSlotUI BeginDragSlot; // 현재 드래그를 시작한 슬롯
    private Transform BeginDragIconTransform; // 해당 슬롯의 아이콘 Transform

    private Vector3 BeginDragIconPoint; // 드래그 시작 시 슬롯의 위치
    private Vector3 BeginDragCursorPoint; // 드래그 시작 시 커서의 위치
    private int BeginDragSlotSiblingIndex;

    private FilterOption CurrentFilterOption = FilterOption.All;
    #endregion // 변수

    #region 함수
    /** 초기화 */
    private void Awake()
    {
        SettingSlots();  
        TrimButton.onClick.AddListener(() => oInventory.TrimAll());
        SortButton.onClick.AddListener(() => oInventory.SortAll());

        SettingToggleEvent();
    }

    /** 초기화 => 상태를 갱신한다 */
    private void Update()
    {
        EventData.position = Input.mousePosition; // 포인트 위치를 마우스 위치로 설정

        OnPointerEnterAndExit();

        ShowOrHideItemTooltip();

        OnPointerDown();
        OnPointerDrag();
        OnPointerUp();
    }

    /** 레이케스트 결과값 리스트에서 첫번째 컴포넌트를 가져온다 */
    private T RaycastAndGetFirstComponent<T>() where T : Component
    {
        RayCastResult.Clear(); // 초기화
        GrRayCaster.Raycast(EventData, RayCastResult);

        // 레이케스트 결과값 리스트가 없을 경우
        if(RayCastResult.Count == 0)
        {
            return null;
        }

        return RayCastResult[0].gameObject.GetComponent<T>();
    }

    /** 인벤토리에서 클릭을 한다 */
    private void OnPointerDown()
    {
        // 왼쪽 클릭을 했을 경우
        if(Input.GetMouseButtonDown(0))
        {
            BeginDragSlot = RaycastAndGetFirstComponent<ItemSlotUI>(); // 클릭한 슬롯의 컴포넌트를 가져온다

            // 아이템을 갖고 있는 슬롯일 경우
            if(BeginDragSlot != null && BeginDragSlot.HasItem)
            {
                // 위치 기억
                BeginDragIconTransform = BeginDragSlot.oIconRect.transform; // 해당 슬롯의 아이콘 위치 저장
                BeginDragIconPoint = BeginDragIconTransform.position; // 슬롯의 위치 저장
                BeginDragCursorPoint = Input.mousePosition; //  마우스 커서 위치 저장

                // 맨 위에 보이게 설정
                BeginDragSlotSiblingIndex = BeginDragSlot.transform.GetSiblingIndex(); // 해당 오브젝트의 순위를 가져온다
                BeginDragSlot.transform.SetAsLastSibling(); // 가장 나중에 출력 > 맨 앞으로 나오게설정

                // 해당 슬롯의 하이라이트 이미지를 아이콘보다 뒤에 위치
                BeginDragSlot.SetHighlightOnTop(true);
            }
            else
            {
                BeginDragSlot = null;
            }
        }
    }

    /** 인벤토리에서 클릭을 뗄 경우 */
    private void OnPointerUp()
    {
        // 왼쪽을 클릭을 뗄 경우
        if(Input.GetMouseButtonUp(0))
        {
            // 슬롯이 있을 경우
            if (BeginDragSlot != null)
            {
                BeginDragIconTransform.position = BeginDragIconPoint; // 아이콘 위치를 원래대로 돌려놓는다
                BeginDragSlot.transform.SetSiblingIndex(BeginDragSlotSiblingIndex); // UI 순서를 원래대로 돌려놓는다

                // 드래그를 종료한다
                EndDrag();

                // 참조 제거
                BeginDragSlot = null;
                BeginDragIconTransform = null;
            }
        }
    }

    /** 인벤토리에서 드래그를 한다 */
    private void OnPointerDrag()
    {
        // 슬롯이 비워져 있을 경우
        if(BeginDragSlot == null)
        {
            return; // 함수를 종료한다
        }

        /** 왼쪽 클릭을 했을 경우 */
        if(Input.GetMouseButton(0))
        {
            // 선택한 아이콘 위치를 이동한다
            BeginDragIconTransform.position = BeginDragIconPoint
                + (Input.mousePosition - BeginDragCursorPoint);
        }
    }

    /** 슬롯을 세팅한다 */
    private void SettingSlots()
    {
        // 슬롯 프리팹 설정
        SlotPrefab.TryGetComponent(out RectTransform SlotRect);
        SlotRect.sizeDelta = new Vector2(SlotSize, SlotSize);

        SlotPrefab.TryGetComponent(out ItemSlotUI ItemUI);

        if(ItemUI == null)
        {
            SlotPrefab.AddComponent<ItemSlotUI>();
        }

        SlotPrefab.SetActive(false);

        Vector2 BeginPos = new Vector2(ContentAreaPadding, -ContentAreaPadding);
        Vector2 CurrentPos = BeginPos;

        ItemSlotUIList = new List<ItemSlotUI>(VerticalSlotCount * HorizontalSlotCount);

        // 슬롯들 동적 생성
        for(int i = 0; i < VerticalSlotCount; i++)
        {
            for(int j = 0; j < HorizontalSlotCount; j++)
            {
                int SlotIndex = (HorizontalSlotCount * i) + j;

                var SlotRoot = CreateCloneSlot();
                SlotRoot.pivot = new Vector2(0f, 1f); // Left Top
                SlotRoot.anchoredPosition = CurrentPos;
                SlotRoot.gameObject.SetActive(true);
                SlotRoot.gameObject.name = $"Item Slot [{SlotIndex}]";

                var SlotUI = SlotRoot.GetComponent<ItemSlotUI>();
                SlotUI.SetSlotIndex(SlotIndex);
                ItemSlotUIList.Add(SlotUI);

                // X축 증가
                CurrentPos.x += (SlotMargin + SlotSize);
            }

            // 다음 라인
            CurrentPos.x = BeginPos.x;
            CurrentPos.y -= (SlotMargin + SlotSize);
        }

        // 슬롯 프리팹 - 프리팹이 아닌 경우 파괴
        if(SlotPrefab.scene.rootCount != 0)
        {
            Destroy(SlotPrefab);
        }

        RectTransform CreateCloneSlot()
        {
            GameObject Slot = Instantiate(SlotPrefab);
            RectTransform SlotRect = Slot.GetComponent<RectTransform>();
            SlotRect.SetParent(ContentRoot);

            return SlotRect;
        }
    }

    /** 드래그를 종료한다 */
    private void EndDrag()
    {
        ItemSlotUI EndDragSlot = RaycastAndGetFirstComponent<ItemSlotUI>();

        // 드래그가 종료된 시점에 슬롯이 있고, 해당 슬롯이 접근 가능한 상태라면
        if(EndDragSlot != null && EndDragSlot.IsAccess)
        {
            // 수량 나누기 조건
            // 마우스 클릭 떼는 순간 좌측 컨트롤 또는 쉬프트 키 유지
            // Begin : 셀 수 있는 아이템 / End : 비어있는 슬롯
            // Begin 아이템의 수량 > 1

            bool IsSeparate = 
                (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftShift)) &&
                (oInventory.IsCountItem(BeginDragSlot.SlotIndex) && !oInventory.HasItem(EndDragSlot.SlotIndex));

            // true 수량 나누기, false 교환 또는 이동
            bool IsPossible = false;
            int CurrentAmount = 0;

            if(IsSeparate)
            {
                CurrentAmount = oInventory.GetCurrentAmount(BeginDragSlot.SlotIndex);
                if(CurrentAmount > 1)
                {
                    IsPossible = true;
                }
            }

            // 개수 나누기
            if(IsPossible)
            {
                TrySeparableAmount(BeginDragSlot.SlotIndex, EndDragSlot.SlotIndex, CurrentAmount);
            }
            // 교환 또는 이동
            else
            {
                // 원래 위치한 슬롯과 해당 슬롯에 있는 아이템을 교환한다
                TrySwapItem(BeginDragSlot, EndDragSlot);
            }

            // 툴팁 갱신
            UpdateTooltipUI(EndDragSlot);
            return;         
        }

        // 커서가 UI 레이케스트 타겟 위에 있지 않은 경우, 버리기
        if(!IsOverUI())
        {
            int SlotIndex = BeginDragSlot.SlotIndex;
            string ItemName = oInventory.GetItemName(SlotIndex);
            int Amount = oInventory.GetCurrentAmount(SlotIndex);

            // 셀 수 있는 아이템의 경우, 수량 표시
            if(Amount > 1)
            {
                ItemName += $" x{Amount}";
            }

            // 확인 팝업 띄우고 콜백
            InventoryPopup.ShowOKCancel(() => TryRemoveItem(SlotIndex), ItemName);
        }
    }
    
    /** 두 슬롯의 아이템 교환 */
    private void TrySwapItem(ItemSlotUI FirstSlot, ItemSlotUI EndSlot)
    {
        // 슬롯이 같을 경우
        if(FirstSlot == EndSlot)
        {
            return;
        }

        FirstSlot.SwapSlotItemIcon(EndSlot);
        oInventory.Swap(FirstSlot.SlotIndex, EndSlot.SlotIndex);
    }

    /** UI 인벤토리에서 아이템 제거 */
    private void TryRemoveItem(int SlotIndex)
    {
        oInventory.Remove(SlotIndex);
    }

    /** 셀 수 있는 아이템 개수 나누기 */
    private void TrySeparableAmount(int SlotIndexA, int SlotIndexB, int Amount)
    {
        if(SlotIndexA == SlotIndexB)
        {
            return;
        }

        string ItemName = oInventory.GetItemName(SlotIndexA);

        InventoryPopup.ShowAmountInputPopup(CurrentAmount =>
        oInventory.SeparateAmount(SlotIndexA, SlotIndexB, CurrentAmount), Amount, ItemName);
    }

    /** 커서가 UI 레이케스트 타겟 위에 있는지 확인한다 */
    private bool IsOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    /** 슬롯에 포인터가 올라가는 경우, 슬롯에서 포인터가 빠져나가는 경우 */
    private void OnPointerEnterAndExit()
    {
        // 이전 프레임의 슬롯
        var PrevSlot = PointerOverSlot;

        // 현재 프레임의 슬롯
        var CurrentSlot = PointerOverSlot = RaycastAndGetFirstComponent<ItemSlotUI>();

        // 이전 프레임의 슬롯이 존재하지 않을 경우
        if(PrevSlot == null)
        {
            // 현재 프레임의 슬롯이 존재 할 경우
            if(CurrentSlot != null)
            {
                // 현재 슬롯 하이라이트 표시
                OnCurrentEnter();
            }
        }
        else
        {
            // 현재 프레임의 슬롯이 존재하지 않을 경우
            if(CurrentSlot == null)
            {
                // 이전 프레임의 슬롯 하이라이트 해제
                OnPrevExit();
            }
            // 이전 프레임의 슬롯과 현재 프레임의 슬롯이 다를 경우
            else if(PrevSlot != CurrentSlot)
            {
                // 이전 프레임의 슬롯 하이라이트 해제
                OnPrevExit();
                // 현재 프레임의 슬롯 하이라이트 표시
                OnCurrentEnter();
            }
        }

        /** 지금 슬롯의 하이라이트 표시 */
        void OnCurrentEnter()
        {
            CurrentSlot.Highlight(true);
        }

        /** 전 슬롯의 하이라이트 해제 */
        void OnPrevExit()
        {
            PrevSlot.Highlight(false);
        }
    }

    /** 아이템 정보 툴팁 보여주거나 감추기 */
    private void ShowOrHideItemTooltip()
    {
        // 마우스가 유효한 아이템 아이콘 위에 올라와 있다면 툴팁 보여주기
        bool IsValid = PointerOverSlot != null && PointerOverSlot.HasItem && PointerOverSlot.IsAccess &&
            (PointerOverSlot != BeginDragSlot); // 드래그 시작한 슬롯이면 보여주지 않기

        // 마우스가 조건에 맞는 아이템에 올라와 있다면
        if(IsValid)
        {
            UpdateTooltipUI(PointerOverSlot);
            TooltipUI.Show();
        }
        else
        {
            TooltipUI.Hide();
        }
    }

    /** 툴팁 UI의 슬롯 데이터 갱신 */
    private void UpdateTooltipUI(ItemSlotUI Slot)
    {
        // 툴팁 정보 갱신
        TooltipUI.SetItemInfo(oInventory.GetItemData(Slot.SlotIndex));
    }

    /** 특정 슬롯의 필터 상태 업데이트 */
    public void UpdateFilterState(int SlotIndex, ItemData Data)
    {
        bool IsFilter = true;

        // null인 슬롯은 타입 검사 없이 필터 활성화
        if(Data != null)
        {
            switch(CurrentFilterOption)
            {
                case FilterOption.Equip:
                    //IsFilter = (Data is EquipItemData);
                    break;
                case FilterOption.Potion:
                    IsFilter = (Data is PotionItemData);
                    break;
            }
        }

        ItemSlotUIList[SlotIndex].SetItemAccessState(IsFilter);
    }

    /** 모든 슬롯 필터 상태 업데이트 */
    public void UpdateAllSlotFilter()
    {
        int Capacity = oInventory.oCapacity;

        for(int i = 0; i < Capacity; i++)
        {
            ItemData Data = oInventory.GetItemData(i);
            UpdateFilterState(i, Data);
        }
    }

    /** 토글 설정 */
    private void SettingToggleEvent()
    {
        ToggleFilterAll.onValueChanged.AddListener(Flag => UpdateFilter(Flag, FilterOption.All));
        ToggleFilterEquip.onValueChanged.AddListener(Flag => UpdateFilter(Flag, FilterOption.Equip));
        ToggleFilterPotion.onValueChanged.AddListener(Flag => UpdateFilter(Flag, FilterOption.Potion));

        void UpdateFilter(bool Flag, FilterOption Option)
        {
            if(Flag)
            {
                CurrentFilterOption = Option;
                UpdateAllSlotFilter();
            }
        }
    }
    #endregion // 함수
}