using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    #region 변수
    [SerializeField] private Image SlotImg;
    [SerializeField] private Image ItemIconImg;

    private ItemInfoTable Item;
    private RectTransform ItemIconImgRect;
    #endregion // 변수

    #region 프로퍼티
    public ItemInfoTable oItemInfo
    {
        get { return Item; }
        set
        {
            Item = value;
            // 아이템이 있을 경우
            if (Item != null)
            {
                ItemIconImg.sprite = Item.ItemImage;
                // 표시
                ItemIconImg.color = new Color(1, 1, 1, 1);
            }
            else
            {
                // 표시 X
                ItemIconImg.color = new Color(1, 1, 1, 0);
            }
        }
    }

    public bool HasItem => ItemIconImg != null;
    public RectTransform oItemIconImgRect => ItemIconImgRect;
    #endregion // 프로퍼티

    #region 함수
    private void Awake()
    {
        ItemIconImgRect = ItemIconImg.rectTransform;
    }
    #endregion // 함수
}