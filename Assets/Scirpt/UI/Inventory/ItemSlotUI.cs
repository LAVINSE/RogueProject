using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    #region 변수
    [SerializeField] private Image IconImage = null; // 아이템 아이콘 이미지
    [SerializeField] private TMP_Text ItemAmountText = null; // 아이템 개수 텍스트
    [SerializeField] private Image HighlightImage = null; // 아이콘 하이라이트 이미지

    [Space]
    [SerializeField] private float HighlightImageAlpha = 0.0f; // 하이라이트 이미지 알파값
    [SerializeField] private float HighlightImageFadeDuration = 0.0f; // 하이라이트 Fade 소요시간

    private InventoryUI oInventoryUI;

    private RectTransform SlotRect;
    private RectTransform IconRect;
    private RectTransform HighlightRect;

    private GameObject IconObject;
    private GameObject TextObject;
    private GameObject HighlightObject;

    private Image SlotImage;

    private float CurrentHighlightAlpha = 0.0f; // 현재 하이라이트 알파값

    private bool IsAccessSlot = true; // 슬롯 접근가능 여부
    private bool IsAccessItem = true; // 아이템 접근가능 여부

    // 비활성화 된 슬롯의 색상
    private static readonly Color ActiveFalseSlotColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);
    // 비활성화 된 아이콘 색상
    private static readonly Color ActiveFalseIconColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    #endregion // 변수

    #region 프로퍼티
    public int Index { get; private set; } // 슬롯의 인덱스
    public bool HasItem => IconImage.sprite != null; // 슬롯이 아이템을 가지고있는지 확인하는 변수
    public bool IsAccess => IsAccessSlot && IsAccessItem; // 접근 가능한 슬롯인지 확인하는 변수

    public RectTransform oSlotRect => SlotRect;
    public RectTransform oIconRect => IconRect;
    #endregion // 프로퍼티

    #region 함수
    /** 아이콘을 보여준다*/
    private void ShowIcon() => IconObject.SetActive(true);
    /** 아이콘을 숨긴다 */
    private void HideIcon() => IconObject.SetActive(false);

    /** 텍스트를 보여준다 */
    private void ShowText() => TextObject.SetActive(true);
    /** 텍스트를 숨긴다 */
    private void HideText() => TextObject.SetActive(false);

    public void SetSlotIndex(int Index) => this.Index = Index; // 몇번째 슬롯인지 인덱스 저장

    /** 슬롯 자체의 활성화 / 비활성화 여부 설정 */
    public void SetSlotAccessState(bool IsAccess)
    {
        // 중복, 예외처리
        if(this.IsAccess == IsAccess)
        {
            return;
        }

        // 활성화 상태 일 경우
        if(IsAccess == true)
        {
            SlotImage.color = Color.black;
        }
        else
        {
            SlotImage.color = ActiveFalseSlotColor;
            HideIcon();
            HideText();
        }

        this.IsAccessSlot = IsAccess;
    }
    #endregion // 함수

}
