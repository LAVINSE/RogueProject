using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

#region 키 종류
public enum UserKeyAction
{
    Jump = 0, // C
    Pickup, // Z
    Inventory, // I
    Interaction, // F
    Skill_Q, // Q
    KeyCount,
}
#endregion // 키 종류

public class KeyManager : CSingleton<KeyManager>
{
    #region 기본 키 세팅
    private KeyCode[] DefalutKeys = new KeyCode[]
    {
        KeyCode.C,
        KeyCode.Z,
        KeyCode.I,
        KeyCode.F,
        KeyCode.Q,
    };
    #endregion // 기본 키 세팅

    #region 변수
    private int Key = -1;
    #endregion // 변수

    #region 함수
    /** 초기화 */
    public override void Awake()
    {
        base.Awake();

        for(int i = 0; i< (int)UserKeyAction.KeyCount; i++)
        {
            KeySetting.Keys.Add((UserKeyAction)i, DefalutKeys[i]);
        }
    }

    /** 랜더링 및 GUI 이벤트 처리 */
    private void OnGUI()
    {
        if(CSceneManager.IsDestroy) return;

        // 설정창이 활성화 상태일 경우
        if(CSceneManager.Instance.OptionObj.activeSelf == true)
        {
            // OnGUI를 호출하게 된 이벤트
            Event KeyEvent = Event.current;

            // 키 입력일 경우
            if (KeyEvent.isKey && Key != -1)
            {
                // 이미 세팅되어있는 Key일 경우
                if (KeySetting.Keys.ContainsValue(KeyEvent.keyCode))
                {
                    var ButtonKey = KeySetting.Keys[(UserKeyAction)Key];
                    var InputKey = KeyEvent.keyCode;
                    var InputKey_Key = KeySetting.Keys.FirstOrDefault(x => x.Value == InputKey).Key;

                    KeySetting.Keys[(UserKeyAction)Key] = InputKey;
                    KeySetting.Keys[(InputKey_Key)] = ButtonKey;
                    
                    Key = -1;
                    return;
                }
                else
                {
                    KeySetting.Keys[(UserKeyAction)Key] = KeyEvent.keyCode;
                    Key = -1;
                    return;
                }
            }
        } 
    }

    /** 키를 변경한다 */
    public void ChangeKey(int Num)
    {
        Key = Num;
    }
    #endregion // 함수
}

public static class KeySetting
{
    public static Dictionary<UserKeyAction, KeyCode> Keys = new Dictionary<UserKeyAction, KeyCode>();
}
