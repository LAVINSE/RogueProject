using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionButton : MonoBehaviour
{
    #region 변수
    private Button OptionClickButton;
    #endregion // 변수

    #region 함수
    /** 초기화 */
    private void Awake()
    {
        OptionClickButton = GetComponent<Button>();
        OptionClickButton.onClick.AddListener(OnClickOptionButton);
    }

    /** 버튼을 눌렀을 때 */
    private void OnClickOptionButton()
    {
        if(CSceneManager.Instance.OptionObj != null)
        {
            if(CSceneManager.Instance.OptionObj.activeSelf == true)
            {
                CSceneManager.Instance.OptionObj.SetActive(false);
            }
            else
            {
                CSceneManager.Instance.OptionObj.SetActive(true);
            }
        }
        else
        {
            Debug.Log("설정창이 존재하지 않습니다");
            return;
        }
    }
    #endregion // 함수
}
