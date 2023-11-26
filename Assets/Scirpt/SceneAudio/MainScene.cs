using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    /** 초기화 */
    private void Awake()
    {
        AudioManager.Inst.StopBGM();
        AudioManager.Inst.PlayBGM(BGMEnum.MainBGM);
    }
}
