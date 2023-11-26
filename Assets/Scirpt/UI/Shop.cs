using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    #region 변수
    [Header("=====> 설정 <=====")]
    [SerializeField] private List<Image> ItemImgList = new List<Image>();
    [SerializeField] private List<TMP_Text> ItemNameList = new List<TMP_Text>();
    [SerializeField] private List<TMP_Text> ItemPriceList = new List<TMP_Text>();
    [SerializeField] private List<Button> ItemButtonList = new List<Button>();
    [SerializeField] private List<GameObject> ShopItemObjectList = new List<GameObject>();

    [Space]
    [SerializeField] private TMP_Text PlayerGoldText;
    [SerializeField] private Button CancelButton;

    [Space]
    [Header("=====> 인스펙터 확인용 <=====")]
    [SerializeField] private WeightDropTable DropTable;

    private Player PlayerComponent;
    #endregion // 변수

    #region 프로퍼티
    #endregion // 프로퍼티

    #region 함수
    /** 초기화 */
    private void Awake()
    {
        PlayerComponent = CSceneManager.Instance.PlayerObj.GetComponent<Player>();

        CancelButton.onClick.AddListener(() =>
        {
            this.gameObject.SetActive(false);
        });
    }

    /** 초기화 */
    private void Update()
    {
        // 상점이 활성화 상태일 경우
        if(this.gameObject.activeSelf == true)
        {
            PlayerGoldText.text = "골드 : " + PlayerComponent.oPlayerCurrentGold.ToString();

            for(int i = 0; i< ItemImgList.Count; i++)
            {
                if (ItemButtonList[i].interactable == false)
                {
                    ItemNameList[i].text = "Sold Out";
                    // TODO : 이미지 교체
                }
            }
        }
    }

    /** 상점을 세팅한다 */
    public void SettingShop(WeightDropTable ItemTable)
    {
        DropTable = ItemTable;

        for (int i = 0; i< ItemImgList.Count; i++)
        {
            int Index = i;
            var Drop = DropTable.ItemDrop();

            // 드랍 아이템이 있을 경우
            if(Drop != null)
            {
                ItemImgList[i].sprite = Drop.ItemImage;
                ItemNameList[i].text = Drop.ItemName;
                ItemPriceList[i].text = "Gold : " + Drop.ItemPrice.ToString();

                ItemButtonList[i].onClick.AddListener(() =>
                {
                    // 아이템을 살 수 있을 경우
                    if (PlayerComponent.oPlayerCurrentGold >= Drop.ItemPrice)
                    {
                        // 아이템 가격만큼 빼기
                        PlayerComponent.oPlayerCurrentGold -= Drop.ItemPrice;
                        ItemInfoTable Item = Drop.ItemPrefab.GetComponent<ItemObject>().ItemAdd();
                        Inventory.Instance.AddItem(Item);
                        ItemButtonList[Index].interactable = false;
                    }
                    else
                    {
                        return;
                    }
                });
            }
        }

        for(int i = 0; i < ItemImgList.Count; i++)
        {
            if (ItemImgList[i].sprite == null)
            {
                ShopItemObjectList[i].SetActive(false);
            }
        } 
    }
    #endregion // 함수
}
