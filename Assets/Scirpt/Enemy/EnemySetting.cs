using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySetting : MonoBehaviour
{
    #region 변수
    [SerializeField] private EnemyTable.EnemyType oEnemyType;
    [SerializeField] private float EnemyHp = 0.0f;
    [SerializeField] private float EnemyAtk = 0.0f;

    [SerializeField] private EnemyTable oEnemyTable;
    #endregion // 변수

    #region 함수
    /** 적 정보를 세팅한다 */
    public void EnemyInfoSetting(EnemyTable EnemyScriptTable)
    {
        this.oEnemyTable = EnemyScriptTable;
        this.oEnemyType = this.oEnemyTable.oEnemyType;
        this.EnemyHp = this.oEnemyTable.EnemyHp;
        this.EnemyAtk = this.oEnemyTable.EnemyAtk;
    }

    /** 적이 죽었을 때 */
    private void EnemyDie()
    {
        StageManager.Instance.PoolManager.DeSpawnObj<EnemySetting>(this.gameObject, CompleteDespawn);
    }

    /** 적 비활성화가 완료 되었을 경우 */
    private void CompleteDespawn(object Obj)
    {
        (Obj as GameObject).SetActive(false);
    }
    #endregion // 함수
}
