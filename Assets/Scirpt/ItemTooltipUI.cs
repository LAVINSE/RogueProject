using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltipUI : MonoBehaviour
{
    #region 변수
    [SerializeField] private TMP_Text ItemName_Text;
    [SerializeField] private TMP_Text ItemTooltip_Text;

    // 툴팁 위치 조정에 필요한 변수
    private RectTransform Rect;
    private CanvasScaler CanScaler;
    #endregion // 변수

    #region 함수
    /** 초기화 */
    private void Awake()
    {
        TooltipSetting();
        Hide();
    }

    /** 툴팁을 보여준다 */
    public void Show() => gameObject.SetActive(true);

    /** 툴팁을 숨긴다 */
    public void Hide() => gameObject.SetActive(false);

    /** 툴팁 UI를 세팅한다 */
    private void TooltipSetting()
    {
        TryGetComponent(out Rect);
        Rect.pivot = new Vector2(0f, 1f); // 좌 상단 위치
        CanScaler = GetComponentInParent<CanvasScaler>();

        DisableAllChildrenRaycastTarget(transform);
    }

    /** 모든 자식 UI에 레이캐스트 타겟 해제 */
    private void DisableAllChildrenRaycastTarget(Transform oTransform)
    {
        // 본인이 Graphic(UI)를 상속하면 레이캐스트 타겟 해제
        oTransform.TryGetComponent(out Graphic oGraphic);
        if(oGraphic != null)
        {
            oGraphic.raycastTarget = false;
        }

        // 자식이 없으면 종료
        int ChildCount = oTransform.childCount;
        if(ChildCount == 0)
        {
            return;
        }

        for(int i = 0; i < ChildCount; i++)
        {
            DisableAllChildrenRaycastTarget(oTransform.GetChild(i));
        }
    }

    /** 툴팁 UI에 아이템 정보 등록 */
    public void SetItemInfo(ItemData Data)
    {
        ItemName_Text.text = Data.oItemName;
        ItemTooltip_Text.text = Data.oItemInfo;
    }
    #endregion // 함수
}
