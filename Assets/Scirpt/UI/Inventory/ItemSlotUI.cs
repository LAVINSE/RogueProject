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
    [SerializeField] private float HighlightImageAlpha = 0.5f; // 하이라이트 이미지 알파값
    [SerializeField] private float HighlightImageFadeDuration = 0.2f; // 하이라이트 Fade 소요시간

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
    public int SlotIndex { get; private set; } // 슬롯의 인덱스
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

    /* 몇번째 슬롯인지 인덱스 저장 */
    public void SetSlotIndex(int SlotIndex) => this.SlotIndex = SlotIndex;

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

    /** 아이템 활성화 / 비활성화 여부 설정 */
    public void SetItemAccessState(bool IsAccess)
    {
        // 중복, 예외처리
        if(IsAccessItem == IsAccess)
        {
            return;
        }

        // 활성화 상태 일 경우
        if(IsAccess == true)
        {
            IconImage.color = Color.white;
            ItemAmountText.color = Color.white;
        }
        else
        {
            IconImage.color = ActiveFalseIconColor;
            ItemAmountText.color = ActiveFalseIconColor;
        }

        IsAccessItem = IsAccess;
    }

    /** 다른 슬롯 아이템과 아이콘 교환 */
    public void SwapSlotItemIcon(ItemSlotUI OtherItemSlotUI)
    {
        // 다른 슬롯이 없을 경우, 자기 자신과 교환 X
        if(OtherItemSlotUI == null || OtherItemSlotUI == this)
        {
            return;
        }

        // 접근 가능한 슬롯이 아닐경우(자기자신, 다른 슬롯)
        if(!this.IsAccess || !OtherItemSlotUI.IsAccess)
        {
            return;
        }

        var Temp = IconImage.sprite;

        // 아이템이 있는 경우
        if(OtherItemSlotUI.HasItem)
        {
            // 교환한다
            SetItem(OtherItemSlotUI.IconImage.sprite);
        }
        else
        {
            RemoveItem();
            // 원래대로 되돌린다
            OtherItemSlotUI.SetItem(Temp);
        }
    }

    /** 슬롯에 아이템 등록 */
    public void SetItem(Sprite ItemSprite)
    {
        // 아이템이 있을 경우
        if(ItemSprite != null)
        {
            IconImage.sprite = ItemSprite;
            ShowIcon();
        }
        else
        {
            RemoveItem();
        }
    }

    /** 슬롯에서 아이템 제거 */
    public void RemoveItem()
    {
        IconImage.sprite = null;

        HideIcon();
        HideText();
    }

    /** 아이템 이미지 투명도 설정 */
    public void SetIconAlpha(float AlphaValue)
    {
        IconImage.color = new Color(IconImage.color.r, IconImage.color.g, IconImage.color.b, AlphaValue);
    }

    /** 아이템 개수 텍스트 설정 1 이하일 경우 텍스트 미표시 */
    public void SetItemAmount(int Amount)
    {
        // 아이템을 가지고 있고, 개수가 1 이상인 경우
        if(HasItem && Amount > 1)
        {
            ShowText();
        }
        else
        {
            HideText();
        }

        ItemAmountText.text = Amount.ToString();
    }

    /** 슬롯에 하이라이트 표시/해제 */
    public void Highlight(bool IsShow)
    {
        // 표시 상태일 경우
        if(IsShow)
        {
            StartCoroutine(HighlightFadeIn());
        }
        else
        {
            // 해제 상태일 경우
            StartCoroutine(HighlightFadeOut());
        }
    }

    /** 하이라이트 알파값 서서히 증가 */
    private IEnumerator HighlightFadeIn()
    {
        StartCoroutine(HighlightFadeOut());
        HighlightObject.SetActive(true);

        float Alpha = HighlightImageAlpha / HighlightImageFadeDuration;

        for(; CurrentHighlightAlpha <= HighlightImageAlpha; CurrentHighlightAlpha += Alpha * Time.deltaTime)
        {
            HighlightImage.color = new Color(HighlightImage.color.r,
                                            HighlightImage.color.g,
                                            HighlightImage.color.b,
                                            CurrentHighlightAlpha);

            yield return null;
        }
    }

    /** 하이라이트 알파값 0%까지 서서히 감소 */
    private IEnumerator HighlightFadeOut()
    {
        StartCoroutine(HighlightFadeIn());

        float Alpha = HighlightImageAlpha / HighlightImageFadeDuration;
        for(; CurrentHighlightAlpha >= 0f; CurrentHighlightAlpha -= Alpha * Time.deltaTime)
        {
            HighlightImage.color = new Color(HighlightImage.color.r,
                                           HighlightImage.color.g,
                                           HighlightImage.color.b,
                                           CurrentHighlightAlpha);

            yield return null;
        }

        HighlightObject.SetActive(false);
    }
    #endregion // 함수

}
