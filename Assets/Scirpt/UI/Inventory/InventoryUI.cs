using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
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

    private List<ItemSlotUI> ItemSlotUIList = new List<ItemSlotUI>();
    #endregion // 변수

    #region 함수
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

   
    #endregion // 함수
}
