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
    [SerializeField] private NPCType Type = NPCType.None;
    [SerializeField] private GameObject InteractionPanel = null;
    [SerializeField] private TMP_Text InteractionText = null;

    private Player PlayerData;
    #endregion // 변수

    #region 함수
    /** 초기화 => 처음 접촉했을 때 */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어와 접촉중일 경우
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerData = collision.gameObject.GetComponent<Player>();
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
            ShowInteractionPanel();
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
        // switch문으로 관리 or if문
        Debug.Log("asdf");
    }
    #endregion // 함수
}
