using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, ItemAdd
{
    #region 변수
    [Header("=====> 아이템 <=====")]
    [SerializeField] private ItemInfo Item = null;

    private SpriteRenderer ItemImg;
    #endregion // 변수

    #region 함수
    /** 초기화 */
    private void Awake()
    {
        ItemImg = GetComponent<SpriteRenderer>();
    }

    /** 초기화 */
    private void Start()
    {
        SettingItem();
    }

    /** 아이템을 세팅한다 */
    private void SettingItem()
    {
        ItemImg.sprite = Item.ItemImage;
    }
    
    public ItemInfo ItemAdd()
    {
        return this.Item;
    }
    #endregion // 함수
}
