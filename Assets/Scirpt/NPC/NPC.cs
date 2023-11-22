using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public enum NPCType
    {
        None,
        ShopNPC,
        StageNPC,
    }

    #region 변수
    [Header("=====> NPC 설정 <=====")]
    [SerializeField] private NPCType Type = NPCType.None;
    [Tooltip(" 상점 NPC만 사용함 ")][SerializeField] private WeightDropTable ItemTable = null;

    [Header("=====> 패널 설정 <=====")]
    [SerializeField] private GameObject InteractionPanel = null;
    [SerializeField] private TMP_Text InteractionText = null;
    

    private Player PlayerData;
    private GameObject ShopObj;
    private GameObject DungeonPopupObj;
    #endregion // 변수

    #region 함수
    /** 초기화 => 처음 접촉했을 때 */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어와 접촉중일 경우
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerData = collision.gameObject.GetComponent<Player>();
            
            // 패널 보여주기
            ShowInteractionPanel();

            // 플레이어 데이터가 있을 경우
            if (PlayerData != null )
            {
                PlayerData.oNPC = this;
            }
        }
    }

    /** 초기화 => 처음 접촉이 끝났을 경우 */
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 패널 닫기
            ShowInteractionPanel();

            // 상점이 있을 경우
            if(ShopObj != null)
            {
                // 상점 닫기
                ShopObj.SetActive(false);
            }
            
            // 플레이어 데이터가 있을 경우
            if (PlayerData != null )
            {
                PlayerData.oNPC = null;
                PlayerData = null;
            }
        }
    }

    /** 상호작용 패널을 설정한다 */
    private void ShowInteractionPanel()
    {
        if(InteractionPanel.activeSelf == false)
        {
            InteractionPanel.SetActive(true);
        }
        else
        {
            InteractionPanel.SetActive(false);
        }
    }

    // NPC 상호작용
    public void NPCInteraction()
    {
        switch(Type)
        {
            case NPCType.None:
                Debug.Log("a");
                break;
            case NPCType.ShopNPC:
                // 상점 열기
                ShowShop();
                break;
            case NPCType.StageNPC:
                ShowDungeonPopup();
                break;
        }
    }

    /** 상점을 보여준다 */
    private void ShowShop()
    {
        // 아이템테이블이 없을경우
        if (ItemTable == null)
        {
            return;
        }

        var ShopComponent = CSceneManager.Instance.PublicRoot.GetComponentInChildren<Shop>(true);

        // Shop이 없을 경우
        if (ShopComponent == null)
        {
            ShopObj = CSceneManager.Instance.CreateShop().gameObject;
            ShopObj.GetComponent<Shop>().SettingShop(ItemTable);
            ShopObj.SetActive(true);
        }
        else
        {
            if (ShopObj.activeSelf == true)
            {
                ShopObj.SetActive(false);
            }
            else
            {
                ShopObj.SetActive(true);
            }
        }
    }

    /** 던전 입장 팝업을 보여준다 */
    private void ShowDungeonPopup()
    {
        var PopupComponent = CSceneManager.Instance.PublicRoot.GetComponentInChildren<DungeonPopup>(true);

        if(PopupComponent == null)
        {
            DungeonPopupObj = CSceneManager.Instance.CreateDungeonPopup().gameObject;
            DungeonPopupObj.SetActive(true);
        }
        else
        {
            if (DungeonPopupObj.activeSelf == true)
            {
                DungeonPopupObj.SetActive(false);
            }
            else
            {
                DungeonPopupObj.SetActive(true);
            }
        }
    }
    #endregion // 함수
}
