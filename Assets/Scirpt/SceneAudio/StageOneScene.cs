using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageOneScene : MonoBehaviour
{
    #region 변수
    [SerializeField] private GameObject StageNPC;
    #endregion // 변수

    #region 함수
    /** 초기화 */
    private void Awake()
    {
        AudioManager.Inst.StopBGM();
        AudioManager.Inst.PlayBGM(BGMEnum.StageOneBGM);

        StageNPC.SetActive(false);
    }

    /** 초기화 => 상태를 갱신한다 */
    private void Update()
    {
        if(EnemySpawnerManager.Instance.ClearCount == 0)
        {
            StageNPC.SetActive(true);
        }
    }
    #endregion // 함수
}
