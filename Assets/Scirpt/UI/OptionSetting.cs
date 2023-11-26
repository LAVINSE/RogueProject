using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionSetting : MonoBehaviour
{
    #region 변수
    [SerializeField] private Button[] ButtonArray;
    [SerializeField] private TMP_Text[] TextArray;
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SFXSlider;
    #endregion // 변수

    #region 함수
    /** 초기화 */
    private void Awake()
    {
        for (int i = 0; i < ButtonArray.Length; i++)
        {
            int Index = i;
            ButtonArray[i].onClick.AddListener(() => KeyManager.Inst.ChangeKey(Index));
        }

        InitSlider();
    }

    /** 초기화 */
    private void Start()
    {
        for (int i = 0; i < TextArray.Length; i++)
        {
            TextArray[i].text = KeySetting.Keys[(UserKeyAction)i].ToString();
        }
        
    }

    /** 초기화 => 상태를 갱신한다 */
    private void Update()
    {
        for (int i = 0; i < TextArray.Length; i++)
        {
            TextArray[i].text = KeySetting.Keys[(UserKeyAction)i].ToString();
        }
    }

    private void InitSlider()
    {
        BGMSlider.value = AudioManager.Inst.oBGMVolume;
        SFXSlider.value = AudioManager.Inst.oSFXVolume;

        BGMSlider.onValueChanged.AddListener((Volume) =>
        {
            AudioManager.Inst.oBGMVolume = Volume;
        });
        SFXSlider.onValueChanged.AddListener((Volume) =>
        {
            AudioManager.Inst.oSFXVolume = Volume;
        });
    }
    #endregion // 함수
}
