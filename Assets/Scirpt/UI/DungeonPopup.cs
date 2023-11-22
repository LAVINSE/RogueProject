using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonPopup : MonoBehaviour
{
    #region 변수
    [SerializeField] private Button OkButton = null;
    [SerializeField] private Button CancelButton = null;
    #endregion // 변수

    #region 함수
    /** 초기화 */
    private void Awake()
    {
        OkButton.onClick.AddListener(() =>
        {
            GameManager.Inst.ChangeScene("Stage1");
        });

        CancelButton.onClick.AddListener(() =>
        {
            this.gameObject.SetActive(false);
        });
    }
    #endregion // 함수
}
