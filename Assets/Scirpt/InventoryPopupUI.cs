using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryPopupUI : MonoBehaviour
{
    #region 변수
    [Header("=====> 아이템 버리기 패널 <=====")]
    [SerializeField] private GameObject AbandonPopupObject = null;
    [SerializeField] private TMP_Text AbandonItemNameText = null;
    [SerializeField] private TMP_Text AbandonInfoText = null;
    [SerializeField] private Button AbandonOKButton = null;
    [SerializeField] private Button AbandonCancelButton = null;

    [Header("=====> 아이템 수량나누기 패널 <=====")]
    [SerializeField] private GameObject AmountPopupObject = null;
    [SerializeField] private TMP_Text AmountItemNameText = null;
    [SerializeField] private TMP_Text AmountInfoText = null;
    [SerializeField] private TMP_InputField AmountInputField = null;
    [SerializeField] private Button AmountMinusButton = null;
    [SerializeField] private Button AmountPlusButton = null;
    [SerializeField] private Button AmountOKButton = null;
    [SerializeField] private Button AmountCancelButton = null;

    private event Action OnAbandonInputOK;
    private event Action<int> OnAmountInputOK;

    private int MaxAmount; // 최대 수량
    #endregion // 변수

    #region 함수
    /** 초기화 */
    private void Awake()
    {
        // 세팅
        AbandonPopupButtonSetting();
        AmountPopupButtonSetting();
        AmountInputFieldSetting();

        // 숨기기
        HidePanel();
        HideAbandonPopupObject();
        HideAmountPopupObject();
    }

    /** 초기화 => 상태를 갱신한다 */
    private void Update()
    {
        KeyDownEnterReturn();
    }

    /** 인벤토리 팝업 UI를 설정한다 */
    private void AbandonPopupButtonSetting()
    {
        // 확인 버튼
        // 이벤트 호출, 팝업 비활성화
        AbandonOKButton.onClick.AddListener(HidePanel);
        AbandonOKButton.onClick.AddListener(HideAbandonPopupObject);
        AbandonOKButton.onClick.AddListener(() => OnAbandonInputOK?.Invoke());
        
        // 취소 버튼
        // 팝업 비활성화
        AbandonCancelButton.onClick.AddListener(HidePanel);
        AbandonCancelButton.onClick.AddListener(HideAbandonPopupObject);    
    }

    /** 인벤토리 팝업 UI를 설정한다 */
    private void AmountPopupButtonSetting()
    {
        // 확인 버튼
        // 이벤트 호출, 팝업 비활성화
        AmountOKButton.onClick.AddListener(HidePanel);
        AmountOKButton.onClick.AddListener(HideAmountPopupObject);
        AmountOKButton.onClick.AddListener(() => OnAmountInputOK?.Invoke(int.Parse(AmountInfoText.text)));

        // 취소 버튼
        // 팝업 비활성화
        AmountCancelButton.onClick.AddListener(HidePanel);
        AmountCancelButton.onClick.AddListener(HideAmountPopupObject);

        // - 버튼
        AmountMinusButton.onClick.AddListener(() =>
        {
            int.TryParse(AmountInputField.text, out int Amount);
            if (Amount > 1)
            {
                // 왼쪽 쉬프트 눌러서 클릭할 경우 -10, 아닐 경우 -1
                int ShiftAmount = Input.GetKey(KeyCode.LeftShift) ? Amount - 10 : Amount - 1;

                // 개수가 1 미만일 경우
                if (ShiftAmount < 1)
                {
                    ShiftAmount = 1;
                }
                AmountInputField.text = ShiftAmount.ToString();
            }
        });

        // + 버튼
        AmountPlusButton.onClick.AddListener(() =>
        {
            int.TryParse(AmountInputField.text, out int Amount);
            if (Amount < MaxAmount)
            {
                // 왼쪽 쉬프트 눌러서 클릭할 경우 +10, 아닐 경우 +1
                int ShiftAmount = Input.GetKey(KeyCode.LeftShift) ? Amount + 10 : Amount + 1;

                // 개수가 최대 개수보다 많을 경우
                if (ShiftAmount > MaxAmount)
                {
                    ShiftAmount = MaxAmount;
                }
                AmountInputField.text = ShiftAmount.ToString();
            }
        });
    }

    /** 인벤토리 팝업 UI를 설정한다 */
    private void AmountInputFieldSetting()
    {
        // InputField
        // 입력값 범위 제한
        AmountInputField.onValueChanged.AddListener(InputStirng =>
        {
            int.TryParse(InputStirng, out int Amount);
            bool IsLimit = false;

            if (Amount < 1)
            {
                IsLimit = true;
                Amount = 1;
            }
            else if (Amount > MaxAmount)
            {
                IsLimit = true;
                Amount = MaxAmount;
            }

            if (IsLimit == true)
            {
                AmountInputField.text = Amount.ToString();
            }

        });
    }

    /** Enter, Esc를 눌렀을 경우 */
    private void KeyDownEnterReturn()
    {
        // 수량 팝업이 활성화 됐을 경우
        if (AmountPopupObject.activeSelf)
        {
            // 엔터를 누를 경우
            if (Input.GetKeyDown(KeyCode.Return))
            {
                AmountOKButton.onClick?.Invoke();
            }
            // 뒤로가기를 누를 경우
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                AmountCancelButton.onClick?.Invoke();
            }
        }
        // 아이템 버리기 패널이 활성화 됐을 경우
        else if (AbandonPopupObject.activeSelf)
        {
            // Enter 를 누를 경우
            if (Input.GetKeyDown(KeyCode.Return))
            {
                AbandonOKButton.onClick?.Invoke();
            }
            // Esc 를 누를 경우
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                AbandonCancelButton.onClick?.Invoke();
            }
        }
    }

    /** 패널을 보여준다 */
    private void ShowPanel() => gameObject.SetActive(true);
    /** 패널을 숨긴다 */
    private void HidePanel() => gameObject.SetActive(false);
    /** 팝업 오브젝트를 숨긴다 */
    private void HideAbandonPopupObject() => AbandonPopupObject.SetActive(false);

    private void HideAmountPopupObject() => AmountPopupObject.SetActive(false);

    /** 버리기 패널 확인 버튼을 눌렀을 때 */
    private void SetAbandonOKEvent(Action Handler) => OnAbandonInputOK = Handler;

    /** 수량 팝업 패널 확인 버튼을 눌렀을 때 */
    private void SetAmountOKEvent(Action<int> Handler) => OnAmountInputOK = Handler;

    /** 버리기 팝업 패널을 보여준다 */
    private void ShowAbandonPopup(string ItemName)
    {
        AbandonItemNameText.text = ItemName;
        AbandonPopupObject.SetActive(true);
    }

    /** 수량 팝업 패널을 보여준다 */
    private void ShowAmountPopup(string ItemName)
    {
        AmountItemNameText.text = ItemName;
        AmountPopupObject.SetActive(true);
    }

    /** 수량 입력 팝업을 보여준다 */
    public void ShowAmountInputPopup(Action<int> OKCallback, int CurrentAmount, string ItemName)
    {
        MaxAmount = CurrentAmount - 1;
        AmountInputField.text = "1";

        ShowPanel();
        ShowAmountPopup(ItemName);
        SetAmountOKEvent(OKCallback);
    }

    /** 확인/취소 팝업을 보여준다 */
    public void ShowOKCancel(Action OKCallback, string ItemName)
    {
        ShowPanel();
        ShowAbandonPopup(ItemName);
        SetAbandonOKEvent(OKCallback);
    }
    #endregion // 함수
}
