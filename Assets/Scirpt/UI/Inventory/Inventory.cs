using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class Inventory : MonoBehaviour
{
    #region 변수
    [Header("=====> 아이템 리스트 <=====")]
    [SerializeField] private List<ItemInfoTable> ItemList = new List<ItemInfoTable>();

    [Header("=====> 인벤토리 슬롯 설정 <=====")]
    [Range(0, 10)][SerializeField] private int VerticalSlotCount = 0; // 세로 길이 설정
    [SerializeField] private int HorizontalSlotCount = 5;
    [SerializeField] private float ContentHeight = 150f;

    [Header("=====> 슬롯 설정 <=====")]
    [SerializeField] private GameObject SlotPrefab;
    [SerializeField] private RectTransform SlotRectTransform;

    [Header("=====> 인벤토리 UI 설정 <=====")]
    [SerializeField] private GameObject PopupPanel;
    [SerializeField] private Button InventoryCancelButton;

    [Header("=====> 인벤토리 PopupUse 설정 <=====")]
    [SerializeField] private GameObject UsePanel;
    [SerializeField] private TMP_Text UseItemNameText;
    [SerializeField] private TMP_Text UseInfoText;
    [SerializeField] private Button UseOkButton;
    [SerializeField] private Button UseCancelButton;

    [Header("=====> 인벤토리 PopupAbandon 설정 <=====")]
    [SerializeField] private GameObject AbandonPanel;
    [SerializeField] private TMP_Text AbandonItemNameText;
    [SerializeField] private TMP_Text AbandonInfoText;
    [SerializeField] private Button AbandonOkButton;
    [SerializeField] private Button AbandonCancelButton;

    [Header("=====> 인스펙터 확인용 <=====")]
    [SerializeField] private ItemSlot[] Slots;
    [SerializeField] private GameObject Content;
    [SerializeField] private ScrollRect Scroll;

    private GridLayoutGroup InventoryContentGridLayoutGroup;
    private PointerEventData PointerEvent;
    private ItemSlot BeginDragSlot; // 현재 드래그를 시작한 슬롯
    private Transform BeginDragIconTransform; // 해당 슬롯의 아이콘 트랜스폼
    private Vector3 BeginDragIconPoint; // 드래그 시작 시 아이콘의 위치
    private Vector3 BeginDragCursorPoint; // 드래그 시작 시 커서의 위치
    private List<RaycastResult> RayResult;
    private GraphicRaycaster GraphicRay;
    private int DragSlotSiblingIndex;
    

    // 내부에서 사용
    private event Action PopupUseOkCallback;
    private event Action PopupAbandonOkCallback;
    #endregion // 변수

    #region 프로퍼티
    public static Inventory Instance { get; private set; }
    public List<ItemInfoTable> oItemList
    {
        get => ItemList;
        set => ItemList = value;
    }

    public bool IsShowInven = false;
    #endregion // 프로퍼티

    #region 함수
    /** 초기화 */
    private void Awake()
    {
        Instance = this;

        TryGetComponent(out GraphicRay);
        if (GraphicRay == null)
        {
            GraphicRay = gameObject.AddComponent<GraphicRaycaster>();
        }
        PointerEvent = new PointerEventData(EventSystem.current);
        RayResult = new List<RaycastResult>(10);

        SettingButton();

        // 가로 길이 설정
        InventoryContentGridLayoutGroup = GetComponentInChildren<GridLayoutGroup>();
        HorizontalSlotCount = InventoryContentGridLayoutGroup.constraintCount;
        
        // 슬롯 설정
        SettingSlot();
        Slots = SlotRectTransform.transform.GetComponentsInChildren<ItemSlot>();

        // 저장된 데이터 가져오기
        oItemList = GameManager.Inst.oPlayerItemList;

        // 슬롯 업데이트
        UpdateSlot();
    }

    /** 초기화 => 상태를 갱신한다 */
    private void Update()
    {
        PointerEvent.position = Input.mousePosition;

        OnPointerDown();
        OnPointerDrag();
        OnPointerUp();
    }

    /** 버튼을 설정한다 */
    private void SettingButton()
    {
        InventoryCancelButton.onClick.AddListener(() =>
        {
            IsShowInven = false;
            this.gameObject.SetActive(false);
        });

        UseOkButton.onClick.AddListener(() =>
        {
            PopupUseOkCallback?.Invoke();
            PopupPanel.SetActive(false);
            UsePanel.SetActive(false);
        });
        UseCancelButton.onClick.AddListener(() =>
        {
            PopupPanel.SetActive(false);
            UsePanel.SetActive(false);
        });

        AbandonOkButton.onClick.AddListener(() =>
        {
            PopupAbandonOkCallback?.Invoke();
            PopupPanel.SetActive(false);
            AbandonPanel.SetActive(false);
        });
        AbandonCancelButton.onClick.AddListener(() =>
        {
            PopupPanel.SetActive(false);
            AbandonPanel.SetActive(false);
        });
    }

    /** 슬롯을 설정한다 */
    private void SettingSlot()
    {
        int SlotCount = 0;

        for (int i = 0; i < VerticalSlotCount; i++)
        {
            for (int j = 0; j < HorizontalSlotCount; j++)
            {
                SlotCount = (HorizontalSlotCount * i) + j;


                var Slot = CreateSlot();
                Slot.gameObject.SetActive(true);
                Slot.gameObject.name = $"ItemSlot {SlotCount}";
            }
        }

        ContentHeight += ((SlotCount + 1) / HorizontalSlotCount) * 100;
        Scroll.content.sizeDelta = new Vector2(0, ContentHeight);
    }

    /** 슬롯을 업데이트 한다 */
    public void UpdateSlot()
    {
        int i = 0;

        // 아이템 리스트에 들어있는 아이템 수 만큼 슬롯에 넣는다
        for (; i < ItemList.Count && i < Slots.Length; i++)
        {
            Slots[i].oItemInfo = ItemList[i];
        }

        // 슬롯이 남아 있으면 null 처리
        for(; i < Slots.Length;i++)
        {
            Slots[i].oItemInfo = null;
        }

        // 데이터 저장
        GameManager.Inst.oPlayerItemList = oItemList;
    }

    /** 슬롯을 생성한다 */
    private RectTransform CreateSlot()
    {
        GameObject Slot = Instantiate(SlotPrefab);
        RectTransform SlotRect = Slot.GetComponent<RectTransform>();
        SlotRect.SetParent(SlotRectTransform);

        return SlotRect;
    }

    /** 아이템을 추가한다 */
    public bool AddItem(ItemInfoTable Item)
    {
        bool IsAddItem = false;

        if (ItemList.Count < Slots.Length)
        {
            ItemList.Add(Item);
            IsAddItem = true;
            UpdateSlot();    
        }
        else
        {
            IsAddItem = false;
            // TODO : 가득 찼다는걸 알리는 함수 추가
            // 슬롯이 가득 참
        }

        return IsAddItem;
    }

    /** 레이케스트 첫번째 컴포넌트를 가져온다 */
    private T RaycastAndGetFirstComponent<T>() where T : Component
    {
        RayResult.Clear();

        GraphicRay.Raycast(PointerEvent, RayResult);

        if(RayResult.Count == 0)
        {
            return null;
        }

        return RayResult[0].gameObject.GetComponent<T>();
    }

    /** 마우스 클릭을 했을 때 */
    private void OnPointerDown()
    {
        // 왼쪽 클릭을 누르는 순간
        if(Input.GetMouseButtonDown(0))
        { 
            BeginDragSlot = RaycastAndGetFirstComponent<ItemSlot>();

            // 현재 드래그 시작한 슬롯이 존재하고, 아이템이 있을 경우
            if (BeginDragSlot != null && BeginDragSlot.HasItem)
            {
                // 스크롤 막기
                Scroll.vertical = false;

                BeginDragIconTransform = BeginDragSlot.oItemIconImgRect;
                BeginDragIconPoint = BeginDragIconTransform.position;
                BeginDragCursorPoint = Input.mousePosition;

                BeginDragIconTransform.transform.SetParent(Content.transform);
                DragSlotSiblingIndex = BeginDragSlot.transform.GetSiblingIndex();
                BeginDragIconTransform.transform.SetAsLastSibling();
            }
            else
            {
                BeginDragSlot = null;
            }
        }
        // 오른쪽 클릭을 누르는 순간, 왼쪽 컨트롤을 안눌렀을때
        else if(Input.GetMouseButtonDown(1) && !Input.GetKey(KeyCode.LeftControl))
        {
            ItemSlot Slot = RaycastAndGetFirstComponent<ItemSlot>();

            if (Slot != null && Slot.HasItem)
            {
                // 아이템 사용 
                InventoryUseItem(() => Slot.UseItem(), Slot);
            }
        }
        // 왼쪽 컨트롤과, 오른쪽 클릭을 눌렀을 경우
        else if(Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(1))
        {
            ItemSlot Slot = RaycastAndGetFirstComponent<ItemSlot>();

            if (Slot != null && Slot.HasItem)
            {
                // 아이템 버리기
                InventoryAbandonItem(() => Slot.AbandonItem(), Slot);
            }
        }
    }

    /** 마우스 드래그를 했을 때 */
    private void OnPointerDrag()
    {
        // 현재 드래그 시작한 슬롯이 없을 경우
        if(BeginDragSlot == null)
        {
            return;
        }

        // 왼쪽 클릭을 누르는 동안
        if (Input.GetMouseButton(0))
        {
            // 해당 슬롯의 아이콘 위치 변경
            BeginDragIconTransform.position =
                BeginDragIconPoint + (Input.mousePosition - BeginDragCursorPoint);
        }
    }

    /** 마우스 클릭을 뗄 경우 */
    private void OnPointerUp()
    {
        // 왼쪽 클릭을 눌렀다 때는 순간
        if(Input.GetMouseButtonUp(0))
        {
            // 현재 드래그 시작한 슬롯이 존재할 경우
            if (BeginDragSlot != null)
            {
                // 원래 위치로

                BeginDragIconTransform.transform.SetParent(BeginDragSlot.transform);
                BeginDragIconTransform.localPosition = Vector3.zero;
                
                BeginDragSlot.transform.SetSiblingIndex(DragSlotSiblingIndex);

                EndDrag();

                BeginDragSlot = null;
                BeginDragIconTransform = null;

                // 스크롤 허용
                Scroll.vertical = true;
            }
        }
    }

    /** 드래그를 종료한다 */
    private void EndDrag()
    {
        ItemSlot EndDragSlot = RaycastAndGetFirstComponent<ItemSlot>();

        // 드래그가 끝난시점에 슬롯이 있을 경우
        if(EndDragSlot != null)
        {
            // 처음 시작한 슬롯과, 마지막 슬롯이 같을 경우
            if(BeginDragSlot == EndDragSlot)
            {
                return;
            }

            // 슬롯에 있는 아이템을 옮긴다
            BeginDragSlot.SlotSwap(EndDragSlot);
        }
    }

    /** 인벤토리에서 아이템을 사용하는 팝업, 콜백 설정 */
    private void InventoryUseItem(Action Callback, ItemSlot Slot)
    {
        PopupPanel.SetActive(true);
        UsePanel.SetActive(true);
        UseItemNameText.text = Slot.oItemInfo.ItemName;
        UseInfoText.text = "사용하시겠습니까?";
        PopupUseOkCallback = Callback;
    }

    /** 인벤토리에서 아이템을 버리는 팝업, 콜백 설정 */
    private void InventoryAbandonItem(Action Callback, ItemSlot Slot)
    {
        PopupPanel.SetActive(true);
        AbandonPanel.SetActive(true);
        AbandonItemNameText.text = Slot.oItemInfo.ItemName;
        AbandonInfoText.text = "버리시겠습니까?";
        PopupAbandonOkCallback = Callback;
    }
    #endregion // 함수
}
