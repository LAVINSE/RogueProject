using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, ItemAdd
{
    #region 변수
    [Header("=====> 아이템 <=====")]
    [SerializeField] private ItemInfoTable Item = null;

    private SpriteRenderer ItemImg;
    private Rigidbody2D Rigid;
    private BoxCollider2D BoxCollider;
    #endregion // 변수

    #region 함수
    /** 초기화 */
    private void Awake()
    {
        ItemImg = GetComponent<SpriteRenderer>();
        Rigid = GetComponent<Rigidbody2D>();
        BoxCollider = GetComponent<BoxCollider2D>();
    }

    /** 초기화 */
    private void Start()
    {
        SettingItem();
    }

    /** 초기화 => 처음 접촉했을 때 */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Map"))
        {
            Rigid.bodyType = RigidbodyType2D.Kinematic;
            BoxCollider.isTrigger = true;
        }
    }

    /** 아이템을 세팅한다 */
    private void SettingItem()
    {
        ItemImg.sprite = Item.ItemImage;
    }
    
    public ItemInfoTable ItemAdd()
    {
        return this.Item;
    }
    #endregion // 함수
}
